using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;
namespace VanillaQuestsExpandedDeadlife
{
    [HarmonyPatch(typeof(CompExplosive), "Detonate")]
    public static class CompExplosive_Detonate_Patch
    {
        public static void Prefix(CompExplosive __instance, Map map)
        {
            if (__instance.parent is DeadlifeNode deadlifeNode)
            {
                var room = deadlifeNode.Position.GetRoom(map);
                var gasAmount = Mathf.Min(Mathf.CeilToInt(room.CellCount * 0.15f * 255f), 12750);
                GasUtility.AddDeadifeGas(deadlifeNode.Position, map, Faction.OfEntities, gasAmount);
            }
        }
    }
}
