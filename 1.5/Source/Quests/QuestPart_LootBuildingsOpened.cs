using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using System.Linq;
using System.Collections.Generic;
using VanillaFurnitureExpanded;

namespace VanillaQuestsExpandedDeadlife
{
    public class QuestPart_LootBuildingsOpened : QuestPart_Site
    {
        [NoTranslate]
        public string inSignal;
        private int totalLootBuildings;
        private int openedLootBuildingsCount;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref inSignal, "inSignal");
            Scribe_Values.Look(ref inSignalEnable, "inSignalEnable");
            Scribe_Values.Look(ref totalLootBuildings, "totalLootBuildings");
            Scribe_Values.Look(ref openedLootBuildingsCount, "openedLootBuildingsCount");
        }

        public override void Notify_QuestSignalReceived(Signal signal)
        {
            base.Notify_QuestSignalReceived(signal);
            if (signal.tag == inSignalEnable)
            {
                if (Map != null && totalLootBuildings <= 0)
                {
                    var lootBuildings = Map.listerThings.GetThingsOfType<LootableBuilding>().ToList();
                    totalLootBuildings = lootBuildings.Count;
                    openedLootBuildingsCount = 0;
                }
            }
            else if (signal.tag == inSignal)
            {
                openedLootBuildingsCount++;
                if (totalLootBuildings > 0 && (float)openedLootBuildingsCount / totalLootBuildings >= 0.25f)
                {
                    this.quest.End(QuestEndOutcome.Success);
                }
            }
        }
    }
}
