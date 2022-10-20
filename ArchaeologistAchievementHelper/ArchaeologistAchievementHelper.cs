using OWML.ModHelper;
using OWML.Common;

namespace ArchaeologistAchievementHelper;

public class ArchaeologistAchievementHelper: ModBehaviour
{
    private static bool _showMissingFacts;

    private void Start()
    {
        ModHelper.HarmonyHelper.AddPostfix<ShipLogEntry>("HasMoreToExplore", typeof(ArchaeologistAchievementHelper), nameof(ArchaeologistAchievementHelper.ShipLogEntryHasMoreToExplore));
        ModHelper.HarmonyHelper.AddPostfix<ShipLogEntryDescriptionField>("SetEntry", typeof(ArchaeologistAchievementHelper), nameof(ArchaeologistAchievementHelper.ShipLogEntryDescriptionFieldSetEntry));
    }

    public override void Configure(IModConfig config)
    {
        _showMissingFacts = config.GetSettingsValue<bool>("Show missing facts (WARNING: SPOILERS)");
    }

    private static void ShipLogEntryHasMoreToExplore(ShipLogEntry __instance, ref bool __result)
    {
        if (!__result && __instance.GetState() == ShipLogEntry.State.Explored)
        {
            if (HasMissingFactForArchaeologistAchievement(__instance))
            {
                __result = true;
                return;
            }
            foreach (ShipLogEntry childEntry in __instance.GetChildren())
            {
                if (childEntry.GetState() == ShipLogEntry.State.Hidden && HasMissingFactForArchaeologistAchievement(childEntry))
                {
                    __result = true;
                    return;
                }
            }
        }
    }

    private static void ShipLogEntryDescriptionFieldSetEntry(ShipLogEntryDescriptionField __instance, ShipLogEntry entry)
    {
        if (_showMissingFacts && entry.GetState() == ShipLogEntry.State.Explored)
        {
            foreach (ShipLogFact fact in entry.GetExploreFacts())
            {
                if (IsMissingForArchaeologistAchievement(entry, fact))
                {
                    DisplayMissingInfo(__instance, Texts.GetTranslated(Texts.Text.MissingFact) + ": " + fact.GetText());
                }
            }
            foreach (ShipLogEntry childEntry in entry.GetChildren())
            {
                if (childEntry.GetState() == ShipLogEntry.State.Hidden && HasMissingFactForArchaeologistAchievement(childEntry))
                {
                    DisplayMissingInfo(__instance, Texts.GetTranslated(Texts.Text.MissingSubentrie) + ": " + childEntry._name);
                }
            }
        }
    }

    private static void DisplayMissingInfo(ShipLogEntryDescriptionField descriptionField, string info)
    {
        // There are enough items (10), entries have up to 7 explore facts + childres entries, so we need max 8 items
        descriptionField._factListItems[descriptionField._displayCount].DisplayText($"<color=orange>{info}</color>");
        descriptionField._displayCount++;
    }

    private static bool HasMissingFactForArchaeologistAchievement(ShipLogEntry entry)
    {
        foreach (ShipLogFact fact in entry.GetExploreFacts())
        {
            if (IsMissingForArchaeologistAchievement(entry, fact))
            {
                return true;
            }
        }

        return false;
    }

    // This is based on ShipLogManager.CheckForCompletionAchievement()
    private static bool IsMissingForArchaeologistAchievement(ShipLogEntry entry, ShipLogFact fact)
    {
        return !fact.IsRumor() && !fact.IsRevealed() &&
               !fact.GetID().Equals("TH_VILLAGE_X3") && !fact.GetID().Equals("GD_GABBRO_ISLAND_X1") &&
               entry.GetCuriosityName() != CuriosityName.InvisiblePlanet;
    }
}
