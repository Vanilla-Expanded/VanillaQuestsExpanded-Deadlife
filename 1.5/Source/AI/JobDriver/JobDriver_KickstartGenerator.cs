using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using VFECore;

namespace VanillaQuestsExpandedDeadlife
{
    public class JobDriver_KickstartGenerator : JobDriver
    {
        public const int totalTime = 800;


        public int totalTimer = 0;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A).Thing, this.job, 1, -1, null, true);
        }
        private Kickstartable Building => (Kickstartable)job.GetTarget(TargetIndex.A).Thing;

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Thing building = this.job.GetTarget(TargetIndex.A).Thing;
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);

           

            if (TargetA.Thing.def.hasInteractionCell)
            {
                yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
            }
            else
            {
                yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            }

            Toil study = ToilMaker.MakeToil("MakeNewToils");

            study.tickAction = delegate
            {
                Pawn actor = study.actor;
               

                actor.rotationTracker.FaceTarget(actor.CurJob.GetTarget(TargetIndex.A));

                totalTimer++;
                if (totalTimer > totalTime)
                {

                    actor.jobs.EndCurrentJob(JobCondition.Succeeded);


                }
            };

            study.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            study.WithProgressBar(TargetIndex.A, () => (float)totalTimer / totalTime);
            study.defaultCompleteMode = ToilCompleteMode.Never;
         
            study.handlingFacing = true;
            study.AddFinishAction(delegate
            {
                Messages.Message("VQED_KickstartedSuccess".Translate(GetActor().NameFullColored), MessageTypeDefOf.PositiveEvent);
                Building.Notify_Kickstarted();
            });
            yield return study;



        }
    }
}
