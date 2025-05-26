using RimWorld;
using Verse;
using System.Linq;
using System.Collections.Generic;
using HarmonyLib;

namespace VanillaQuestsExpandedDeadlife
{
    public class GameCondition_AncientSiloUnderground : GameCondition_ForceWeather
    {
        public override int TransitionTicks => 0;
        public override float SkyTargetLerpFactor(Map map)
        {
            return 1f;
        }

        private bool done;
        public override void GameConditionDraw(Map map)
        {
            base.GameConditionDraw(map);
            if (done is false)
            {
                var type = AccessTools.TypeByName("RimWorld.GameConditionManager+MapBrightnessTracker");
                var tracker = AccessTools.Field(typeof(GameConditionManager), "mapBrightnessTracker").GetValue(map.gameConditionManager);
                AccessTools.Field(type, "brightness").SetValue(tracker, 0f);
                AccessTools.Field(type, "targetBrightness").SetValue(tracker, 0f);
                map.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.GroundGlow);
                done = true;
                map.glowGrid.Rebuild();
            }
        }
        

        public override SkyTarget? SkyTarget(Map map)
        {
            return new SkyTarget(0f, GameCondition_NoSunlight.EclipseSkyColors, 1f, 0f);
        }
    }
}
