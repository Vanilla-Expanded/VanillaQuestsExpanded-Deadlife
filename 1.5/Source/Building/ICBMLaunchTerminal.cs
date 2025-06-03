using Verse;
using System.Linq;
namespace VanillaQuestsExpandedDeadlife
{
    public class ICBMLaunchTerminal : VanillaFurnitureExpanded.SwappableBuilding
    {
        public override void Notify_Swap()
        {
            var loadedSilos = Map.listerThings.ThingsOfDef(InternalDefOf.VQED_LoadedICBMSilo).OfType<ICBMSilo>().ToList();
            foreach (var silo in loadedSilos)
            {
                silo.Notify_Swap();
            }
            base.Notify_Swap();
        }
    }
}
