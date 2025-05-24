using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.Sound;
using static HarmonyLib.Code;
using Verse.Noise;
using Verse.AI;




namespace VanillaQuestsExpandedDeadlife
{
    public class Kickstartable : Building
    {

        MapComponent_DeadlifeBuildingsInMap cachedMapComp;
        CompKickstartablePowerPlant cachedComp;

        public CompKickstartablePowerPlant compKickstartablePowerPlant
        {

            get
            {
                if (cachedComp is null)
                {
                    cachedComp = this.TryGetComp<CompKickstartablePowerPlant>();
                }return cachedComp;
            }

        }
        public MapComponent_DeadlifeBuildingsInMap compDeadlifeBuildingsInMap
        {

            get
            {
                if (cachedMapComp is null)
                {
                    cachedMapComp = Map.GetComponent<MapComponent_DeadlifeBuildingsInMap>(); ;
                }
                return cachedMapComp;
            }

        }

    

        public override IEnumerable<Gizmo> GetGizmos()
        {

            foreach (Gizmo c in base.GetGizmos())
            {
                yield return c;
            }

            Command_Action command_Action = new Command_Action();

            if (compDeadlifeBuildingsInMap?.kickstartables_InMap.Contains(this) == false)
            {
                command_Action.defaultDesc = "VQED_KickstartDesc".Translate();
                command_Action.defaultLabel = "VQED_Kickstart".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/StartBurnoutGenerator", true);
                command_Action.hotKey = KeyBindingDefOf.Misc1;
                command_Action.action = delegate
                {
                    Map.GetComponent<MapComponent_DeadlifeBuildingsInMap>()?.AddKickstartableToMap(this);
                };
            }
            else
            {
                command_Action.defaultDesc = "VQED_KickstartDesc".Translate();
                command_Action.defaultLabel = "VQED_Kickstart".Translate();
                command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/StartBurnoutGenerator", true);
                command_Action.Disabled = true;
            }

            yield return command_Action;




        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {

            compDeadlifeBuildingsInMap.RemoveKickstartableFromMap(this);
            
            base.Destroy(mode);

        }

        public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
        {

            compDeadlifeBuildingsInMap.RemoveKickstartableFromMap(this);
           
            base.Kill(dinfo, exactCulprit);

        }


        public void Notify_Kickstarted()
        {
            compKickstartablePowerPlant.active = true;
            compDeadlifeBuildingsInMap.RemoveKickstartableFromMap(this);
           
        }



        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
        {
            foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
            {
                yield return floatMenuOption;
            }
            if (selPawn.CanReserve(this) && selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                if (!selPawn.CanReach(this, PathEndMode.OnCell, Danger.Deadly))
                {
                    yield return new FloatMenuOption("CannotUseReason".Translate("NoPath".Translate().CapitalizeFirst()), null);
                }
                else
                {
                    yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("VQED_Kickstart".Translate().CapitalizeFirst(), delegate
                    {
                        selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(InternalDefOf.VQED_KickstartGenerator, this), JobTag.Misc);
                    }), selPawn, this);
                }



            }
        }


    }
}
