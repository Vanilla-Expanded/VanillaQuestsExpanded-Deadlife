using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Unix.Native;
using RimWorld;
using Verse;
using Verse.Sound;
using static HarmonyLib.Code;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace VanillaQuestsExpandedDeadlife
{
    public class DeadlifeGasSpewer : Building
    {
        protected Sustainer gasSustainer;


        public override void Tick()
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
