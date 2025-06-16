using RimWorld;
using RimWorld.QuestGen;
using Verse;
using System.Linq;
using VEF.Storyteller;

namespace VanillaQuestsExpandedDeadlife
{
    public class QuestNode_Root_AncientICBMLaunchSite : QuestNode_Site
    {
        public const string GeneralDefeated = "GeneralDefeated";
        public const string DeathlifeApocalypsisStarted = "DeathlifeApocalypsisStarted";
        public const string TerminalDestroyed = "TerminalDestroyed";

        public override SitePartDef QuestSite => QuestDefOf.VQE_AncientICBMLaunchSite;
        public override Predicate<Map, int> TileValidator => (Map map, int tile) => Find.WorldGrid.ApproxDistanceInTiles(tile, map.Tile) <= 50 && Find.WorldGrid[tile].hilliness < RimWorld.Planet.Hilliness.LargeHills;

        protected override void RunInt()
        {
            var slate = QuestGen.slate;
            var general = QuestGen.Root.GetModExtension<QuestChainExtension>().questChainDef.Worker.GetUniquePawn("General");
            slate.Set("General", general);

            var points = slate.Get("points", 0f);
            if (!TryFindSiteTile(out var tile))
            {
                Log.Error("Failed to find any suitable site tile for the Ancient ICBM Launch Site quest.");
                return;
            }

            var site = GenerateSite(points, tile, Faction.OfEntities, out string siteMapGeneratedSignal, out string siteMapRemovedSignal, failWhenMapRemoved: true, timeoutTicks: 14 * GenDate.TicksPerDay);

            QuestGen.quest.SignalPassActivable(delegate
            {
                QuestGen.quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
            }, siteMapGeneratedSignal, QuestGenUtility.HardcodedSignalWithQuestID("site." + DeathlifeApocalypsisStarted));

            QuestGen.quest.SignalPassActivable(delegate
            {
                QuestGen.quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
            }, siteMapGeneratedSignal, QuestGenUtility.HardcodedSignalWithQuestID("site." + GeneralDefeated));

            QuestGen.quest.SignalPassActivable(delegate
            {
                var triggerGameConditionPart = new QuestPart_TriggerGameCondition();
                triggerGameConditionPart.inSignal = siteMapRemovedSignal;
                triggerGameConditionPart.gameConditionDef = GameConditionDefOf.PsychicDrone;
                triggerGameConditionPart.durationTicks = (int)(Rand.Range(1f, 3f) * GenDate.TicksPerDay);
                QuestGen.quest.AddPart(triggerGameConditionPart);
            }, siteMapGeneratedSignal, siteMapRemovedSignal);

            QuestGen.quest.SignalPassActivable(delegate
            {
                QuestGen.quest.End(QuestEndOutcome.Success, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
            }, siteMapGeneratedSignal, QuestGenUtility.HardcodedSignalWithQuestID("site." + TerminalDestroyed));
        }
    }
}
