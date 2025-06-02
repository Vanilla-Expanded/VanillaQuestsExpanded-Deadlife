using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaQuestsExpandedDeadlife
{
	[DefOf]
	public static class InternalDefOf
	{
		static InternalDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
		}

		public static SoundDef VQED_VentillationSustainer;

        public static ThingDef VQED_PawnFlyer_Stun;
		public static ThingDef VQED_EmptyEerieSarcophagus;
		public static ThingDef VQED_AncientKinoScreen;
		public static ThingDef VQED_AncientKinoProjector;
        public static ThingDef VQED_LockedSiloHatch;
        public static ThingDef VQED_BuriedDeathPit;

        public static SitePartDef VQE_AncientSilo;
		[DefAlias("VQE_AncientSilo")] public static BiomeDef VQE_AncientSiloBiome;		
		
		public static PawnKindDef VQE_MilitaryShambler;

		public static GameConditionDef VQE_AncientSiloUnderground;
		
		public static QuestScriptDef VQE_Deadlife_AncientSilo;

		public static SitePartDef VQE_AncientICBMLaunchSite;
	}
}
