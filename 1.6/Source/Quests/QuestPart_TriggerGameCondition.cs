using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace VanillaQuestsExpandedDeadlife
{
    public class QuestPart_TriggerGameCondition : QuestPart
    {
        public string inSignal;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref inSignal, "inSignal");
        }

        public override void Notify_QuestSignalReceived(Signal signal)
        {
            base.Notify_QuestSignalReceived(signal);
            if (signal.tag == inSignal || signal.tag == inSignal.Replace("MapRemoved", "Destroyed"))
            {
                DistantICBMExplosion.DoExplosion(null);
            }
        }
    }
}
