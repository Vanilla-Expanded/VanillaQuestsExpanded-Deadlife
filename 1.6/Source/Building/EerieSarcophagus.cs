using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI.Group;
using Verse.Grammar;

namespace VanillaQuestsExpandedDeadlife
{
    public class EerieSarcophagus : Building_AncientCryptosleepCasket
    {
        private int ticksToOpen = -1;
        private Pawn generalPawn;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                var questChainDef = InternalDefOf.VQE_DeadlifeQuestChain;
                generalPawn = questChainDef.Worker.GetUniquePawn("General");
                questChainDef.Worker.State.RemoveFromDeepSave(generalPawn);
                innerContainer.ClearAndDestroyContents();
                innerContainer.TryAdd(generalPawn);
                ticksToOpen = Rand.Range(600, 1500);
            }
        }

        protected override void Tick()
        {
            base.Tick();
            if (ticksToOpen > 0)
            {
                ticksToOpen--;
                if (ticksToOpen == 0)
                {
                    EjectContents();
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksToOpen, "ticksToOpen", -1);
            Scribe_References.Look(ref generalPawn, "generalPawn");
        }

        public override void EjectContents()
        {
            contentsKnown = true;
            ticksToOpen = -1;
            var map = Map;

            Effecter effecter = EffecterDefOf.MonolithStage2.Spawn();
            effecter.Trigger(new TargetInfo(Position, map), new TargetInfo(Position, map));
            effecter.Cleanup();
            base.EjectContents();

            Thing.allowDestroyNonDestroyable = true;
            Destroy();
            Thing.allowDestroyNonDestroyable = false;
            var pod = ThingMaker.MakeThing(InternalDefOf.VQED_EmptyEerieSarcophagus);
            GenSpawn.Spawn(pod, Position, map, Rotation);

            var hediff = generalPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.CryptosleepSickness);
            hediff?.pawn.health.RemoveHediff(hediff);
            generalPawn.health.AddHediff(InternalDefOf.VQED_General);
            InitializeGeneralAI(generalPawn);

            var letterLabel = "VQED_GeneralAwakensLabel".Translate(generalPawn.Named("PAWN"));
            var letterText = "VQED_GeneralAwakensText".Translate(generalPawn.Named("PAWN"));
            Find.LetterStack.ReceiveLetter(letterLabel, letterText, LetterDefOf.ThreatBig, generalPawn);
        }

        private void InitializeGeneralAI(Pawn general)
        {
            var lordJob = new LordJob_General();
            var lord = LordMaker.MakeNewLord(general.Faction, lordJob, general.Map);
            lord.AddPawn(general);
        }
    }
}
