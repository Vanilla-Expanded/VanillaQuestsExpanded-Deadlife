using HarmonyLib;
using Verse;
using Verse.AI;
using RimWorld;

namespace VanillaQuestsExpandedDeadlife
{
    
    [HarmonyPatch(typeof(JoyGiver_WatchBuilding), "CanInteractWith")]
    public static class JoyGiver_WatchBuilding_CanInteractWith_Patch
    {
        public static void Postfix(Pawn pawn, Thing t, bool inBed, ref bool __result)
        {
            if (!__result)
            {
                return;
            }

            if (t.def == InternalDefOf.VQED_AncientKinoScreen)
            {
                var reason = t.TryGetComp<CompKinoScreen>().CanWork();
                if (reason.Accepted is false)
                {
                    __result = false;
                    return;
                }
            }
        }

        public static Thing GetProjector(this Thing t)
        {
            return GetProjector(t.Position, t.Rotation, t.Map);
        }
        
        public static Thing GetProjector(IntVec3 pos, Rot4 rot, Map map)
        {
            var projectorPos = pos + rot.FacingCell * 6;
            var projector = projectorPos.GetThingList(map).FirstOrDefault(x => x.def == InternalDefOf.VQED_AncientKinoProjector);
            return projector;
        }
    }
}
