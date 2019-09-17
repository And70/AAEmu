﻿using System;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Models.Tasks.Skills;
using NLog;

namespace AAEmu.Game.Models.Game.Skills.Effects.SpecialEffects
{
    public class AutoAttack : ISpecialEffect
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
            // TODO start auto attack...
            _log.Warn("Special effects: AutoAttack");
            if (value1 > 0 && caster != null && target != null)
            {
                var autoAttackSkill = new Skill(SkillManager.Instance.GetSkillTemplate((uint)value1));
                
                TaskManager.Instance.Schedule(new Tasks.Skills.SkillUse(autoAttackSkill,
                        caster,
                        casterObj,
                        target,
                        targetObj,
                        skillObject),
                    TimeSpan.FromMilliseconds(value2));
            }
        }
    }
}
