using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;

using Verse;
using Verse.Noise;

namespace VanillaQuestsExpandedDeadlife
{
    public static class Utils
    {
     

        public static void PlaceDistinctBlueprint(Building current, ThingDef replacement)
        {
            if (current.Map.thingGrid.ThingAt(current.Position, replacement.blueprintDef) == null &&
                current.Map.thingGrid.ThingAt(current.Position, replacement.frameDef) == null)
            {
                GenConstruct.PlaceBlueprintForBuild(replacement, current.Position, current.Map, current.Rotation, Faction.OfPlayer, null);
            }
        }
    }
}