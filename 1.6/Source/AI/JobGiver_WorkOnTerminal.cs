using System.Linq;
using Verse;
using Verse.AI;
using RimWorld;
using Verse.AI.Group;

namespace VanillaQuestsExpandedDeadlife
{
    public class JobGiver_WorkOnTerminal : ThinkNode_JobGiver
    {
        protected override Job TryGiveJob(Pawn pawn)
        {
            return GetJob(pawn);
        }

        public static Job GetJob(Pawn pawn)
        {
            Building activeTerminal = FindNearestActiveTerminal(pawn);
            if (activeTerminal != null)
            {
                return CreateTerminalJob(activeTerminal);
            }
            Building icbmTerminal = FindICBMLaunchTerminal(pawn);
            if (icbmTerminal != null)
            {
                return CreateTerminalJob(icbmTerminal);
            }
            return null;
        }

        private static Building FindNearestActiveTerminal(Pawn pawn)
        {
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(InternalDefOf.VQED_ActiveTerminal), PathEndMode.InteractionCell, TraverseParms.For(pawn), 9999f, b => pawn.CanReach(b, PathEndMode.InteractionCell, Danger.Deadly)) as Building;
        }

        private static Building FindICBMLaunchTerminal(Pawn pawn)
        {
            return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(InternalDefOf.VQED_ICBMLaunchTerminal), PathEndMode.InteractionCell, TraverseParms.For(pawn), 9999f, b => pawn.CanReach(b, PathEndMode.InteractionCell, Danger.Deadly)) as Building;
        }

        private static Job CreateTerminalJob(Building terminal)
        {
            Job job = JobMaker.MakeJob(InternalDefOf.VQE_WorkOnTerminal, terminal);
            job.ignoreDesignations = true;
            job.ignoreForbidden = true;
            job.ignoreJoyTimeAssignment = true;
            job.playerForced = true;
            return job;
        }
    }
}
