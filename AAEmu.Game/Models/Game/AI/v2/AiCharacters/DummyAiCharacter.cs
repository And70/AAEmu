using AAEmu.Game.Models.Game.AI.v2.Behaviors.Common;
using AAEmu.Game.Models.Game.AI.v2.Framework;

namespace AAEmu.Game.Models.Game.AI.v2.AiCharacters;

public class DummyAiCharacter : NpcAi
{
    protected override void Build()
    {
        AddBehavior(BehaviorKind.Spawning, new SpawningBehavior());
        AddBehavior(BehaviorKind.Dummy, new DummyBehavior()).SetDefaultBehavior();
        AddBehavior(BehaviorKind.Dead, new DeadBehavior());
        AddBehavior(BehaviorKind.Despawning, new DespawningBehavior());
    }

    public override void GoToIdle()
    {
        SetCurrentBehavior(BehaviorKind.Dummy);
    }

    public override void GoToRunCommandSet()
    {
        SetCurrentBehavior(BehaviorKind.Dummy);
    }
}
