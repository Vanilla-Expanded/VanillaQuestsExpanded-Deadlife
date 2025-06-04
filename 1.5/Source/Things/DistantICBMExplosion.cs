using Verse;
using Verse.Sound;
using RimWorld;
using System.Linq;

namespace VanillaQuestsExpandedDeadlife
{
    public class DistantICBMExplosion : Thing
    {
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            var cells = map.AllCells.InRandomOrder().Take(4).ToList();
            foreach (var cell in cells)
            {
                FleckMaker.Static(cell, map, InternalDefOf.VFED_ICBMExplosionFlash, 1000);
            }
            InternalDefOf.VQED_ICBMExplosionDistant.PlayOneShotOnCamera();
            var cond = GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicDrone, (int)(Rand.Range(1f, 3f) * GenDate.TicksPerDay));
            Find.World.GameConditionManager.RegisterCondition(cond);
            Find.CameraDriver.shaker.DoShake(4f, 600);
            this.Destroy();
        }
    }
}
