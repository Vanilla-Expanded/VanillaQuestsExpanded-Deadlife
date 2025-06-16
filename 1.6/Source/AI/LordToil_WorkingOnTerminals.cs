using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;

namespace VanillaQuestsExpandedDeadlife
{
    public class LordToil_WorkingOnTerminals : LordToil
    {
        public override void UpdateAllDuties()
        {
            var duty = new PawnDuty(InternalDefOf.VQE_GeneralAI);
            foreach (var pawn in lord.ownedPawns)
            {
                pawn.mindState.duty = duty;
            }
        }
    }
}
