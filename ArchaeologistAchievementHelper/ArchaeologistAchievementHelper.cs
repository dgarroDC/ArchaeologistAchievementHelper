using HarmonyLib;
using OWML.ModHelper;
using OWML.Common;

namespace ArchaeologistAchievementHelper;

[HarmonyPatch]
public class ArchaeologistAchievementHelper: ModBehaviour
{
    private static bool _showMissingFacts;
    private static bool _showMissingEntries;

    private void Start()
    {
        Harmony.CreateAndPatchAll(System.Reflection.Assembly.GetExecutingAssembly());
    }

    public override void Configure(IModConfig config)
    {
        _showMissingFacts = config.GetSettingsValue<bool>("Show missing facts (WARNING: SPOILERS)");
        bool prevShowMissingEntries = _showMissingEntries;
        _showMissingEntries = config.GetSettingsValue<bool>("Show missing entries (WARNING: SPOILERS)");
        if (prevShowMissingEntries && !_showMissingEntries)
        {
            // We need to hide the cards again
            ShipLogController shipLogController = FindObjectOfType<ShipLogController>();
            ShipLogDetectiveMode detectiveMode = (ShipLogDetectiveMode)shipLogController._detectiveMode;
            foreach (ShipLogEntryCard card in detectiveMode._cardList)
            {
                if (card.gameObject.activeSelf && card.GetEntry().GetState() == ShipLogEntry.State.Hidden)
                {
                    card.gameObject.SetActive(false); // UpdateStateVisuals()
                }
            }
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ShipLogEntry), nameof(ShipLogEntry.HasMoreToExplore))]
    private static void ShipLogEntry_HasMoreToExplore(ShipLogEntry __instance, ref bool __result)
    {
        if (!__result && __instance._state == ShipLogEntry.State.Explored)
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

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ShipLogEntry), nameof(ShipLogEntry.GetState))]
    private static void ShipLogEntry_GetState(ShipLogEntry __instance, ref ShipLogEntry.State __result)
    {
        if (_showMissingEntries && __result == ShipLogEntry.State.Hidden && HasMissingFactForArchaeologistAchievement(__instance))
        {
            __result = ShipLogEntry.State.Rumored;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(ShipLogEntryDescriptionField), nameof(ShipLogEntryDescriptionField.SetEntry))]
    private static void ShipLogEntryDescriptionField_SetEntry(ShipLogEntryDescriptionField __instance, ShipLogEntry entry)
    {
        if (_showMissingFacts && entry._state == ShipLogEntry.State.Explored)
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
                    DisplayMissingInfo(__instance, Texts.GetTranslated(Texts.Text.MissingSubentry) + ": " + childEntry._name);
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
