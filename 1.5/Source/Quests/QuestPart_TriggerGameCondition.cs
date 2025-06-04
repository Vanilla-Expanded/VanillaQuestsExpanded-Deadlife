using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace VanillaQuestsExpandedDeadlife
{
    public class QuestPart_TriggerGameCondition : QuestPart
    {
        public string inSignal;
        public GameConditionDef gameConditionDef;
        public int durationTicks;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref inSignal, "inSignal");
            Scribe_Defs.Look(ref gameConditionDef, "gameConditionDef");
            Scribe_Values.Look(ref durationTicks, "durationTicks");
        }

        public override void Notify_QuestSignalReceived(Signal signal)
        {
            base.Notify_QuestSignalReceived(signal);
            if (signal.tag == inSignal)
            {
                if (gameConditionDef == null)
                {
                    gameConditionDef = GameConditionDefOf.PsychicDrone;
                }
                if (durationTicks <= 0)
                {
                    durationTicks = GenDate.TicksPerDay;
                }
                var cond = GameConditionMaker.MakeCondition(gameConditionDef, durationTicks);
                Find.World.GameConditionManager.RegisterCondition(cond);
            }
        }
    }
}
