using RimWorld;
using Verse;
using System.Linq;

namespace VanillaQuestsExpandedDeadlife
{
    public class GenStep_AncientSiloUnderground : GenStep
    {
        public ThingDef exitDef;
        public override int SeedPart => 987654321;
        public AncientSiloStructureSetDef structureSetDef;
        public override void Generate(Map map, GenStepParams parms)
        {
            var structureRects = AncientSiloStructureGenerator.Generate(map, structureSetDef);
            var cell = map.Center;
            foreach (var item in GenRadial.RadialCellsAround(cell, 4.5f, useCenter: true))
            {
                foreach (var thing in item.GetThingList(map).ToList().Where(t => t.def.destroyable))
                {
                    thing.Destroy();
                }
            }

            foreach (var current in map.AllCells)
            {
                var isInsideStructure = false;
                foreach (var rect in structureRects)
                {
                    if (rect.Contains(current))
                    {
                        isInsideStructure = true;
                        break;
                    }
                }
                if (!isInsideStructure)
                {
                    var rockDef = GenStep_RocksFromGrid.RockDefAt(current);
                    if (rockDef != null)
                    {
                        var thing = ThingMaker.MakeThing(rockDef);
                        GenSpawn.Spawn(thing, current, map);
                    }
                }
            }

            GenSpawn.Spawn(ThingMaker.MakeThing(exitDef), cell, map);
            MapGenerator.PlayerStartSpot = cell;
        }
    }
}
