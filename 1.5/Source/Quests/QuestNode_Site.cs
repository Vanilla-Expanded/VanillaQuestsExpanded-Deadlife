using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using VFECore;

namespace VanillaQuestsExpandedDeadlife
{
    public abstract class QuestNode_Site : QuestNode
    {
        public abstract SitePartDef QuestSite { get; }
        public virtual Predicate<Map, int> TileValidator { get; }
        public virtual List<BiomeDef> AllowedBiomes { get; }
        protected bool TryFindSiteTile(out int tile)
        {
            var allowedBiomes = AllowedBiomes;
            if (allowedBiomes != null && Find.WorldGrid.tiles.Any(x => allowedBiomes.Contains(x.biome)) is false)
            {
                allowedBiomes = null;
            }
            var predicator = TileValidator;
            var map = QuestGen_Get.GetMap();
            var tiles = Find.World.tilesInRandomOrder.Tiles.Where((int x) => (predicator == null || predicator(map, x)) && IsValidTile(x, allowedBiomes));
            if (tiles.TryRandomElement(out tile))
            {
                return true;
            }
            tile = -1;
            return false;
        }

        public static bool IsValidTile(int tile, List<BiomeDef> allowedBiomes = null)
        {
            Tile tile2 = Find.WorldGrid[tile];
            if (!tile2.biome.canBuildBase)
            {
                return false;
            }
            if (!tile2.biome.implemented)
            {
                return false;
            }
            if (tile2.hilliness == Hilliness.Impassable)
            {
                return false;
            }
            if (Find.WorldObjects.AnyMapParentAt(tile) || Current.Game.FindMap(tile) != null
            || Find.WorldObjects.AnyWorldObjectOfDefAt(WorldObjectDefOf.AbandonedSettlement, tile))
            {
                return false;
            }
            if (allowedBiomes != null && allowedBiomes.Count > 0 && !allowedBiomes.Contains(tile2.biome))
            {
                return false;
            }
            return true;
        }

        protected override bool TestRunInt(Slate slate)
        {
            return true;
        }

        protected Site GenerateSite(float points,
            int tile, Faction parentFaction, out string siteMapGeneratedSignal, bool failWhenMapRemoved = true, int timeoutTicks = 0)
        {
            SitePartParams sitePartParams = new SitePartParams
            {
                points = points,
                threatPoints = points
            };

            Site site = QuestGen_Sites.GenerateSite(new List<SitePartDefWithParams>
            {
                new SitePartDefWithParams(QuestSite, sitePartParams)
            }, tile, parentFaction);

            site.doorsAlwaysOpenForPlayerPawns = true;
            if (parentFaction != null && site.Faction != parentFaction)
            {
                site.SetFaction(parentFaction);
            }
            Quest quest = QuestGen.quest;
            Slate slate = QuestGen.slate;
            slate.Set("site", site);
            quest.SpawnWorldObject(site);
            if (timeoutTicks > 0)
            {
                quest.WorldObjectTimeout(site, timeoutTicks);
            }
            string siteMapRemovedSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
            siteMapGeneratedSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");

            if (failWhenMapRemoved)
            {
                quest.SignalPassActivable(delegate
                {
                    quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, sendStandardLetter: true);
                }, siteMapGeneratedSignal, siteMapRemovedSignal);
            }
            
            
            return site;
        }

        protected Faction CreateFaction(FactionDef factionDef)
        {
            var quest = QuestGen.quest;
            var slate = QuestGen.slate;
            FactionGeneratorParms parms = new FactionGeneratorParms(factionDef, default(IdeoGenerationParms), true);
            parms.ideoGenerationParms = new IdeoGenerationParms(parms.factionDef);
            Faction parentFaction = FactionGenerator.NewGeneratedFaction(parms);
            parentFaction.temporary = true;
            parentFaction.factionHostileOnHarmByPlayer = true;
            parentFaction.neverFlee = true;
            Find.FactionManager.Add(parentFaction);
            quest.ReserveFaction(parentFaction);
            slate.Set("parentFaction", parentFaction);
            slate.Set("siteFaction", parentFaction);
            return parentFaction;
        }

        protected bool PrepareQuest(out Map map, out float points,
        out int tile)
        {
            var slate = QuestGen.slate;
            map = QuestGen_Get.GetMap();
            QuestGenUtility.RunAdjustPointsForDistantFight();
            points = slate.Get("points", 0f);
            slate.Set("playerFaction", Faction.OfPlayer);
            slate.Set("map", map);
            if (!TryFindSiteTile(out tile))
            {
                return false;
            }
            return true;
        }
    }
}
