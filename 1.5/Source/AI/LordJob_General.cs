using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;
using RimWorld;

namespace VanillaQuestsExpandedDeadlife
{
    public class LordJob_General : LordJob
    {
        public override StateGraph CreateGraph()
        {
            var graph = new StateGraph();

            var workOnTerminalToil = new LordToil_WorkingOnTerminals();
            graph.StartingToil = workOnTerminalToil;

            return graph;
        }
    }
}
