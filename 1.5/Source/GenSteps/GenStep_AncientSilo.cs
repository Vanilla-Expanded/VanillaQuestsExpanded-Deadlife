using RimWorld;
using Verse;
using Verse.AI.Group;

namespace VanillaQuestsExpandedDeadlife
{
    public class GenStep_AncientSilo : GenStep
    {
        public override int SeedPart => 123456789;
        public override void Generate(Map map, GenStepParams parms)
        {
            var center = map.Center;
            var clearRadius = 10;

            foreach (var cell in CellRect.CenteredOn(center, clearRadius))
            {
                if (cell.InBounds(map))
                {
                    var thingList = cell.GetThingList(map);
                    for (var i = thingList.Count - 1; i >= 0; i--)
                    {
                        var thing = thingList[i];
                        if (thing is Building)
                        {
                            thing.Destroy();
                        }
                    }
                }
            }

            var hatch = ThingMaker.MakeThing(InternalDefOf.VQED_LockedSiloHatch);
            GenSpawn.Spawn(hatch, center, map, WipeMode.Vanish);

            var spawnRadius = 10;
            var shamblerCount = new IntRange(6, 12).RandomInRange;

            var lordJob = new LordJob_DefendPoint(center, spawnRadius);
            var lord = LordMaker.MakeNewLord(Faction.OfEntities, lordJob, map);

            for (var i = 0; i < shamblerCount; i++)
            {
                var randomRoot = hatch.OccupiedRect().ExpandedBy(1).EdgeCells.RandomElement();
                if (CellFinder.TryFindRandomSpawnCellForPawnNear(randomRoot, map, out var spawnCell, spawnRadius, (IntVec3 c) => c.GetFirstBuilding(map) == null))
                {
                    var pawn = PawnGenerator.GeneratePawn(InternalDefOf.VQE_MilitaryShambler, Faction.OfEntities);
                    GenSpawn.Spawn(pawn, spawnCell, map);
                    lord.AddPawn(pawn);
                }
            }
        }
    }
}
