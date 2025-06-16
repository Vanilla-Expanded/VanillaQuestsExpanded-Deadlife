using Verse.Sound;
using Verse;
using RimWorld;
using UnityEngine;
using HarmonyLib;
using VEF;
using LudeonTK;

namespace VanillaQuestsExpandedDeadlife
{
    // some code here is borrowed from VFE Mechanoids, coded by erdelf
    [HotSwappable]
    public class Skyfaller_DeadlifeICBM : Skyfaller
    {
        public const int SKYFALLER_LAUNCH_TICKS = 600;
        private int ticksUntilLaunch;
        private int ticksToImpactMaxPrivate;
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            this.ticksToImpactMaxPrivate = (int)AccessTools.Field(typeof(Skyfaller), "ticksToImpactMax").GetValue(this);
            this.angle = 0f;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ticksUntilLaunch, "ticksUntilLaunch", 0);
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            Thing thingForGraphic = this;
            float num = 0f;
            if (def.skyfaller.rotateGraphicTowardsDirection)
            {
                num = angle;
            }
            if (def.skyfaller.angleCurve != null)
            {
                angle = def.skyfaller.angleCurve.Evaluate(TimeInAnimation);
            }
            if (def.skyfaller.rotationCurve != null)
            {
                num += def.skyfaller.rotationCurve.Evaluate(TimeInAnimation);
            }
            if (def.skyfaller.xPositionCurve != null)
            {
                drawLoc.x += def.skyfaller.xPositionCurve.Evaluate(TimeInAnimation);
            }
            if (def.skyfaller.zPositionCurve != null)
            {
                drawLoc.z += def.skyfaller.zPositionCurve.Evaluate(TimeInAnimation);
            }
            Graphic.Draw(drawLoc, flip ? thingForGraphic.Rotation.Opposite : thingForGraphic.Rotation, thingForGraphic, num);
            DrawDropSpotShadow();
        }
        
        protected override void Tick()
        {
            ticksUntilLaunch++;
            Notify_ColorChanged();
            if (ticksUntilLaunch < SKYFALLER_LAUNCH_TICKS)
            {
                return;
            }
            else if (ticksUntilLaunch == SKYFALLER_LAUNCH_TICKS)
            {
                InternalDefOf.VQED_ICBMLaunch.PlayOneShot(new TargetInfo(Position, Map));
            }

            var smokePos = DrawPos;
            Utils.ThrowSmoke(smokePos, this.Map, 3f);

            base.Tick();
            var drawPos = this.GetDrawPosForMotes();
            if (this.Map == null || !drawPos.InBounds(this.Map))
                return;


            FleckCreationData data = FleckMaker.GetDataStatic(drawPos, this.Map, FleckDefOf.HeatGlow, Rand.Range(4f, 6f) * 2);
            data.rotationRate = Rand.Range(-3f, 3f);
            data.velocityAngle = Rand.Range(0, 360);
            data.velocitySpeed = 0.12f;
            this.Map.flecks.CreateFleck(data);
        }

        public override Color DrawColor
        {
            get
            {
                var color = base.DrawColor;
                var brightness = Mathf.Lerp(0.5f, 1f, Mathf.Min((float)ticksUntilLaunch / (float)SKYFALLER_LAUNCH_TICKS, 1f));
                color.r *= brightness;
                color.g *= brightness;
                color.b *= brightness;
                return color;
            }
        }

        public override Vector3 DrawPos
        {
            get
            {
                var pos = base.DrawPos;
                pos.y = 4f;
                return pos;
            }
        }

        private Vector3 GetDrawPosForMotes()
        {
            int ticksToImpactPrediction = this.ticksToImpact - GenTicks.TicksPerRealSecond / 2;


            float timeInAnim = 1 - ticksToImpactPrediction / this.ticksToImpactMaxPrivate;

            float currentSpeed = (this.def.skyfaller.speedCurve?.Evaluate(timeInAnim) ?? 1) * this.def.skyfaller.speed;

            switch (this.def.skyfaller.movementType)
            {
                case SkyfallerMovementType.Accelerate:
                    return SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, ticksToImpactPrediction, this.angle, currentSpeed);
                case SkyfallerMovementType.ConstantSpeed:
                    return SkyfallerDrawPosUtility.DrawPos_ConstantSpeed(base.DrawPos, ticksToImpactPrediction, this.angle, currentSpeed);
                case SkyfallerMovementType.Decelerate:
                    return SkyfallerDrawPosUtility.DrawPos_Decelerate(base.DrawPos, ticksToImpactPrediction, this.angle, currentSpeed);
                default:
                    Log.ErrorOnce("SkyfallerMovementType not handled: " + this.def.skyfaller.movementType, this.thingIDNumber ^ 0x7424EBC7);
                    return SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, ticksToImpactPrediction, this.angle, currentSpeed);
            }
        }
    }
}
