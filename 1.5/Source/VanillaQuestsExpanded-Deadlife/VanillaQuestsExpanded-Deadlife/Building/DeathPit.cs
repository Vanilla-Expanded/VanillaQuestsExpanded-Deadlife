using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.Sound;
using static HarmonyLib.Code;
using Verse.Noise;
using Verse.AI;
using Mono.Unix.Native;
using Verse.AI.Group;
using VFECore;

namespace VanillaQuestsExpandedDeadlife
{
    public class DeathPit : Building
    {
        public int tickCounter;
        public int nextCount;
        public int deadDustCounter=0;
        public bool emittingDeaddust = false;
        public IntRange nextShamblerEmergence = new IntRange(3600, 8400);
        public IntRange shamblerAmount = new IntRange(2, 4);

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref tickCounter, "tickCounter", 0);
            Scribe_Values.Look(ref nextCount, "nextCount", 0);
            Scribe_Values.Look(ref emittingDeaddust, "emittingDeaddust", false);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            nextCount = nextShamblerEmergence.RandomInRange;
            base.SpawnSetup(map, respawningAfterLoad);
        }

        public override void Tick()
        {
            base.Tick();

            tickCounter++;
            if (tickCounter > nextCount)
            {
                Notify_StartDeaddust();
               
                tickCounter = 0;
                nextCount = nextShamblerEmergence.RandomInRange;
            }
            if (emittingDeaddust)
            {
                GasUtility.AddGas(Position, Map, GasType.DeadlifeDust, 0.2f);
                deadDustCounter--;
                if (deadDustCounter < 0) { 
                    emittingDeaddust = false;
                    Notify_EjectShamblers();
                }
            }

        }

        public void Notify_StartDeaddust()
        {
            emittingDeaddust = true;
            deadDustCounter = 300;
            
        }

        public void Notify_EjectShamblers()
        {
            

            for (int i = 0; i < shamblerAmount.RandomInRange; i++) {
                int amountShamblersInMap = this.Map.mapPawns.SpawnedShamblers.Count;
                if (amountShamblersInMap <= 50)
                {
                    CellRect cellRect = GenAdj.OccupiedRect(this.Position, Rot4.North, this.def.Size);
                    List<PawnFlyer> list = new List<PawnFlyer>();
                    List<IntVec3> list2 = new List<IntVec3>();

                    PawnKindDef chosenPawn = PawnKindDefOf.ShamblerSwarmer;

                    Pawn p = PawnGenerator.GeneratePawn(chosenPawn, Faction.OfEntities);
                    p.mutant.rotStage = RotStage.Dessicated;
                    IntVec3 randomCell = cellRect.RandomCell;
                    GenSpawn.Spawn(p, randomCell, this.Map);
                    if (CellFinder.TryFindRandomCellNear(this.Position, this.Map, 3, (IntVec3 c) => !c.Fogged(this.Map) && c.Walkable(this.Map) && !c.Impassable(this.Map), out IntVec3 result))
                    {
                        p.rotationTracker.FaceCell(result);
                        list.Add(PawnFlyer.MakeFlyer(InternalDefOf.VQED_PawnFlyer_Stun, p, result, null, null, flyWithCarriedThing: false, randomCell.ToVector3()));
                        list2.Add(randomCell);
                    }

                    if (list2.Count != 0)
                    {
                        SpawnRequest spawnRequest = new SpawnRequest(list.Cast<Thing>().ToList(), list2, 1, 1f);
                        spawnRequest.initialDelay = 400;
                        this.Map.deferredSpawner.AddRequest(spawnRequest);


                    }
                }
                
            }
            
        }

        public override string GetInspectString()
        {
            if (Prefs.DevMode)
            {
                return base.GetInspectString() + "VQE_DebugNextSpawn".Translate((nextCount - tickCounter).ToStringTicksToPeriod());
            }
            else 

                return base.GetInspectString();
        }

    }
}
