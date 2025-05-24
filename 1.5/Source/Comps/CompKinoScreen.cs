using Verse;
using RimWorld;
using UnityEngine;

namespace VanillaQuestsExpandedDeadlife
{
    public class CompKinoScreen : ThingComp
    {
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                Messages.Message("VQED_KinoScreenRequiresProjector".Translate(), MessageTypeDefOf.SilentInput);
            }
        }

        public override string CompInspectStringExtra()
        {
            if (parent.Map is null)
            {
                return null;
            }
            var reason = CanWork();
            if (!reason.Accepted)
            {
                return reason.Reason;
            }
            return null;
        }
        
        public AcceptanceReport CanWork()
        {
            var projector = parent.GetProjector();
            if (projector == null)
            {
                return "VQED_MissingKinoProjector".Translate();
            }
            
            if (projector.Rotation != parent.Rotation.Opposite)
            {
                return "VQED_KinoScreenWrongRotation".Translate();
            }

            var powerComp = projector.TryGetComp<CompPowerTrader>();
            if (!powerComp.PowerOn)
            {
                return "VQED_KinoProjectorUnpowered".Translate();
            }
            return true;
        }
    }
}
