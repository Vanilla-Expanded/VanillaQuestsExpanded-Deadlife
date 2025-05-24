using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace VanillaQuestsExpandedDeadlife
{
    public class QuestNode_Root_AncientSilo : QuestNode_Site
    {
        public override SitePartDef QuestSite => InternalDefOf.VQE_AncientSilo;
        public override Predicate<Map, int> TileValidator => (Map map, int tile) => Find.WorldGrid.ApproxDistanceInTiles(tile, map.Tile) <= 50;
        protected override void RunInt()
        {
            var slate = QuestGen.slate;
            var points = slate.Get("points", 0f);
            if (!TryFindSiteTile(out var tile))
            {
                Log.Error("Failed to find any suitable site tile for the Ancient Silo quest.");
                return;
            }
            var site = GenerateSite(points, tile, Faction.OfAncientsHostile, out string siteMapGeneratedSignal, failWhenMapRemoved: true);
        }
    }
}
