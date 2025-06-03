using Verse;
using System.Linq;
using RimWorld;
using Verse.Sound;
using VFECore;
using UnityEngine;
namespace VanillaQuestsExpandedDeadlife
{

    [HotSwappable]
    public class ICBMSilo : VanillaFurnitureExpanded.SwappableBuilding
    {
        private int? ticksLaunching;
        override public void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksLaunching, "ticksLaunching", null);
        }

        override public void Tick()
        {
            base.Tick();
            if (ticksLaunching.HasValue)
            {
                var smokePos = DrawPos;
                smokePos.y = 50;
                FleckMaker.ThrowDustPuff(smokePos, this.Map, 5f);
                ticksLaunching--;
                if (ticksLaunching == -1)
                {
                    Notify_Swap();
                }
            }
        }

        public override void Notify_Swap()
        {
            var pos = Position;
            var map = Map;

            if (ticksLaunching == -1)
            {
                base.Notify_Swap();
                var emptySilo = pos.GetThingList(map).OfType<EmptyICBMSilo>().FirstOrDefault();
                emptySilo.startLaunching = true;
                var skyfaller = (Skyfaller_DeadlifeICBM)SkyfallerMaker.MakeSkyfaller(InternalDefOf.VQED_ICBMSkyfaller, (Thing)null);
                GenSpawn.Spawn(skyfaller, pos, map);
            }
            else
            {
                ticksLaunching = 300;
                InternalDefOf.VQED_ICBMLaunch.PlayOneShot(new TargetInfo(pos, map));
            }
        }
    }
}
