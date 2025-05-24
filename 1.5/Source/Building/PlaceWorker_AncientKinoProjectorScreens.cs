using Verse;
using UnityEngine;
using System.Collections.Generic;

namespace VanillaQuestsExpandedDeadlife
{
    public class PlaceWorker_AncientKinoProjectorScreens : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
        {
            var map = thing?.Map ?? Find.CurrentMap;
            if (map == null)
            {
                return;
            }

            var screens = map.listerBuildings.AllBuildingsColonistOfDef(InternalDefOf.VQED_AncientKinoScreen);
            foreach (var screen in screens)
            {
                var placeWorkerScreen = screen.def.PlaceWorkers.Find(pw => pw.GetType() == typeof(PlaceWorker_AncientKinoScreen)) as PlaceWorker_AncientKinoScreen;
                if (placeWorkerScreen != null)
                {
                    placeWorkerScreen.DrawGhost(screen.def, screen.Position, screen.Rotation, Color.white, screen);
                }
            }
        }
    }
}
