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

        protected override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(20))
            {
                GasUtility.AddDeadifeGas(Position, Map, Faction.OfEntities, 250);
            }

        }


    }
}
