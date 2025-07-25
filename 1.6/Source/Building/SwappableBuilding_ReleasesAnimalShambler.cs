﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Analytics;
using VEF.Buildings;
using Verse;
using Verse.AI.Group;
using Verse.Noise;

namespace VanillaQuestsExpandedDeadlife
{
    public class SwappableBuilding_ReleasesAnimalShambler : SwappableBuilding
    {
        IntRange shamblerNumber = new IntRange(1,1);

        public override void Notify_Swap()
        {


            var lordJob = new LordJob_DefendPoint(this.Position, 10);
            var lord = LordMaker.MakeNewLord(Faction.OfEntities, lordJob, this.Map);

            for (int i = 0; i <= shamblerNumber.RandomInRange; i++) {

                if (CellFinder.TryFindRandomSpawnCellForPawnNear(this.Position, this.Map, out var spawnCell, 2, (IntVec3 c) => c.GetFirstBuilding(this.Map) == null))
                {
                    Pawn pawn = GenStep_AncientSilo.GenerateAnimalShambler();
                    GenSpawn.Spawn(pawn, spawnCell, this.Map);
                    lord.AddPawn(pawn);
                }
            }

            

            base.Notify_Swap();

        }

    }
}
