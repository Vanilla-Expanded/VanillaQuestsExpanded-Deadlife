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

	

		public static ThingDef VQED_PawnFlyer_Stun;
		public static ThingDef VQED_EmptyEerieSarcophagus;


		public static ThingDef VQED_AncientKinoScreen;
		public static ThingDef VQED_AncientKinoProjector;

		public static SitePartDef VQE_AncientSilo;

		public static ThingDef VQED_LockedSiloHatch;
	}
}
