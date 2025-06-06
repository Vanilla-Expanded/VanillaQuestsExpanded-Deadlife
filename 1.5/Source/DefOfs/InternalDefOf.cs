using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using VFECore;
using Verse.AI;

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
		public static QuestChainDef VQE_DeadlifeQuestChain;
		public static SoundDef VQED_SiloKeyboardClicking;
		public static ThingDef VQED_ICBMLaunchTerminal;
		public static SoundDef VQED_ICBMLaunchSiren;
		public static ThingDef VQED_ActiveTerminal;
		public static ThingDef VQED_SealedDeathPit;
		public static ThingDef VQED_ClosedChute;
		public static ThingDef VQED_ClosedDeadlifeCasket;
		public static ThingDef VQED_DormantMilitaryTurret;
		public static ThingDef VQED_ClosedRotstinkVent;
		public static ThingDef VQED_AncientKennel;
		public static JobDef VQE_WorkOnTerminal;
		public static DutyDef VQE_GeneralAI;
		public static ThingDef VQED_LoadedICBMSilo;
		public static SoundDef VQED_ICBMLaunch;
		public static ThingDef VQED_ICBMSkyfaller;
		public static SoundDef VQED_ICBMExplosionDistant;
		public static FleckDef VQED_Smoke;
		public static ThingDef VQED_DistantICBMExplosion;
		public static HediffDef VQED_General;
		public static FleckDef VQED_ICBMExplosionFlash;
		public static GameConditionDef VQED_ShamblerApocalypse;
		public static GameConditionDef DeathPall;
		public static WorldObjectDef VQED_ShamblerHorde;
		public static IncidentDef ShamblerAssault;
		public static IncidentDef SmallShamblerSwarm;
		public static IncidentDef ShamblerSwarmAnimals;
		public static IncidentDef ShamblerSwarm;
    }
}
