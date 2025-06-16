using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;
using RimWorld.Planet;

namespace VanillaQuestsExpandedDeadlife;

public class CustomPortalExit : MapPortal
{
    public MapPortal portalEntry;

    private CompCustomPortal CustomPortalComp => GetComp<CompCustomPortal>();

    public override string EnterString => CustomPortalComp.Props.enterCommandKey.Translate();

    protected override Texture2D EnterTex => ContentFinder<Texture2D>.Get(CustomPortalComp.Props.enterTexPath);

    public override Map GetOtherMap()
    {
        return portalEntry.Map;
    }

    public override IntVec3 GetDestinationLocation()
    {
        return portalEntry.Position;
    }

    public override void OnEntered(Pawn pawn)
    {
        base.OnEntered(pawn);
        if (Find.CurrentMap == base.Map)
        {
            CustomPortalComp.Props.traverseSound?.PlayOneShot(SoundInfo.InMap(this));
        }
        else if (Find.CurrentMap == portalEntry.Map)
        {
            CustomPortalComp.Props.traverseSound?.PlayOneShot(SoundInfo.InMap(portalEntry));
        }
    }

    public override IEnumerable<Gizmo> GetGizmos()
    {
        foreach (var gizmo in base.GetGizmos())
        {
            yield return gizmo;
        }
        yield return new Command_Action
        {
            defaultLabel = CustomPortalComp.Props.viewDestinationCommandKey.Translate(),
            defaultDesc = CustomPortalComp.Props.viewDestinationDescKey.Translate(),
            icon = ContentFinder<Texture2D>.Get(CustomPortalComp.Props.viewDestinationTexPath),
            action = delegate
            {
                CameraJumper.TryJumpAndSelect(new TargetInfo(portalEntry));
            }
        };
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_References.Look(ref portalEntry, "portalEntry");
    }
}
