using RimWorld;
using Verse;
using System.Linq;

namespace VanillaQuestsExpandedDeadlife
{
    public class GenStep_AncientICBMLaunchSite : GenStep
    {
        public StructureSetDef structureSetDef;
        public override int SeedPart => 987654322;
        public override void Generate(Map map, GenStepParams parms)
        {
            StructureSetGenerator.Generate(map, structureSetDef);
        }
    }
}
