using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;
using RimWorld;
using UnityEngine;
using System;

namespace VanillaQuestsExpandedDeadlife
{
    public class JobDriver_WorkOnTerminal : JobDriver
    {
        public enum TrapCount { One, All, RandomCaskets }
        private int WorkTicks => TargetThingA.def == InternalDefOf.VQED_ICBMLaunchTerminal ? 7200 : 1500;
        private Sustainer keyboardSustainer;
        private Sustainer icbmSirenSustainer;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
            var work = Toils_General.Wait(WorkTicks);
            WithProgressBarToilDelay(work, TargetIndex.A, interpolateBetweenActorAndTarget: true);
            work.FailOnDespawnedOrNull(TargetIndex.A);
            work.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
            work.tickAction = delegate
            {
                if (keyboardSustainer == null)
                {
                    StartSustainer(ref keyboardSustainer, InternalDefOf.VQED_SiloKeyboardClicking);
                }
                else
                {
                    keyboardSustainer.Maintain();
                }
                if (TargetThingA.def == InternalDefOf.VQED_ICBMLaunchTerminal)
                {
                    if (icbmSirenSustainer == null)
                    {
                        StartSustainer(ref icbmSirenSustainer, InternalDefOf.VQED_ICBMLaunchSiren, onCamera: true);
                    }
                    else
                    {
                        icbmSirenSustainer.Maintain();
                    }
                }
            };

            work.AddFinishAction(delegate
            {
                StopSustainer(ref keyboardSustainer);
                StopSustainer(ref icbmSirenSustainer);
            });

            yield return work;

            var complete = ToilMaker.MakeToil("CompleteTerminalWork");
            complete.initAction = delegate
            {
                CompleteTerminalWork();
            };
            complete.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return complete;
        }

        public static Toil WithProgressBarToilDelay(Toil toil, TargetIndex ind, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
        {
            return WithProgressBar(toil, ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toil.defaultDuration, interpolateBetweenActorAndTarget, offsetZ);
        }
        public static Toil WithProgressBar(Toil toil, TargetIndex ind, Func<float> progressGetter, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f, bool alwaysShow = false)
        {
            Effecter effecter = null;
            toil.AddPreTickAction(delegate
            {
                if (effecter == null)
                {
                    EffecterDef progressBar = EffecterDefOf.ProgressBar;
                    effecter = progressBar.Spawn();
                }
                else
                {
                    LocalTargetInfo localTargetInfo = ((ind == TargetIndex.None) ? LocalTargetInfo.Invalid : toil.actor.CurJob.GetTarget(ind));
                    if (!localTargetInfo.IsValid || (localTargetInfo.HasThing && !localTargetInfo.Thing.Spawned))
                    {
                        effecter.EffectTick(toil.actor, TargetInfo.Invalid);
                    }
                    else if (interpolateBetweenActorAndTarget)
                    {
                        effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), toil.actor);
                    }
                    else
                    {
                        effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), TargetInfo.Invalid);
                    }
                    MoteProgressBar mote = ((SubEffecter_ProgressBar)effecter.children[0]).mote;
                    if (mote != null)
                    {
                        mote.progress = Mathf.Clamp01(progressGetter());
                        mote.offsetZ = offsetZ;
                        mote.alwaysShow = alwaysShow;
                    }
                }
            });
            toil.AddFinishAction(delegate
            {
                if (effecter != null)
                {
                    effecter.Cleanup();
                    effecter = null;
                }
            });
            return toil;
        }

        private void StartSustainer(ref Sustainer sustainer, SoundDef soundDef, bool onCamera = false)
        {
            if (sustainer == null)
            {
                SoundInfo info = onCamera ? SoundInfo.OnCamera(MaintenanceType.PerTick)
                                          : SoundInfo.InMap(TargetThingA, MaintenanceType.PerTick);
                sustainer = soundDef.TrySpawnSustainer(info);
            }
        }

        private void StopSustainer(ref Sustainer sustainer)
        {
            if (sustainer != null)
            {
                sustainer.End();
                sustainer = null;
            }
        }

        private void CompleteTerminalWork()
        {
            if (TargetThingA is VanillaFurnitureExpanded.SwappableBuilding terminal)
            {
                if (terminal.def == InternalDefOf.VQED_ActiveTerminal)
                {
                    TriggerRandomTrapEffect();
                }
                terminal.Notify_Swap();
            }
        }

        private void TriggerRandomTrapEffect()
        {
            Map map = pawn.Map;
            string generalName = pawn.Name?.ToStringFull ?? "General";
            var trapEffects = new List<(ThingDef sourceDef, string messageKey, TrapCount count)>
            {
                (InternalDefOf.VQED_SealedDeathPit, "VQED_GeneralUnsealedDeathPit", TrapCount.One),
                (InternalDefOf.VQED_ClosedChute, "VQED_GeneralOpenedChutes", TrapCount.All),
                (InternalDefOf.VQED_ClosedDeadlifeCasket, "VQED_GeneralAwakenedOfficers", TrapCount.RandomCaskets),
                (InternalDefOf.VQED_DormantMilitaryTurret, "VQED_GeneralActivatedTurrets", TrapCount.All),
                (InternalDefOf.VQED_ClosedRotstinkVent, "VQED_GeneralOpenedRotstinkVents", TrapCount.All),
                (InternalDefOf.VQED_AncientKennel, "VQED_GeneralReleasedAnimals", TrapCount.All),
                (InternalDefOf.VQED_InactiveDeadlifeVent, "VQED_GeneralReactivatedDeadlifeVents", TrapCount.All)
            };

            var availableTrapEffects = trapEffects.Where(effect => map.listerThings.ThingsOfDef(effect.sourceDef).Any()).ToList();
            if (availableTrapEffects.Any())
            {
                var randomEffect = availableTrapEffects.RandomElement();
                var buildings = map.listerThings.ThingsOfDef(randomEffect.sourceDef);
                var toTransform = randomEffect.count switch
                {
                    TrapCount.One => buildings.Take(1),
                    TrapCount.RandomCaskets => buildings.Take(Rand.Range(4, 8)),
                    _ => buildings
                };
                int count = 0;
                var targets = new List<TargetInfo>();
                foreach (var building in toTransform.ToList())
                {
                    if (building is VanillaFurnitureExpanded.SwappableBuilding swappable)
                    {
                        targets.Add(new TargetInfo(building.Position, map));
                        swappable.Notify_Swap();
                        count++;
                    }
                }

                if (count > 0)
                    Messages.Message(randomEffect.messageKey.Translate(generalName), new LookTargets(targets), MessageTypeDefOf.ThreatBig);
            }
        }
    }
}
