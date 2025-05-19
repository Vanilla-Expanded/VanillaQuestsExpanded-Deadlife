using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;
using RimWorld.Planet;

namespace VanillaQuestsExpandedDeadlife
{
    public class MapComponent_DeadlifeBuildingsInMap : MapComponent
    {

      
        public HashSet<Thing> kickstartables_InMap = new HashSet<Thing>();




        public MapComponent_DeadlifeBuildingsInMap(Map map) : base(map)
        {
        }



        public override void ExposeData()
        {
            base.ExposeData();
         
            Scribe_Collections.Look(ref this.kickstartables_InMap, "restartables_InMap", LookMode.Reference);

        }



        public void AddKickstartableToMap(Thing thing)
        {
            if (!kickstartables_InMap.Contains(thing))
            {
                kickstartables_InMap.Add(thing);
            }
        }

        public void RemoveKickstartableFromMap(Thing thing)
        {
            if (kickstartables_InMap.Contains(thing))
            {
                kickstartables_InMap.Remove(thing);
            }

        }
       


    }


}
