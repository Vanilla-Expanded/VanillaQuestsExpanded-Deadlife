using Verse;
using UnityEngine;
using RimWorld;

namespace VanillaQuestsExpandedDeadlife
{
    public class PlaceWorker_AncientKinoScreen : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            var expectedProjectorPos = center + rot.FacingCell * 6;
            var ghostDef = InternalDefOf.VQED_AncientKinoProjector;
            var map = thing?.Map ?? Find.CurrentMap;
            var existingProjector = JoyGiver_WatchBuilding_CanInteractWith_Patch.GetProjector(center, rot, map);
            if (existingProjector != null)
            {
                return;
            }
            GhostDrawer.DrawGhostThing(expectedProjectorPos, rot.Opposite, ghostDef, null, Color.white, AltitudeLayer.Blueprint, null, drawPlaceWorkers: false);
        }
    }
}
