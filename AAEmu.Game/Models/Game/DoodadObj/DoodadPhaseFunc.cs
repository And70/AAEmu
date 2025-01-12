﻿using AAEmu.Game.Core.Managers.UnitManagers;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.DoodadObj;

public class DoodadPhaseFunc
{
    public uint GroupId { get; set; }
    public uint FuncId { get; set; }
    public string FuncType { get; set; }

    /// <summary>
    /// Helper property for DoodadFuncPulseTrigger
    /// </summary>
    public bool PulseTriggered { get; set; }

    // This acts as an interface/relay for doodad function chain
    public bool Use(BaseUnit caster, Doodad owner)
    {
        var template = DoodadManager.Instance.GetPhaseFuncTemplate(FuncId, FuncType);
        return template != null && template.Use(caster, owner);
    }
}
