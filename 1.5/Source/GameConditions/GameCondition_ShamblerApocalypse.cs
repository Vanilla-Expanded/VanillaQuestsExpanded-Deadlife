using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;

namespace VanillaQuestsExpandedDeadlife
{
    public class GameCondition_ShamblerApocalypse : GameCondition
    {
        private int nextEventTicks;
        private int nextHordeTicks;
        private readonly IntRange eventInterval = new IntRange(GenDate.TicksPerDay * 2, GenDate.TicksPerDay * 5);
        private readonly IntRange hordeInterval = new IntRange(GenDate.TicksPerDay * 3, GenDate.TicksPerDay * 6);

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref nextEventTicks, "nextEventTicks", 0);
            Scribe_Values.Look(ref nextHordeTicks, "nextHordeTicks", 0);
        }

        public override void GameConditionTick()
        {
            base.GameConditionTick();

            foreach (var map in Find.Maps)
            {
                if (!map.gameConditionManager.ConditionIsActive(InternalDefOf.DeathPall))
                {
                    var gameCondition = GameConditionMaker.MakeCondition(InternalDefOf.DeathPall, -1);
                    gameCondition.Permanent = true;
                    map.gameConditionManager.RegisterCondition(gameCondition);
                }
            }

            if (Find.TickManager.TicksGame >= nextEventTicks)
            {
                TriggerRandomEvent();
                nextEventTicks = Find.TickManager.TicksGame + eventInterval.RandomInRange;
            }
            if (Find.TickManager.TicksGame >= nextHordeTicks)
            {
                SpawnNewShamblerHorde();
                nextHordeTicks = Find.TickManager.TicksGame + hordeInterval.RandomInRange;
            }
        }

        public override void End()
        {
            base.End();
            foreach (var map in Find.Maps)
            {
                map.gameConditionManager.GetActiveCondition(InternalDefOf.DeathPall)?.End();
            }
        }

        public override void Init()
        {
            base.Init();
            nextEventTicks = Find.TickManager.TicksGame + eventInterval.RandomInRange;
            nextHordeTicks = Find.TickManager.TicksGame + hordeInterval.RandomInRange;
            for (var i = 0; i < 3; i++)
            {
                SpawnNewShamblerHorde();
            }
        }

        private void TriggerRandomEvent()
        {
            float totalWeight = 4.5f;
            float random = Rand.Value * totalWeight;

            if (random < 2f)
            {
                var swarmDef = InternalDefOf.ShamblerSwarm;
                if (swarmDef != null)
                {
                    IncidentParms parms = StorytellerUtility.DefaultParmsNow(swarmDef.category, Find.Maps.Where(m => m.IsPlayerHome).RandomElement());
                    swarmDef.Worker.TryExecute(parms);
                }
            }
            else if (random < 2.25f)
            {
                var swarmAnimalsDef = InternalDefOf.ShamblerSwarmAnimals;
                if (swarmAnimalsDef != null)
                {
                    IncidentParms parms = StorytellerUtility.DefaultParmsNow(swarmAnimalsDef.category, Find.Maps.Where(m => m.IsPlayerHome).RandomElement());
                    swarmAnimalsDef.Worker.TryExecute(parms);
                }
            }
            else if (random < 2.5f)
            {
                var smallSwarmDef = InternalDefOf.SmallShamblerSwarm;
                IncidentParms parms = StorytellerUtility.DefaultParmsNow(smallSwarmDef.category, Find.Maps.Where(m => m.IsPlayerHome).RandomElement());
                smallSwarmDef.Worker.TryExecute(parms);
            }
            else
            {
                var assaultDef = InternalDefOf.ShamblerAssault;
                IncidentParms parms = StorytellerUtility.DefaultParmsNow(assaultDef.category, Find.Maps.Where(m => m.IsPlayerHome).RandomElement());
                assaultDef.Worker.TryExecute(parms);
            }
        }

        private void SpawnNewShamblerHorde()
        {
            var tile = TileFinder.RandomSettlementTileFor(Faction.OfEntities);
            ShamblerHorde.SpawnShamblerHorder(tile);
        }
    }
}
