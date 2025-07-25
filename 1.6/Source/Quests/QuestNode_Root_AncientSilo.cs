using RimWorld;
using RimWorld.QuestGen;
using Verse;
using System.Linq;
using RimWorld.Planet;

namespace VanillaQuestsExpandedDeadlife
{
    public class QuestNode_Root_AncientSilo : QuestNode_Site
    {
        public override SitePartDef QuestSite => InternalDefOf.VQE_AncientSilo;
        public override Predicate<Map, PlanetTile> TileValidator => (Map map, PlanetTile tile) 
            => map == null || Find.WorldGrid.ApproxDistanceInTiles(tile, map.Tile) <= 50;
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
                else
                {
                    slate.Set("askerIsNull", true);
                }
            }
            else
            {
                slate.Set("askerIsNull", true);
            }

            var points = slate.Get("points", 0f);
            if (!TryFindSiteTile(out var tile))
            {
                Log.Error("Failed to find any suitable site tile for the Ancient Silo quest.");
                return;
            }
            var site = GenerateSite(points, tile, Faction.OfEntities, out string siteMapGeneratedSignal, out _, failWhenMapRemoved: true);

            var lootPart = new QuestPart_LootBuildingsOpened();
            lootPart.mapParent = site;
            lootPart.inSignalEnable = siteMapGeneratedSignal;
            lootPart.inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.LootableBuildingOpened");
            lootPart.applyOnPocketMap = true;
            QuestGen.quest.AddPart(lootPart);
        }
    }
}
