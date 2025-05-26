using RimWorld;
using RimWorld.QuestGen;
using Verse;
using System.Linq;

namespace VanillaQuestsExpandedDeadlife
{
    public class QuestNode_Root_AncientSilo : QuestNode_Site
    {
        public override SitePartDef QuestSite => InternalDefOf.VQE_AncientSilo;
        public override Predicate<Map, int> TileValidator => (Map map, int tile) => Find.WorldGrid.ApproxDistanceInTiles(tile, map.Tile) <= 50;
        public static bool noAsker;
        protected override bool TestRunInt(Slate slate)
        {
            if (noAsker is false)
            {
                var askerPawn = GetAsker();
                if (askerPawn is null)
                {
                    return false;
                }
            }
            return base.TestRunInt(slate);
        }

        private Pawn GetAsker()
        {
            var friendlyFaction = Find.FactionManager.AllFactionsVisible.Where(f => f.def.humanlikeFaction && !f.HostileTo(Faction.OfPlayer) && f.leader != null).RandomElementWithFallback();
            if (friendlyFaction != null)
            {
                return friendlyFaction.leader;
            }
            return null;
        }
        protected override void RunInt()
        {
            var slate = QuestGen.slate;
            if (noAsker is false)
            {
                var askerPawn = GetAsker();
                if (askerPawn != null)
                {
                    slate.Set("asker", askerPawn);
                }
            }

            var points = slate.Get("points", 0f);
            if (!TryFindSiteTile(out var tile))
            {
                Log.Error("Failed to find any suitable site tile for the Ancient Silo quest.");
                return;
            }
            var site = GenerateSite(points, tile, Faction.OfAncientsHostile, out string siteMapGeneratedSignal, failWhenMapRemoved: true);

            var questPart = new QuestPart_Site();
            questPart.mapParent = site;
            questPart.inSignalEnable = siteMapGeneratedSignal;
            QuestGen.quest.AddPart(questPart);
            
            var lootPart = new QuestPart_LootBuildingsOpened();
            lootPart.mapParent = site;
            lootPart.inSignalEnable = siteMapGeneratedSignal;
            lootPart.inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.LootableBuildingOpened");
            QuestGen.quest.AddPart(lootPart);
        }
    }
}
