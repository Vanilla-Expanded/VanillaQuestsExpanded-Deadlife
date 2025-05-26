using HarmonyLib;
using RimWorld;
using Verse;
using VanillaQuestsExpandedDeadlife;
using System.Linq;

namespace VanillaQuestsExpandedDeadlife
{
    [HarmonyPatch(typeof(Pawn), nameof(Pawn.GetInspectString))]
    public static class Pawn_GetInspectString_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn __instance, ref string __result)
        {
            if (__instance.equipment?.Primary != null)
            {
                var comp = __instance.equipment.Primary.TryGetComp<CompWeaponDeteriorable>();
                if (comp != null)
                {
                    var compInspectString = comp.CompInspectStringExtra();
                    if (!compInspectString.NullOrEmpty())
                    {
                        var lines = __result.Split('\n').ToList();
                        var equippedLineIndex = -1;
                        for (var i = 0; i < lines.Count; i++)
                        {
                            if (lines[i].StartsWith("Equipped".TranslateSimple() + ": "))
                            {
                                equippedLineIndex = i;
                                break;
                            }
                        }

                        if (equippedLineIndex != -1)
                        {
                            lines.Insert(equippedLineIndex + 1, compInspectString);
                            __result = string.Join("\n", lines);
                        }
                        else
                        {
                            __result += "\n" + compInspectString;
                        }
                    }
                }
            }
        }
    }
}
