﻿using AAEmu.Commons.Network;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Models.Game.Chat;
using AAEmu.Game.Models.StaticValues;

namespace AAEmu.Game.Core.Packets.G2C;

public class SCLeavedChatChannelPacket : GamePacket
{
    private readonly ChatType _type;
    private readonly short _subType;
    private readonly FactionsEnum _factionId;

    public SCLeavedChatChannelPacket(ChatType type, short subType, FactionsEnum factionId) : base(SCOffsets.SCLeavedChatChannelPacket, 1)
    {
        _type = type;
        _subType = subType;
        _factionId = factionId;
    }

    public override PacketStream Write(PacketStream stream)
    {
        stream.Write((short)_type);
        stream.Write(_subType);
        stream.Write((uint)_factionId);
        return stream;
    }
}
