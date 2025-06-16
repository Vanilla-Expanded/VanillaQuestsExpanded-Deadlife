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

        public static void ThrowSmoke(Vector3 loc, Map map, float size)
        {
            if (loc.ShouldSpawnMotesAt(map))
            {
                FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, InternalDefOf.VQED_Smoke, Rand.Range(1.5f, 2.5f) * size);
                dataStatic.rotationRate = Rand.Range(-30f, 30f);
                dataStatic.velocityAngle = Rand.Range(30, 40);
                dataStatic.velocitySpeed = Rand.Range(0.5f, 0.7f);
                map.flecks.CreateFleck(dataStatic);
            }
        }

    }
}
