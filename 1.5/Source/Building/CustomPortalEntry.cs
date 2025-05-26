using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;
using RimWorld.Planet;

namespace VanillaQuestsExpandedDeadlife;

public class CustomPortalEntry : MapPortal
{
    private Map destinationMap;
    private MapPortal destinationExit;
    private CompCustomPortal CustomPortalComp => GetComp<CompCustomPortal>();
    public override string EnterCommandString => CustomPortalComp.Props.enterCommandKey.Translate();
    protected override Texture2D EnterTex => ContentFinder<Texture2D>.Get(CustomPortalComp.Props.enterTexPath);
    public override bool AutoDraftOnEnter => true;
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_References.Look(ref destinationMap, "destinationMap");
        Scribe_References.Look(ref destinationExit, "destinationExit");
    }

    public override Map GetOtherMap()
    {
        if (destinationMap == null)
        {
            GenerateDestinationMap();
        }
        return destinationMap;
    }

    public override IntVec3 GetDestinationLocation()
    {
        return destinationExit?.Position ?? IntVec3.Invalid;
    }

    public override void OnEntered(Pawn pawn)
    {
        base.OnEntered(pawn);
        if (Find.CurrentMap == base.Map)
        {
            CustomPortalComp.Props.traverseSound?.PlayOneShot(this);
        }
        else if (Find.CurrentMap == destinationExit.Map)
        {
            CustomPortalComp.Props.traverseSound?.PlayOneShot(destinationExit);
        }
    }

    public override IEnumerable<Gizmo> GetGizmos()
    {
        foreach (var gizmo in base.GetGizmos())
        {
            yield return gizmo;
        }
        if (destinationMap != null)
        {
            yield return new Command_Action
            {
                defaultLabel = CustomPortalComp.Props.viewDestinationCommandKey.Translate(),
                defaultDesc = CustomPortalComp.Props.viewDestinationDescKey.Translate(),
                icon = ContentFinder<Texture2D>.Get(CustomPortalComp.Props.viewDestinationTexPath),
                action = delegate
                {
                    CameraJumper.TryJumpAndSelect(destinationExit);
                }
            };
        }
    }

    private void GenerateDestinationMap()
    {
        var mapSize = CustomPortalComp.Props.mapSize;
        var exitDef = CustomPortalComp.Props.exitDef;
        var mapGeneratorDef = CustomPortalComp.Props.mapGeneratorDef;

        destinationMap = PocketMapUtility.GeneratePocketMap(mapSize, mapGeneratorDef, null, base.Map);
        destinationExit = destinationMap.listerThings.ThingsOfDef(exitDef).First() as MapPortal;
        if (destinationExit != null)
        {
            ((CustomPortalExit)destinationExit).portalEntry = this;
        }
    }
}
