using RimWorld;
using RimWorld.Planet;
using Verse;
using System.Collections.Generic;
using VEF;
using VEF.Planet;
using System.Linq;
using System;
using UnityEngine;

namespace VanillaQuestsExpandedDeadlife
{
    [HotSwappable]
    public class ShamblerHorde : MovingBase
    {
        private bool shouldBeDestroyed;
        private bool arrived = false;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref arrived, "arrived", false);
            Scribe_Values.Look(ref shouldBeDestroyed, "shouldBeDestroyed", false);
        }

        private Material cachedMat;
        public override Material Material
        {
            get
            {
                if (cachedMat == null)
                {
                    cachedMat = MaterialPool.MatFrom(base.def.texture, ShaderDatabase.WorldOverlayTransparentLit, new ColorInt(50, 50, 50).ToColor, WorldMaterials.WorldObjectRenderQueue);
                }
                return cachedMat;
            }
        }
        public override Texture2D ExpandingIcon => def.ExpandingIconTexture;
        public override void Print(LayerSubMesh subMesh)
        {
            float averageTileSize = Find.WorldGrid.AverageTileSize;
            WorldRendererUtility.PrintQuadTangentialToPlanet(DrawPos, 0.7f * averageTileSize, 0.015f, subMesh, counterClockwise: false);
        }

        protected override void Tick()
        {
            base.Tick();
            if (pather.Moving && pather.Destination != PlanetTile.Invalid)
            {
                var settlementAtDestination = Find.WorldObjects.SettlementAt(pather.Destination);
                if (settlementAtDestination == null)
                {
                    PickNewDestination();
                    return;
                }
            }

            if (!arrived && !pather.Moving && Map is null && pather.Destination == Tile)
            {
                HandleArrival();
                arrived = true;
            }
            else if (arrived && pather.Moving)
            {
                arrived = false;
            }

            if (Map != null)
            {
                if (pather.Moving)
                {
                    pather.StopDead();
                }
                CheckDefeated();
            }
            if (shouldBeDestroyed && Map.mapPawns.AnyPawnBlockingMapRemoval is false)
            {
                Destroy();
            }
        }

        private void CheckDefeated()
        {
            if (shouldBeDestroyed is false && Map.mapPawns.SpawnedShamblers.Any(x => x.Downed is false && x.Dead is false) is false)
            {
                Messages.Message("VQED_ShamblersDefeated".Translate(), this, MessageTypeDefOf.NegativeEvent);
                shouldBeDestroyed = true;
            }
        }

        private void HandleArrival()
        {
            var arrivedTile = Tile;
            var settlement = Find.WorldObjects.SettlementAt(arrivedTile);

            if (settlement != null && !settlement.Faction.IsPlayer)
            {
                if (Rand.ChanceSeeded(0.6f, this.ID))
                {
                    Destroy();
                }
                else
                {
                    settlement.Destroy();
                    var newHorde = SpawnShamblerHorder(arrivedTile);
                    Messages.Message("VQED_FactionBaseOverrun".Translate(settlement.Label), newHorde, MessageTypeDefOf.NegativeEvent);
                    PickNewDestination();
                }
            }
            else if (settlement != null && settlement.Faction.IsPlayer)
            {
                var playerMap = settlement.Map;
                AttackPlayerSettlement(playerMap);
                Destroy();
            }
            else
            {
                PickNewDestination();
            }
        }

        public static ShamblerHorde SpawnShamblerHorder(PlanetTile tile)
        {
            var newHorde = (ShamblerHorde)WorldObjectMaker.MakeWorldObject(InternalDefOf.VQED_ShamblerHorde);
            newHorde.SetFaction(Faction.OfEntities);
            newHorde.Tile = tile;
            Find.WorldObjects.Add(newHorde);
            newHorde.PickNewDestination();
            return newHorde;
        }

        public void PickNewDestination()
        {
            var allSettlementTiles = Find.WorldObjects.Settlements
                .Select(settlement => settlement.Tile)
                .ToList();

            if (GenWorldClosest.TryFindClosestTile(Tile, (PlanetTile tile) => allSettlementTiles.Contains(tile), out var closestTile))
            {
                pather.StartPath(closestTile);
            }
            else
            {
                pather.StopDead();
            }
        }

        protected override void DoMapGeneration(Caravan caravan, bool mapWasGenerated, Map map)
        {
            base.DoMapGeneration(caravan, mapWasGenerated, map);
            CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Edge, CaravanDropInventoryMode.DoNotDrop, draftColonists: true);
            var baseShamblerPoints = 45f;
            var assaultDef = InternalDefOf.ShamblerAssault;
            var parms = StorytellerUtility.DefaultParmsNow(assaultDef.category, map);
            var points = baseShamblerPoints * Rand.Range(20, 36);
            parms.spawnCenter = map.Center;
            parms.faction = Faction.OfEntities;
            parms.target = map;
            parms.pawnGroupKind = PawnGroupKindDefOf.Shamblers;
            parms.points = points;
            var raid = assaultDef.Worker as IncidentWorker_Raid;
            raid.TryGenerateRaidInfo(parms, out var pawns);
            var walkableCells = map.AllCells.Where(x => x.Walkable(map)).ToList();
            foreach (var pawn in pawns.ToList())
            {
                pawn.DeSpawn();
                GenSpawn.Spawn(pawn, walkableCells.RandomElement(), map);
            }
            parms.raidStrategy.Worker.MakeLords(parms, pawns);
        }

        private void AttackPlayerSettlement(Map playerMap)
        {
            var assaultDef = InternalDefOf.ShamblerAssault;
            var parms = StorytellerUtility.DefaultParmsNow(assaultDef.category, playerMap);
            parms.points *= 5f;
            assaultDef.Worker.TryExecute(parms);
        }
    }
}
