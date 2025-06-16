using Verse;

namespace VanillaQuestsExpandedDeadlife
{
    public class CompProperties_WeaponDeteriorable : CompProperties
    {
        public int shotsBeforeBreak;

        public CompProperties_WeaponDeteriorable()
        {
            compClass = typeof(CompWeaponDeteriorable);
        }
    }
}
