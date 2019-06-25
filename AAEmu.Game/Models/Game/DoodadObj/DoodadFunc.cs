﻿using System;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Managers.UnitManagers;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Units;
using AAEmu.Game.Utils;
using NLog;

namespace AAEmu.Game.Models.Game.DoodadObj
{
    public class DoodadFunc
    {
        public uint GroupId { get; set; }
        public uint FuncId { get; set; }
        public string FuncType { get; set; }
        public int NextPhase { get; set; }
        public uint SoundId { get; set; }
        public uint SkillId { get; set; }
        public uint PermId { get; set; }
        public int Count { get; set; }

        protected static Logger _log = LogManager.GetCurrentClassLogger();

        public async void Use(Unit caster, Doodad owner, uint skillId)
        {
            _log.Debug("DoodadFunc : GroupId {0}, FuncId {1}, FuncType {2}, NextPhase {3}, SoundId {4}, SkillId {5}, PermId {6}, Count {7}", GroupId, FuncId, FuncType, NextPhase, SoundId, SkillId, PermId, Count);

            owner.GrowthTime = DateTime.MinValue;
            var template = DoodadManager.Instance.GetFuncTemplate(FuncId, FuncType);
            if (template == null)
                return;

            owner.FuncGroupId = (uint)NextPhase;
            template.Use(caster, owner, skillId);
            if (NextPhase > 0)
            {
                if (owner.FuncTask != null)
                {
                    await owner.FuncTask.Cancel();
                    owner.FuncTask = null;
                }
                //owner.FuncGroupId = (uint)NextPhase;
                //owner.BroadcastPacket(new SCDoodadPhaseChangedPacket(owner), true); // TODO добавил для работы вкл/выкл освещения и разрушения бочек/ящиков
                var funcs = DoodadManager.Instance.GetPhaseFunc(owner.FuncGroupId);
                foreach (var func in funcs)
                {
                    func.Use(caster, owner, skillId);
                }
            }
        }
    }
}
