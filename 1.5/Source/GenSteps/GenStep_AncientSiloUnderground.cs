using RimWorld;
using Verse;
using System.Linq;

namespace VanillaQuestsExpandedDeadlife
{
    public class GenStep_AncientSiloUnderground : GenStep
    {
        public ThingDef exitDef;
        public override int SeedPart => 987654321;
        public override void Generate(Map map, GenStepParams parms)
        {
            var cell = map.Center;
            foreach (var item in GenRadial.RadialCellsAround(cell, 4.5f, useCenter: true))
            {
                foreach (var thing in item.GetThingList(map).ToList().Where(t => t.def.destroyable))
                {
                    thing.Destroy();
                }
            }
            GenSpawn.Spawn(ThingMaker.MakeThing(exitDef), cell, map);
            MapGenerator.PlayerStartSpot = cell;
        }
    }
}
