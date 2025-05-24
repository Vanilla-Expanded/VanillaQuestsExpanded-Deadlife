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
            var hatch = ThingMaker.MakeThing(InternalDefOf.VQED_LockedSiloHatch);
            GenSpawn.Spawn(hatch, center, map, WipeMode.Vanish);
            var spawnRadius = 10f;
            
            var shamblerCount = 5;
            var lordJob = new LordJob_DefendPoint(center, spawnRadius);
            var lord = LordMaker.MakeNewLord(Faction.OfEntities, lordJob, map);

            for (var i = 0; i < shamblerCount; i++)
            {
                var spawnCell = CellFinder.RandomSpawnCellForPawnNear(center, map, (int)spawnRadius);
                if (spawnCell.IsValid)
                {
                    var pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.ShamblerSwarmer, Faction.OfEntities);
                    GenSpawn.Spawn(pawn, spawnCell, map);
                    lord.AddPawn(pawn);
                }
            }
        }
    }
}
