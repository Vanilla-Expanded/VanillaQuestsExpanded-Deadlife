using Verse;

namespace VanillaQuestsExpanded_Deadlife;

public class CompProperties_CustomPortal : CompProperties
{
    public IntVec3 mapSize;
    public ThingDef exitDef;
    public MapGeneratorDef mapGeneratorDef;
    public string enterTexPath;
    public string viewDestinationTexPath;
    public string enterCommandKey;
    public string viewDestinationCommandKey;
    public string viewDestinationDescKey;

    public SoundDef traverseSound;

    public CompProperties_CustomPortal()
    {
        compClass = typeof(CompCustomPortal);
    }
}
