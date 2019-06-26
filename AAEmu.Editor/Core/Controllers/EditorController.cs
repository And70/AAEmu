﻿using System;
using System.Collections.Generic;
using System.Linq;
using AAEmu.Commons.Utils;
using AAEmu.Editor.Core.Network.Connections;
using AAEmu.Editor.Core.Packets.E2C;
using AAEmu.Editor.Core.Packets.C2E;
using AAEmu.Editor.Utils;
using AAEmu.Editor.Core.Packets.L2C;
using AAEmu.Editor.Core.Packets.L2G;

namespace AAEmu.Editor.Core.Controllers
{
    public class EditorController : Singleton<EditorController>
    {
        private Dictionary<byte, Dictionary<uint, uint>> _tokens; // gsId, [token, accountId]

        protected EditorController()
        {
            _tokens = new Dictionary<byte, Dictionary<uint, uint>>();
        }

        /// <summary>
        /// Kr Method Auth
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="username"></param>
        public static void Editor(EditorConnection connection, string username)
        {
            using (var connect = MySQL.Create())
            {
                using (var command = connect.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM users where username=@username";
                    command.Parameters.AddWithValue("@username", username);
                    command.Prepare();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            connection.SendPacket(new ACEditorDeniedPacket(2));
                            return;
                        }

                        // TODO ... validation password

                        connection.AccountId = reader.GetUInt32("id");
                        connection.AccountName = username;
                        connection.LastEditor = DateTime.Now;
                        connection.LastIp = connection.Ip;

                        //connection.SendPacket(new ECEditorResponsePacket(0, "c:\\aa\\Archeage1.2\\game\\"));
                    }
                }
            }
        }

        /// <summary>
        /// Eu Method Auth
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void Editor(EditorConnection connection, string username, IEnumerable<byte> password)
        {
            using (var connect = MySQL.Create())
            {
                using (var command = connect.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM users where username=@username";
                    command.Parameters.AddWithValue("@username", username);
                    command.Prepare();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            connection.SendPacket(new ACEditorDeniedPacket(2));
                            return;
                        }

                        var pass = Convert.FromBase64String(reader.GetString("password"));
                        if (!pass.SequenceEqual(password))
                        {
                            connection.SendPacket(new ACEditorDeniedPacket(2));
                            return;
                        }

                        connection.AccountId = reader.GetUInt32("id");
                        connection.AccountName = username;
                        connection.LastEditor = DateTime.Now;
                        connection.LastIp = connection.Ip;

                        connection.SendPacket(new ACJoinResponsePacket(0, 6));
                        connection.SendPacket(new ACAuthResponsePacket(connection.AccountId, 6));
                    }
                }
            }
        }

        public void AddReconnectionToken(InternalConnection connection, byte gsId, uint accountId, uint token)
        {
            if (!_tokens.ContainsKey(gsId))
                _tokens.Add(gsId, new Dictionary<uint, uint>());

            _tokens[gsId].Add(token, accountId);
            connection.SendPacket(new LGPlayerReconnectPacket(token));
        }

        public void Reconnect(EditorConnection connection, byte gsId, uint accountId, uint token)
        {
            if (!_tokens.ContainsKey(gsId))
            {
                var parentId = GameController.Instance.GetParentId(gsId);
                if (parentId != null)
                    gsId = (byte)parentId;
                else
                {
                    // TODO ...
                    return;
                }
            }

            if (!_tokens[gsId].ContainsKey(token))
            {
                // TODO ...
                return;
            }

            if (_tokens[gsId][token] == accountId)
            {
                connection.AccountId = accountId;
                connection.SendPacket(new ACJoinResponsePacket(0, 6));
                connection.SendPacket(new ACAuthResponsePacket(connection.AccountId, 6));
            }
            else
            {
                // TODO ...
            }
        }
    }
}
