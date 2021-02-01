﻿using AAEmu.Commons.Network;
using AAEmu.Commons.Utils;
using AAEmu.Game.Core.Managers.World;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Models.Game.Gimmicks;
using AAEmu.Game.Models.Game.NPChar;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Models.Game.Units.Movements;
using AAEmu.Game.Models.Game.World;

namespace AAEmu.Game.Core.Packets.G2C
{
    public class SCUnitMovementsPacket : GamePacket // TODO ... SCOneMoveTypePacket
    {
        private (uint id, MoveType type)[] _movements;

        public SCUnitMovementsPacket((uint id, MoveType type)[] movements) : base(SCOffsets.SCUnitMovementsPacket, 5)
        {
            _movements = movements;
        }

        public override PacketStream Write(PacketStream stream)
        {
            stream.Write((ushort) _movements.Length); // TODO ... max size is 400
            foreach (var (id, type) in _movements)
            {
                // ---- test Ai_old ----
                var unit = WorldManager.Instance.GetUnit(id);
                if (unit is Npc || unit is Transfer || unit is Gimmick)
                {
                    var movementAction = new MovementAction(
                        new Point(type.X, type.Y, type.Z, Helpers.ConvertRadianToSbyteDirection(type.Rot.X), Helpers.ConvertRadianToSbyteDirection(type.Rot.Y), Helpers.ConvertRadianToSbyteDirection(type.Rot.Z)),
                        new Point(0, 0, 0),
                        (sbyte)type.Rot.Z,
                        3,
                        MoveTypeEnum.Unit
                    );
                    unit.VisibleAi.OwnerMoved(movementAction);
                }
                // ---- test Ai_old ----

                stream.WriteBc(id);
                stream.Write((byte)type.Type);
                stream.Write(type);
            }

            return stream;
        }
    }
}
