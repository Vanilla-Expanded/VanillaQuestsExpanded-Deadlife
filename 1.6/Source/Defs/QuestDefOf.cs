using RimWorld;
using Verse;

namespace VanillaQuestsExpandedDeadlife
{
    [DefOf]
    public static class QuestDefOf
    {
        public static SitePartDef VQE_AncientICBMLaunchSite;

        static QuestDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(QuestDefOf));
        }
    }
}
