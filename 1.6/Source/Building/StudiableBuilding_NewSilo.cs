using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.Sound;
using static HarmonyLib.Code;
using Verse.Noise;
using Verse.AI;

using VEF.Buildings;


namespace VanillaQuestsExpandedDeadlife
{
    public class StudiableBuilding_NewSilo : StudiableBuilding
    {
        public override void Study(Pawn pawn)
        {
            base.Study(pawn);
            QuestNode_Root_AncientSilo.noAsker = true;
            Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(InternalDefOf.VQE_Deadlife_AncientSilo, StorytellerUtility.DefaultThreatPointsNow(Find.World));
            QuestUtility.SendLetterQuestAvailable(quest);
            QuestNode_Root_AncientSilo.noAsker = false;
        }
    }
}
