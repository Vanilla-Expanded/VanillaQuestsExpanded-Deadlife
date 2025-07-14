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
        public static bool IsDeadlifeQuestMap(this Map map)
        {
            if (map is null) return false;
            var parent = map.Parent is PocketMapParent parent2 ? parent2.sourceMap.Parent : map.Parent;
            if (parent is Site site && site.parts.Any(p => p.def == InternalDefOf.VQE_AncientSilo || p.def == InternalDefOf.VQE_AncientICBMLaunchSite))
            {
                return true;
            }
            return false;
        }

        public static void Prefix(Pawn pawn, ref Faction faction)
        {
            if (faction != Faction.OfEntities)
            {
                var map = pawn.MapHeld;
                if (map.IsDeadlifeQuestMap())
                {
                    faction = Faction.OfEntities;
                }
            }
        }

        public static void Postfix(Pawn pawn, Faction faction)
        {
            if (faction == Faction.OfEntities)
            {
                var map = pawn.Map;
                if (map.IsDeadlifeQuestMap())
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
                            var lordJob = new LordJob_DefendBaseNoEat(faction, pawn.Position, 180000);
                            lord = LordMaker.MakeNewLord(faction, lordJob, map);
                            lord.AddPawn(pawn);
                        }
                    }
                }
            }
        }
    }
}
