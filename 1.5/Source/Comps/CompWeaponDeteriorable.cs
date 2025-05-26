using Verse;
using RimWorld;
using UnityEngine;

namespace VanillaQuestsExpandedDeadlife
{
    public class CompWeaponDeteriorable : ThingComp
    {
        public int shotsFired;

        public CompProperties_WeaponDeteriorable Props => (CompProperties_WeaponDeteriorable)props;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref shotsFired, "shotsFired", 0);
        }

        public override string CompInspectStringExtra()
        {
            if (Props.shotsBeforeBreak > 0)
            {
                return "VQED_WeaponDeteriorationInfo".Translate() +"\n" + ShotRemainingInfo();
            }
            return base.CompInspectStringExtra();
        }
        
        public string ShotRemainingInfo() => "VQED_ShotRemaining".Translate(Props.shotsBeforeBreak - shotsFired);

        public void Notify_ShotFired(Verb_Shoot verb)
        {
            shotsFired++;
            if (shotsFired >= Props.shotsBeforeBreak)
            {
                var weaponName = parent.LabelNoParenthesisCap;
                var pawnName = verb.CasterPawn?.LabelShort;
                var message = "VQED_WeaponDeterioratedMessage".Translate(weaponName, pawnName);
                Messages.Message(message, MessageTypeDefOf.NegativeEvent, false);
                parent.Destroy();
            }
        }
    }
}
