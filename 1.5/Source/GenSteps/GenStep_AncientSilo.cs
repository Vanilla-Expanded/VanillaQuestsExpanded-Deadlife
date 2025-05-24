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

            // Clear area around the center
            foreach (var cell in CellRect.CenteredOn(center, clearRadius))
            {
                if (cell.InBounds(map))
                {
                    var thingList = cell.GetThingList(map);
                    for (var i = thingList.Count - 1; i >= 0; i--)
                    {
                        var thing = thingList[i];
                        if (thing.def.mineable || thing.def.IsBuildingArtificial)
                        {
                            thing.Destroy();
                        }
                    }
                }
            }

            // Place the silo hatch
            var hatchDef = InternalDefOf.VQED_LockedSiloHatch;
            if (hatchDef != null)
            {
                var hatch = ThingMaker.MakeThing(hatchDef);
                GenSpawn.Spawn(hatch, center, map, WipeMode.Vanish);
            }

            // Spawn shamblers around the hatch
            var shamblerDef = PawnKindDefOf.ShamblerSwarmer;
            if (shamblerDef != null)
            {
                var spawnRadius = 10f;
                var shamblerCount = 5;

                var lordJob = new LordJob_DefendPoint(center, spawnRadius);
                var lord = LordMaker.MakeNewLord(Faction.OfEntities, lordJob, map);

                for (var i = 0; i < shamblerCount; i++)
                {
                    var spawnCell = CellFinder.RandomSpawnCellForPawnNear(center, map, (int)spawnRadius);
                    if (spawnCell.IsValid)
                    {
                        var pawn = PawnGenerator.GeneratePawn(shamblerDef, Faction.OfEntities);
                        GenSpawn.Spawn(pawn, spawnCell, map);
                        lord.AddPawn(pawn);
                    }
                }
            }
        }
    }
}
