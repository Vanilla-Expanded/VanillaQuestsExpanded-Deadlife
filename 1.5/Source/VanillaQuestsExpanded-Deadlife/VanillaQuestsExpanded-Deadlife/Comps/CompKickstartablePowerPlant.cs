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




namespace VanillaQuestsExpandedDeadlife
{
    public class CompKickstartablePowerPlant : CompPowerPlant
    {

        public bool active = false;
        public int countDown;
       

        public new CompProperties_KickstartablePower Props => (CompProperties_KickstartablePower)props;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref active, "active", false);
            Scribe_Values.Look(ref countDown, "countDown", 0);

        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            countDown = Props.kickStartableTimer;
        }

        protected override float DesiredPowerOutput
        {
            get
            {
                if (!active)
                {
                    return 0;
                }
                return base.DesiredPowerOutput;
            }
            
        }

        public override void CompTick()
        {
            base.CompTick();
            if (active)
            {
                countDown--;
                if (countDown <= 0)
                {
                    active = false;
                    countDown = Props.kickStartableTimer;


                }
            }
        }
        public override string CompInspectStringExtra()
        {
            if (active)
            {
                return base.CompInspectStringExtra()+"\n"+"VQED_KickStartTimer".Translate(countDown.ToStringTicksToPeriod());
            }
            return base.CompInspectStringExtra();
        }

    }
}
