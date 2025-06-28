using Verse;
using RimWorld;
using RimWorld.Planet;
using KCSG;
using Verse.AI.Group;
using RimWorld.BaseGen;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace VanillaQuestsExpandedDeadlife
{
    public static class StructureSetGenerator
    {
        public static List<CellRect> Generate(Map map, StructureSetDef structureSetDef)
        {
            var generatedRects = new List<CellRect>();
            var mapCenter = map.Center;
            foreach (var layout in structureSetDef.structureLayouts)
            {
                var matchingDefs = DefDatabase<KCSG.StructureLayoutDef>.AllDefsListForReading
                    .Where(def => Regex.IsMatch(def.defName, "^" + layout.pattern + "$"))
                    .ToList();

                if (matchingDefs.Any())
                {
                    var selectedDef = matchingDefs.RandomElement();
                    var spawnPos = mapCenter + layout.offset * selectedDef.Sizes.x;

                    var structureRect = CellRect.CenteredOn(spawnPos, selectedDef.Sizes);
                    GenOption.GetAllMineableIn(structureRect, map);
                    selectedDef.Generate(structureRect, map, map.ParentFaction);
                    generatedRects.Add(structureRect);
                    var walkableCells = structureRect.Cells.Where(cell => cell.Walkable(map) && (layout.forceSpawnEnemiesIndoor is false || cell.Roofed(map) && cell.UsesOutdoorTemperature(map) is false)).ToList();
                    var pawns = new List<Pawn>();
                    if (layout.spawnEnemies != null)
                    {
                        foreach (var spawnOption in layout.spawnEnemies)
                        {
                            for (var i = 0; i < spawnOption.count.RandomInRange; i++)
                            {
                                var rootCell = walkableCells.RandomElement();
                                if (!rootCell.IsValid)
                                {
                                    rootCell = structureRect.CenterCell;
                                }
                                var spawnCell = CellFinder.RandomSpawnCellForPawnNear(rootCell, map, 5);
                                if (spawnCell.IsValid)
                                {
                                    var pawn = GenStep_AncientSilo.GenerateShambler(spawnOption.kind);
                                    GenSpawn.Spawn(pawn, spawnCell, map);
                                    pawns.Add(pawn);
                                }
                            }
                        }
                    }

                    if (pawns.Any())
                    {
                        var lordJob = new LordJob_DefendBaseNoEat(Faction.OfEntities, walkableCells.RandomElement(), 180000);
                        var lord = LordMaker.MakeNewLord(Faction.OfEntities, lordJob, map, pawns);
                    }
                }
                else
                {
                    Log.Warning($"No StructureLayoutDefs found matching pattern: {layout.pattern}");
                }
            }
            return generatedRects;
        }
    }
}
