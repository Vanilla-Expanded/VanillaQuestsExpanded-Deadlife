using Verse;
using Verse.Sound;
using RimWorld;

namespace VanillaQuestsExpandedDeadlife
{
    public class DistantICBMExplosion : Thing
    {
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            InternalDefOf.VQED_ICBMExplosionDistant.PlayOneShotOnCamera();
            var cond = GameConditionMaker.MakeCondition(GameConditionDefOf.Eclipse, (int)(Rand.Range(1f, 3f) * GenDate.TicksPerDay));
            Find.World.GameConditionManager.RegisterCondition(cond);
            Find.CameraDriver.shaker.DoShake(4f, 600);
            this.Destroy();
        }
    }
}
