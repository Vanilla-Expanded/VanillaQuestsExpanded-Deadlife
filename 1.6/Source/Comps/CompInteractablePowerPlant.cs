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
    public class CompInteractablePowerPlant : CompInteractable
    {
        CompKickstartablePowerPlant cachedComp;

        public CompKickstartablePowerPlant compKickstartablePowerPlant
        {

            get
            {
                if (cachedComp is null)
                {
                    cachedComp = parent.TryGetComp<CompKickstartablePowerPlant>();
                }
                return cachedComp;
            }

        }


        public override void OrderForceTarget(LocalTargetInfo target)
        {
            OrderActivation(target.Pawn);
        }

        private void OrderActivation(Pawn pawn)
        {
            Job job = JobMaker.MakeJob(JobDefOf.InteractThing, parent);
            job.count = 1;
            job.playerForced = true;
            pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {

            AcceptanceReport acceptanceReport = CanInteract(selPawn);
            FloatMenuOption floatMenuOption = new FloatMenuOption(Props.jobString.CapitalizeFirst(), delegate
            {
                OrderActivation(selPawn);
            });
            if (!acceptanceReport.Accepted)
            {
                floatMenuOption.Disabled = true;
                floatMenuOption.Label = floatMenuOption.Label + " (" + acceptanceReport.Reason + ")";
            }
            yield return floatMenuOption;

        }

        protected override void OnInteracted(Pawn caster)
        {

          

            Messages.Message("VQED_KickstartedSuccess".Translate(caster.NameFullColored), MessageTypeDefOf.PositiveEvent);
            compKickstartablePowerPlant.active = true;
           

        }
    }
}
