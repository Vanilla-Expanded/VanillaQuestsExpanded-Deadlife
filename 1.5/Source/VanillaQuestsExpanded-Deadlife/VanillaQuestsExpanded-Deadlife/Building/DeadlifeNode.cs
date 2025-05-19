using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Noise;
using Verse.Sound;


namespace VanillaQuestsExpandedDeadlife
{
    public class DeadlifeNode : Building_Trap
    {

        protected override void SpringSub(Pawn p)
        {
            GetComp<CompExplosive>().StartWick(p);
        }

        public override void Tick()
        {
            base.Tick();
           

            if (this.IsHashIntervalTick(60))
            {
                int numCells = GenRadial.NumCellsInRadius(6);
                for (int i = 0; i < numCells; i++)
                {
                    IntVec3 intVec = this.Position + GenRadial.RadialPattern[i];
                    if (intVec.InBounds(this.Map))
                    {
                        foreach (Thing thing in intVec.GetThingList(this.Map))
                        {
                            if (thing != null && thing is Pawn detectedPawn && detectedPawn.RaceProps.Humanlike && !detectedPawn.IsShambler)
                            {
                                this.SpringSub(detectedPawn);
                            }
                        }

                    }
                }
            }


        }

        public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.PostApplyDamage(dinfo, totalDamageDealt);

            this.SpringSub(null);
        }


      


    }
}
