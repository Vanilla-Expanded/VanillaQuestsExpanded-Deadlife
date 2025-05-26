using Verse;

namespace VanillaQuestsExpandedDeadlife;

public class Building_LampForceRefresh : Building
{
    public bool isFogged = true;
    public override void Tick()
    {
        base.Tick();
        if (isFogged &&Spawned && Position.Fogged(Map) is false)
        {
            isFogged = false;
            Map.glowGrid.DirtyCache(Position);
        }
    }
}
