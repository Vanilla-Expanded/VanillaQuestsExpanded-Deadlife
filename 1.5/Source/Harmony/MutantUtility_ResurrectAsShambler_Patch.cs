using HarmonyLib;
using Verse;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;
using System.Linq;
using KCSG;

namespace VanillaQuestsExpandedDeadlife
{
    [HarmonyPatch(typeof(MutantUtility), nameof(MutantUtility.ResurrectAsShambler))]
    public static class MutantUtility_ResurrectAsShambler_Patch
    {
        public static void Postfix(Pawn pawn, Faction faction)
        {
            if (faction == Faction.OfEntities)
            {
                var map = pawn.Map;
                if (map != null && map.Parent is Site site && site.parts.Any(p => p.def == InternalDefOf.VQE_AncientSilo || p.def == InternalDefOf.VQE_AncientICBMLaunchSite))
                {
                    var lord = map.lordManager.lords.Where(l => l.faction == faction && l.LordJob is LordJob_DefendBaseNoEat).MinBy(x => x.ownedPawns.First().Position.DistanceTo(pawn.Position));
                    if (lord != null)
                    {
                        lord.AddPawn(pawn);
                    }
                    else
                    {
                        lord = map.lordManager.lords.Where(l => l.faction == faction && l.LordJob is LordJob_AssaultColony).MinBy(x => x.ownedPawns.First().Position.DistanceTo(pawn.Position));
                        if (lord != null)
                        {
                            lord.AddPawn(pawn);
                        }
                        else
                        {
                            var lordJob = new LordJob_DefendBaseNoEat(pawn.Faction, pawn.Position);
                            lord = LordMaker.MakeNewLord(pawn.Faction, lordJob, map);
                            lord.AddPawn(pawn);
                        }
                    }
                }
            }
        }
    }
}
