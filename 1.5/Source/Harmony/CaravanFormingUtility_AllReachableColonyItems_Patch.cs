using System.Collections.Generic;
using System.Linq;
using Verse;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;

namespace VanillaQuestsExpandedDeadlife
{
    [HarmonyPatch(typeof(CaravanFormingUtility), nameof(CaravanFormingUtility.AllReachableColonyItems))]
    public static class CaravanFormingUtility_AllReachableColonyItems_Patch
    {
        public static void Postfix(Map map, ref List<Thing> __result)
        {
            if (map.Biome == InternalDefOf.VQE_AncientSiloBiome)
            {
                __result.RemoveAll(t => t.Position.GetZone(map) is not Zone_Stockpile);
            }
        }
    }
}
