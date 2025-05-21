using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDeadlife
{
    public class DeathDoorway: Building
    {

        public override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(30))
            {
                GasUtility.AddGas(Position, Map, GasType.DeadlifeDust, 0.2f);
            }
            
        }


    }
}
