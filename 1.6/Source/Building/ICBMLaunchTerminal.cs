using Verse;
using System.Linq;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
namespace VanillaQuestsExpandedDeadlife
{
    public class ICBMLaunchTerminal : VEF.Buildings.SwappableBuilding
    {
        private bool swapped;
        public override void Notify_Swap()
        {
            swapped = true;
            var loadedSilos = Map.listerThings.ThingsOfDef(InternalDefOf.VQED_LoadedICBMSilo).OfType<ICBMSilo>().ToList();
            foreach (var silo in loadedSilos)
            {
                silo.Notify_Swap();
            }
            var list = new List<Thing>();
            list.Add(ThingMaker.MakeThing(InternalDefOf.VQED_DistantICBMExplosion));
            var pos = new List<IntVec3>();
            pos.Add(Position);
            var request = new SpawnRequest(list, pos, 1, 1);
            request.initialDelay = 40 * 60;
            Map.deferredSpawner.AddRequest(request, false);

            var site = Map.Parent;
            var signal = QuestNode_Root_AncientICBMLaunchSite.DeathlifeApocalypsisStarted;
            Find.SignalManager.SendSignal(new Signal(signal, site.Named("SUBJECT")));
            QuestUtility.SendQuestTargetSignals(site.questTags, signal, site.Named("SUBJECT"));

            base.Notify_Swap();
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (swapped is false)
            {
                var site = Map.Parent;
                var signal = QuestNode_Root_AncientICBMLaunchSite.TerminalDestroyed;
                Find.SignalManager.SendSignal(new Signal(signal));
                QuestUtility.SendQuestTargetSignals(site.questTags, signal, site.Named("SUBJECT"));
            }
            base.Destroy(mode);
        }
    }
}
