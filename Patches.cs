using HarmonyLib;
using Il2Cpp;
using UnityEngine;
using Il2CppTLD.AI;
using Il2CppNodeCanvas.Tasks.Actions;
using MelonLoader;
using Il2CppTLD.Stats;
using UnityEngine.SceneManagement;
using ExpandedAiFramework;
using UnityEngine.AddressableAssets;
using Il2CppTLD.Gameplay;


namespace FeatSettings
{
    internal class Patches
    {
        [HarmonyPatch(typeof(FeatsManager), nameof(FeatsManager.Deserialize), new Type[] { typeof(string) })]
        private static class FeatsManagerPatches_Deserialize
        {
            private static void Prefix(string text, FeatsManager __instance)
            {
                if (!Utils.TryGetField(text, "FeatSettingsData", out string data)) return;
                MelonLogger.Msg(data);
                FeatSettingsManager.Instance.Deserialize(data);
            }
        }

        [HarmonyPatch(typeof(FeatsManager), nameof(FeatsManager.Serialize))]
        private static class FeatsManagerPatches_Serialize
        {
            private static void Postfix(ref string __result)
            {
                __result = Utils.AddOrUpdateField(__result, "FeatSettingsData", FeatSettingsManager.Instance.Serialize());
            }
        }

        [HarmonyPatch(typeof(FeatsManager), nameof(FeatsManager.InstantiateFeatPrefab), new Type[] { typeof(GameObject) })]
        private static class FeatsManagerPatches_InstantiateFeatPrefab
        {
            private static void Postfix(GameObject __result)
            {
                FeatSettingsManager.Instance.RegisterFeat(__result);
            }
        }


        [HarmonyPatch(typeof(BaseAi), nameof(BaseAi.EnterDead))]
        private static class BaseAiPatches_EnterDead
        {
            private static void Prefix(BaseAi __instance)
            {
                if (__instance == null) return;
                if (__instance.Cougar == null) return;
                if (__instance.m_ForceToCorpse) return;
                if (!FeatSettingsManager.Instance.TryGetFeatSpecificSettings<Feat_MasterHunter>(out FeatSpecificSettings<Feat_MasterHunter>? settings)) return;
                if (settings is not MasterHunterSettings masterHunterSettings) return;

                int cougarsKilled = FeatSettingsManager.Instance.Data.CougarsKilled;
                int cougarsNeeded = masterHunterSettings.KillCountRequirement;
                FeatSettingsManager.Instance.Log($"Cougar killed, increasing Big Cat Killer progress! Current: {Math.Min(cougarsKilled, cougarsNeeded)} of {cougarsNeeded} | after: {Math.Min(cougarsKilled + 1, cougarsNeeded)} of {cougarsNeeded}");
                FeatSettingsManager.Instance.Data.CougarsKilled++;
                masterHunterSettings.MaybeUnlock();
                SaveGameSystem.SaveProfile();
            }
        }

        [HarmonyPatch(typeof(Feat), nameof(Feat.TryToDisplayKicker))]
        private static class FeatPatches_TryToDisplayKicker
        {
            private static bool Prefix()
            {
                FeatSettingsManager.Instance.Log($"Attempting to stop kicker!");
                return !SceneUtilities.IsSceneMenu(SceneUtilities.GetActiveSceneName());
            }
        }


        [HarmonyPatch(typeof(FeatNotify), nameof(FeatNotify.ShowFeatUnlockedKicker), new Type[] { typeof(AssetReferenceTexture2D), typeof(string) })]
        private static class FeatNotifyPatches_ShowFeatUnlockedKicker
        {
            private static bool Prefix(AssetReferenceTexture2D textureReference, string footer)
            {
                FeatSettingsManager.Instance.Log($"Attempting to stop kicker display!");
                return !SceneUtilities.IsSceneMenu(SceneUtilities.GetActiveSceneName());
            }
        }

        [HarmonyPatch(typeof(ConsoleManager), nameof(ConsoleManager.Initialize))]
        internal class ConsoleManagerPatches_Initialize
        {
            private static void Postfix()
            {
                uConsole.RegisterCommand("SetCougarKills", new Action(() => SetCougarKills()));
                uConsole.RegisterCommand("EnableFeat", new Action(() => EnableFeat()));
                uConsole.RegisterCommand("DisableFeat", new Action(() => SetCougarKills()));
            }
        }

        private static void SetCougarKills()
        {
            string command = uConsole.GetString();
            if (command == null || command.Length == 0)
            {
                FeatSettingsManager.Instance.Log($"Enter kill quantity!");
                return;
            }
            if (!int.TryParse(command, out int value))
            {
                FeatSettingsManager.Instance.Log($"Enter kill quantity as integer!");
                return;
            }
            FeatSettingsManager.Instance.Data.CougarsKilled = value;
            if (!FeatSettingsManager.Instance.TryGetFeatSpecificSettings<Feat_MasterHunter>(out FeatSpecificSettings<Feat_MasterHunter>? settings)) return;
            if (settings is not MasterHunterSettings masterHunterSettings) return;
            masterHunterSettings.MaybeUnlock();
        }

        private static void EnableFeat()
        {
            string featName = uConsole.GetString();
            if (featName == null || featName.Length == 0)
            {
                FeatSettingsManager.Instance.Log($"Enter feat name!");
                return;
            }
            if (FeatSettingsManager.Instance.TryGetFeatSpecificSettingsByName(featName, out FeatSpecificSettingsBase settings))
            {
                FeatSettingsManager.Instance.Log($"Invalid feat name!");
                return;
            }
            if (!settings.BaseFeat.IsUnlocked())
            {
                FeatSettingsManager.Instance.Log($"You must unlock this feat first!");
                return;
            }
            if (settings.Vanilla)
            {
                if (FeatEnabledTracker.m_FeatsEnabledThisSandbox.Contains(settings.BaseFeat.m_FeatType)) return;
                FeatEnabledTracker.m_FeatsEnabledThisSandbox.Add(settings.BaseFeat.m_FeatType);
            }
            else
            {
                FeatSettingsManager.Instance.Log($"Non vanilla feats WIP!");
            }
        }
    }

    [HarmonyPatch(typeof(Panel_MainMenu), nameof(Panel_MainMenu.GetNumFeatsForXPMode))]
    internal class Panel_MainMenuPatches_GetNumFeatsForXPMode
    {
        private static void Postfix(ref int __result, Panel_MainMenu __instance)
        {
            FeatSettingsManager manager = FeatSettingsManager.Instance;
            if (__instance.m_ActiveFeatObjects == null) return;
            if (__instance.m_ActiveFeatObjects.Count == 0) return;
            if (__instance.m_ActiveFeatObjects.Count < 9)
            {
                manager.Log($"active feat count: {__instance.m_ActiveFeatObjects.Count}");
                GameObject[] newActiveFeatObjects = new GameObject[9];
                for (int i = 0, iMax = __instance.m_ActiveFeatObjects.Count; i < iMax; i++)
                {
                    manager.Log($"copying object {i}");
                    newActiveFeatObjects[i] = __instance.m_ActiveFeatObjects[i];
                    manager.Log($"copied object {i}");
                }
                for (int i = __instance.m_ActiveFeatObjects.Count, iMax = 9; i < iMax; i++)
                {
                    manager.Log($"instantiating object {i}");
                    newActiveFeatObjects[i] = GameObject.Instantiate(newActiveFeatObjects[i - 1], newActiveFeatObjects[i - 1].transform.parent);
                    manager.Log($"instantiated object {i}");
                }
                __instance.m_ActiveFeatObjects = newActiveFeatObjects;
            }
            GameModeConfig currentConfig = ExperienceModeManager.s_CurrentGameMode;
            if (currentConfig == null)
            {
                __result = 0;
                return;
            }
            ExperienceMode currentMode = currentConfig.m_XPMode;
            if (currentMode == null)
            {
                __result = 0;
                return;
            }
            switch (currentMode.m_ModeType)
            {
                case ExperienceModeType.Pilgrim: __result = manager.Settings.PilgrimFeatCount; break;
                case ExperienceModeType.Voyageur: __result = manager.Settings.VoyagerFeatCount; break;
                case ExperienceModeType.Stalker: __result = manager.Settings.StalkerFeatCount; break;
                case ExperienceModeType.Interloper: __result = manager.Settings.InterloperFeatCount; break;
                case ExperienceModeType.Misery: __result = manager.Settings.MiseryFeatCount; break;
                case ExperienceModeType.Custom: __result = manager.Settings.CustomFeatCount; break;
                default: __result = 0; break;
            }
        }
    }


    [HarmonyPatch(typeof(FeatEnabledTracker), nameof(FeatEnabledTracker.Deserialize))]
    internal class FeatEnabledTrackerPatches_Deserialize
    {
        private static void Prefix(string text)
        {
            FeatSettingsManager.Instance.Log(text);
        }
    }


    [HarmonyPatch(typeof(FeatEnabledTracker), nameof(FeatEnabledTracker.Serialize))]
    internal class FeatEnabledTrackerPatches_Serialize
    {
        private static void Postfix(ref string __result)
        {
            FeatSettingsManager.Instance.Log(__result);
        }
    }
}
