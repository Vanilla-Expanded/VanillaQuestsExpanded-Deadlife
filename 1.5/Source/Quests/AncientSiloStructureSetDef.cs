using Verse;
using System.Collections.Generic;
using UnityEngine;
using RimWorld;
using System.Xml;

namespace VanillaQuestsExpandedDeadlife
{
    public class AncientSiloStructureSetDef : Def
    {
        public List<StructurePatternOffset> structureLayouts;
    }

    public class StructurePatternOffset
    {
        public string pattern;
        public IntVec3 offset;
        public List<PawnSpawnOption> spawnEnemies;
    }

    public class PawnSpawnOption
    {
        public PawnKindDef kind;
        public IntRange count;

        public void LoadDataFromXmlCustom(XmlNode xmlRoot)
        {
            DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "kind", xmlRoot.Name);
            count = ParseHelper.FromString<IntRange>(xmlRoot.FirstChild.Value);
        }
    }
}
