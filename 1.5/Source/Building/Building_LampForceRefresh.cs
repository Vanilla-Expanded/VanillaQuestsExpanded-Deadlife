using Verse;

namespace VanillaQuestsExpandedDeadlife;

public class Building_LampForceRefresh : Building
{
    public override void Tick()
    {
        base.Tick();
        Map.glowGrid.DirtyCache(Position);
        Map.glowGrid.Rebuild();
    }
}
