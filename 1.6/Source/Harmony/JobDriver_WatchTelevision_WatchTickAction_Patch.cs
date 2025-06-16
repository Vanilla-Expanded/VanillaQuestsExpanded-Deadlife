using HarmonyLib;
using Verse;
using Verse.AI;
using RimWorld;
using System.Linq;

namespace VanillaQuestsExpandedDeadlife
{
    [HarmonyPatch(typeof(JobDriver_WatchTelevision), "WatchTickAction")]
    public static class JobDriver_WatchTelevision_WatchTickAction_Patch
    {
        public static bool Prefix(JobDriver_WatchTelevision __instance)
        {
            var building = (Building)__instance.job.targetA.Thing;
            if (building.def == InternalDefOf.VQED_AncientKinoScreen)
            {
                var reason = building.GetComp<CompKinoScreen>().CanWork();
                if (reason.Accepted is false)
                {
                    RemovePowerComp(__instance);
                    __instance.EndJobWith(JobCondition.Incompletable);
                    return false;
                }
                var comp = new CompPowerTrader
                {
                    parent = building,
                };
                building.AllComps.Add(comp);
                Traverse.Create(comp).Field("powerOnInt").SetValue(true);
                return true;
            }
            return true;
        }
        
        public static void Postfix(JobDriver_WatchTelevision __instance)
        {
            RemovePowerComp(__instance);
        }

        private static void RemovePowerComp(JobDriver_WatchTelevision __instance)
        {
            var building = (Building)__instance.job?.targetA.Thing;
            if (building?.def == InternalDefOf.VQED_AncientKinoScreen)
            {
                var comp = building.AllComps.FirstOrDefault(x => x is CompPowerTrader);
                if (comp != null)
                {
                    building.AllComps.Remove(comp);
                }
            }
        }
    }
}
