using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace VanillaQuestsExpandedDeadlife
{
    public class BuriedDeathPit : Building
    {
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
           
            List<Thing> list = map.thingGrid.ThingsListAt(Position);
            for (int i = 0; i < list.Count; i++)
            {
                Building existingBuilding = list[i] as Building;
                if (existingBuilding != this && existingBuilding is DeathPit existingBuildingAsDeathPit)
                {
                    allowDestroyNonDestroyable = true;
                    try
                    {
                        existingBuilding.Destroy();
                    }
                    finally
                    {
                        allowDestroyNonDestroyable = false;
                    }
                   
                }
            }

            base.SpawnSetup(map, respawningAfterLoad);



        }

    }
}
