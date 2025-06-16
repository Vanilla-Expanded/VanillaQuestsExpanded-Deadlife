using Verse;
using RimWorld;
using Verse.AI.Group;
using RimWorld.QuestGen;

namespace VanillaQuestsExpandedDeadlife
{
    public class Hediff_General : HediffWithComps
    {
        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            SendDefeatedSignal();
        }
        
        public override void Tick()
        {
            base.Tick();
            if (!pawn.Spawned || pawn.CurJobDef == InternalDefOf.VQE_WorkOnTerminal)
            {
                return;
            }
            
            if (pawn.Faction != Faction.OfEntities || pawn.IsPrisonerOfColony)
            {
                SendDefeatedSignal();
                pawn.health.RemoveHediff(this);
                return;
            }

            var currentLordJob = pawn.GetLord()?.LordJob;
            var terminalJob = JobGiver_WorkOnTerminal.GetJob(pawn);
            if (terminalJob != null)
            {
                if (currentLordJob is not LordJob_General)
                {
                    LordJob newLordJob = new LordJob_General();
                    var lord = LordMaker.MakeNewLord(pawn.Faction, newLordJob, pawn.Map);
                    lord.AddPawn(pawn);
                }
            }
            else if (currentLordJob is null || currentLordJob is LordJob_General)
            {
                currentLordJob?.lord?.RemovePawn(pawn);
                LordJob newLordJob = new LordJob_DefendPoint(pawn.Position, wanderRadius: 15f, defendRadius: 30f);
                var lord = LordMaker.MakeNewLord(pawn.Faction, newLordJob, pawn.Map);
                lord.AddPawn(pawn);
                pawn.health.RemoveHediff(this);
            }
        }

        private void SendDefeatedSignal()
        {
            var site = pawn.MapHeld.Parent;
            var signal = QuestNode_Root_AncientICBMLaunchSite.GeneralDefeated;
            Find.SignalManager.SendSignal(new Signal(signal, site.Named("SUBJECT")));
            QuestUtility.SendQuestTargetSignals(site.questTags, signal, site.Named("SUBJECT"));
        }
    }
}
