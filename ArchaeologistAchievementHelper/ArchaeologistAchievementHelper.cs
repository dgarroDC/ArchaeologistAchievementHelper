using OWML.ModHelper;
using OWML.Common;

namespace ArchaeologistAchievementHelper
{
    public class ArchaeologistAchievementHelper: ModBehaviour
    {
        private static bool _enabled;
        private static bool _showMissingFacts;

        private void Start()
        {
            ModHelper.HarmonyHelper.AddPostfix<ShipLogEntry>("HasMoreToExplore", typeof(ArchaeologistAchievementHelper), nameof(ArchaeologistAchievementHelper.ShipLogEntryHasMoreToExplore));
            ModHelper.HarmonyHelper.AddPostfix<ShipLogEntryDescriptionField>("SetEntry", typeof(ArchaeologistAchievementHelper), nameof(ArchaeologistAchievementHelper.ShipLogEntryDescriptionFieldSetEntry));
        }

        public override void Configure(IModConfig config)
        {
            _enabled = config.Enabled;
            _showMissingFacts = config.GetSettingsValue<bool>("Show missing facts (WARNING: SPOILERS)");
        }

        private static void ShipLogEntryHasMoreToExplore(ShipLogEntry __instance, ref bool __result)
        {
            if (_enabled && !__result && __instance.GetState() == ShipLogEntry.State.Explored)
            {
                foreach (ShipLogFact fact in __instance.GetExploreFacts())
                {
                    if (IsMissingForArcheologhistAchievement(__instance, fact))
                    {
                        __result = true;
                    }
                }
            }
        }

        private static void ShipLogEntryDescriptionFieldSetEntry(ShipLogEntryDescriptionField __instance, ShipLogEntry entry)
        {
            if (_enabled && _showMissingFacts && entry.GetState() == ShipLogEntry.State.Explored)
            {
                foreach (ShipLogFact fact in entry.GetExploreFacts())
                {
                    if (IsMissingForArcheologhistAchievement(entry, fact))
                    {
                        // There are enough items (10), entries have up to 6 explore facts, so we need max 7 items
                        __instance._factListItems[__instance._displayCount].DisplayText($"<color=orange>MISSING FACT: {fact.GetText()}</color>");
                        __instance._displayCount++;
                    }
                }
            }
        }

        // This is based on ShipLogManager.CheckForCompletionAchievement()
        private static bool IsMissingForArcheologhistAchievement(ShipLogEntry entry, ShipLogFact fact)
        {
            return !fact.IsRumor() && !fact.IsRevealed() &&
                !fact.GetID().Equals("TH_VILLAGE_X3") && !fact.GetID().Equals("GD_GABBRO_ISLAND_X1") &&
                entry.GetCuriosityName() != CuriosityName.InvisiblePlanet;
        }
    }
}
