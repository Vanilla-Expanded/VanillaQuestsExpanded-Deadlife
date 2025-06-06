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
                FleckMaker.Static(cell, map, InternalDefOf.VQED_ICBMExplosionFlash, 1000);
            }
            InternalDefOf.VQED_ICBMExplosionDistant.PlayOneShotOnCamera();
            var def = InternalDefOf.VQED_ShamblerApocalypse;
            var cond = GameConditionMaker.MakeCondition(def, (int)(Rand.Range(45f, 180f) * GenDate.TicksPerDay));
            Find.World.GameConditionManager.RegisterCondition(cond);
            IncidentParms parms = new IncidentParms();
            parms.target = base.Map;
            IncidentWorker.SendIncidentLetter(def.LabelCap, def.letterText, def.letterDef, parms, LookTargets.Invalid, null);
            Find.CameraDriver.shaker.DoShake(4f, 600);
            this.Destroy();
        }
    }
}
