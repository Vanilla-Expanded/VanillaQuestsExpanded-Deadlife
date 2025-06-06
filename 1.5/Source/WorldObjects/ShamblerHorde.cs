using RimWorld;
using RimWorld.Planet;
using Verse;
using System.Collections.Generic;
using VFECore;
using System.Linq;
using System;

namespace VanillaQuestsExpandedDeadlife
{
    public class ShamblerHorde : MovingBase
    {
        private bool arrived = false;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref arrived, "arrived", false);
        }

        public override void Tick()
        {
            base.Tick();
            if (pather.Moving && pather.Destination != -1)
            {
                var settlementAtDestination = Find.WorldObjects.SettlementAt(pather.Destination);
                if (settlementAtDestination == null)
                {
                    PickNewDestination();
                    return;
                }
            }

            if (!pather.Moving && !arrived)
            {
                HandleArrival();
                arrived = true;
            }
            else if (pather.Moving)
            {
                arrived = false;
            }
        }

        private void HandleArrival()
        {
            var arrivedTile = Tile;
            var settlement = Find.WorldObjects.SettlementAt(arrivedTile);

            if (settlement != null && !settlement.Faction.IsPlayer)
            {
                if (Rand.Value < 0.6f)
                {
                    Destroy();
                }
                else
                {
                    Messages.Message("VQED_FactionBaseOverrun".Translate(settlement.Label), MessageTypeDefOf.NegativeEvent, historical: false);
                    settlement.Destroy();
                    SpawnShamblerHorder(arrivedTile);
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

        public static void SpawnShamblerHorder(int tile)
        {
            var newHorde = (ShamblerHorde)WorldObjectMaker.MakeWorldObject(InternalDefOf.VQED_ShamblerHorde);
            newHorde.SetFaction(Faction.OfEntities);
            newHorde.Tile = tile;
            Find.WorldObjects.Add(newHorde);
            newHorde.PickNewDestination();
        }

        public void PickNewDestination()
        {
            var allSettlementTiles = Find.WorldObjects.Settlements
                .Select(settlement => settlement.Tile)
                .ToList();

            if (GenWorldClosest.TryFindClosestTile(Tile, (int tile) => allSettlementTiles.Contains(tile), out var closestTile))
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
            var baseShamblerPoints = 50f;
            var assaultDef = InternalDefOf.ShamblerAssault;

            var parms = StorytellerUtility.DefaultParmsNow(assaultDef.category, map);
            parms.points = baseShamblerPoints * Rand.Range(20, 36);
            assaultDef.Worker.TryExecute(parms);
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
