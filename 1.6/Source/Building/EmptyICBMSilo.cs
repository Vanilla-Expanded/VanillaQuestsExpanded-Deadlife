using Verse;
using Verse.Sound;
using RimWorld;
using UnityEngine;
namespace VanillaQuestsExpandedDeadlife
{
    public class EmptyICBMSilo : Building
    {
        public bool startLaunching;

        private int ticksLaunching;

        protected override void Tick()
        {
            base.Tick();
            var smokePos = DrawPos;
            smokePos.y = 50;
            if (startLaunching)
            {
                FleckMaker.ThrowDustPuff(smokePos, this.Map, 5f);
                ticksLaunching++;
                if (ticksLaunching == 1000)
                {
                    startLaunching = false;
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref startLaunching, "startLaunching", false);
            Scribe_Values.Look(ref ticksLaunching, "ticksLaunching", 0);
        }
    }
}
