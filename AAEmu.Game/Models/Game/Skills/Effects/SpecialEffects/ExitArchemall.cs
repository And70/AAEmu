﻿using System;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Units;
using NLog;

namespace AAEmu.Game.Models.Game.Skills.Effects.SpecialEffects
{
    public class ExitArchemall : ISpecialEffect
    {
        private static Logger _log = LogManager.GetCurrentClassLogger();
        public void Execute(Unit caster,
            SkillCaster casterObj,
            BaseUnit target,
            SkillCastTarget targetObj,
            CastAction castObj,
            Skill skill,
            SkillObject skillObject,
            DateTime time,
            int value1,
            int value2,
            int value3,
            int value4)
        {
            if (caster is Character character)
            {
                character.DisabledSetPosition = true;

                character.SendPacket(
                    new SCLoadInstancePacket(
                        1,
                        character.WorldPosition.ZoneId,
                        character.WorldPosition.X,
                        character.WorldPosition.Y,
                        character.WorldPosition.Z,
                        0,
                        0,
                        0
                    )
                );

                character.InstanceId = 1; // TODO ....
                character.Position = character.WorldPosition.Clone();
                character.WorldPosition = null;
                _log.Warn("Special effects: ExitArchemall");
            }
        }
    }
}
