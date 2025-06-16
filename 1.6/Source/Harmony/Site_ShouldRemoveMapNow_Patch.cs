using HarmonyLib;
using Verse;
using RimWorld;
using RimWorld.Planet;
using System.Linq;
using System.Collections.Generic;

namespace VanillaQuestsExpandedDeadlife
{
    [HarmonyPatch(typeof(Site), "ShouldRemoveMapNow")]
    public static class Site_ShouldRemoveMapNow_Patch
    {
        public static void Postfix(Site __instance, ref bool __result, ref bool alsoRemoveWorldObject)
        {
            if (__result && __instance.Map is Map map && Find.World.pocketMaps.Any(x => x.sourceMap == map && x.Map is Map pocketMap && pocketMap.mapPawns.AnyPawnBlockingMapRemoval))
            {
                __result = false;
                alsoRemoveWorldObject = false;
            }
        }
    }
}
