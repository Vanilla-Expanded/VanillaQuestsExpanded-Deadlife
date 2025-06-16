
using RimWorld;
using Verse;
using Verse.Sound;


namespace VanillaQuestsExpandedDeadlife
{
    public class DeadlifeGasSpewer : Building
    {
        protected Sustainer gasSustainer;


        protected override void Tick()
        {
            base.Tick();
            if (this.IsHashIntervalTick(20))
            {
                GasUtility.AddDeadifeGas(Position, Map, Faction.OfEntities, 30);
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
