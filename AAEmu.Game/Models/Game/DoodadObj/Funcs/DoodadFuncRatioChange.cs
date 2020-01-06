﻿using AAEmu.Commons.Utils;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.DoodadObj.Templates;
using AAEmu.Game.Models.Game.Items;
using AAEmu.Game.Models.Game.Items.Actions;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.DoodadObj.Funcs
{
    public class DoodadFuncRatioChange : DoodadFuncTemplate
    {
        public int Ratio { get; set; }
        public uint NextPhase { get; set; }

        public override void Use(Unit caster, Doodad owner, uint skillId)
        {
            _log.Debug("DoodadFuncRatioChange : Ratio {0}, NextPhase {1}, SkillId {2}", Ratio, NextPhase, skillId);

            var character = (Character)caster;
            if (character == null) return;

            if (Ratio < 10000)
            {
                var chance = Rand.Next(0, 10000);
                if (chance < Ratio) return;
            }

            //uint itemId;
            const int count = 1;
            var itemTemplate = ItemManager.Instance.GetDoodadToItemList(owner.TemplateId);
            if (itemTemplate == null)
            {
                var itemId = GetItemIdFromSkill(skillId);
                if (itemId == 0)
                    return;
                character.Inventory.AddNewItem(itemId, count, 0, ItemTaskType.Loot);
            }
            else
            {
                //itemId = itemTemplate[0]; // there can be more than one id
                foreach (var itemId in itemTemplate)
                {
                    character.Inventory.AddNewItem(itemId, count, 0, ItemTaskType.Loot);
                }
            }

            //var item = ItemManager.Instance.Create(itemId, count, 0);
            //character.Inventory.AddExistingItem(item, Items.Actions.ItemTaskType.Loot);
            //character.Inventory.AddNewItem(itemId, count, 0, ItemTaskType.Loot);
        }

        private static uint GetItemIdFromSkill(uint skillId)
        {
            uint itemId;
            switch (skillId)
            {
                case 13783:
                    itemId = 14065u; // Meteorite Ore // TODO something is wrong, where does he come from?
                    break;
                case 13985:
                    itemId = 3411u; // Copper Ore
                    break;
                case 13986:
                    itemId = 8022u; // Iron Ore
                    break;
                case 13987:
                    itemId = 8023; // Silver Ore
                    break;
                case 13989:
                    itemId = 8027u; // Gold Ore
                    break;
                case 13789:
                    itemId = 26463u; // 13789   Uproot Spend 10 Labor to uproot a target
                    break;
                default:
                    itemId = 0;
                    break;
            }
            return itemId;
        }
    }
}
