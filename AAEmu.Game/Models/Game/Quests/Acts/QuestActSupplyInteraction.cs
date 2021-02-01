using AAEmu.Game.Models.Game.Quests.Templates;
using AAEmu.Game.Models.Game.Char;

namespace AAEmu.Game.Models.Game.Quests.Acts
{
    public class QuestActSupplyInteraction : QuestActTemplate
    {
        public uint WorldInteractionId { get; set; }

        public override bool Use(Character character, Quest quest, int objective)
        {
            _log.Warn("QuestActSupplyInteraction: WorldInteractionId {0}", WorldInteractionId);
            return false;
        }
    }
}
