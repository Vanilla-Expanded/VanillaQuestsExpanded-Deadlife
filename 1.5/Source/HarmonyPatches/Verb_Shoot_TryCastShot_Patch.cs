using HarmonyLib;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedDeadlife
{
    [HarmonyPatch(typeof(Verb_Shoot), "TryCastShot")]
    public static class Verb_Shoot_TryCastShot_Patch
    {
        public static void Postfix(bool __result, Verb_Shoot __instance)
        {
            if (__result && __instance.EquipmentSource != null)
            {
                var comp = __instance.EquipmentSource.TryGetComp<CompWeaponDeteriorable>();
                if (comp != null)
                {
                    comp.Notify_ShotFired(__instance);
                }
            }
        }
    }
}
