﻿using AAEmu.Commons.Network;
using AAEmu.Game.Core.Network.Game;

namespace AAEmu.Game.Core.Packets.C2G;

<<<<<<<< HEAD:AAEmu.Game/Core/Packets/C2G/CSPremiumServieceMsgPacket.cs
//public class CSPremiumServieceMsgPacket : GamePacket
//{
//    public CSPremiumServieceMsgPacket() : base(CSOffsets.CSPremiumServieceMsgPacket, 5)
//    {
//    }
========
public class CSPremiumServiceMsgPacket : GamePacket
{
    public CSPremiumServiceMsgPacket() : base(CSOffsets.CSPremiumServiceMsgPacket, 5)
    {
    }

    public override void Read(PacketStream stream)
    {
        Logger.Debug("CSPremiumServiceMsgPacket");

        var stage = stream.ReadInt32();
        Logger.Info("PremiumServieceMsg, stage {0}", stage);
        Connection.SendPacket(new SCAccountWarnedPacket(2, "Premium ..."));
    }
}