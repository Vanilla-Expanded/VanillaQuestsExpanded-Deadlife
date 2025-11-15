using HarmonyLib;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedDeadlife
{
    [HarmonyPatch(typeof(Building_GravEngine), "CanLaunch")]
    public static class Building_GravEngine_CanLaunch_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref AcceptanceReport __result, Building_GravEngine __instance)
        {
            Map map = __instance.Map;
            if (map != null && __result.Accepted is false && __result.Reason == "AbnormalGameCondition".Translate() && map.GameConditionManager.ConditionIsActive(GameConditionDefOf.DeathPall))
            {
                __result = AcceptanceReport.WasAccepted;
            }
        }
    }
}
