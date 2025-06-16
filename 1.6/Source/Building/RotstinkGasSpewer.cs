
using Verse;
using Verse.Sound;


namespace VanillaQuestsExpandedDeadlife
{
    public class RotstinkGasSpewer : Building
    {
        protected Sustainer gasSustainer;


        protected override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(20))
            {
                GasUtility.AddGas(Position, Map, GasType.RotStink, 0.2f);
            }
            if (gasSustainer == null)
            {
                StartSustainer();
            }
            else
            {
                gasSustainer.Maintain();
            }

        }

        private void StartSustainer()
        {

            SoundInfo info = SoundInfo.InMap(this, MaintenanceType.PerTick);
            gasSustainer = InternalDefOf.VQED_VentillationSustainer.TrySpawnSustainer(info);
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (gasSustainer != null)
            {
                gasSustainer.End();
                gasSustainer = null;
            }
            base.Destroy(mode);

        }



    }
}
