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
                FeatSettingsManager.Instance.LoadModData();
            }
        }

        [HarmonyPatch(typeof(FeatsManager), nameof(FeatsManager.Serialize))]
        private static class FeatsManagerPatches_Serialize
        {
            private static void Postfix(ref string __result)
            {
                FeatSettingsManager.Instance.SaveModData();
            }
        }

        [HarmonyPatch(typeof(FeatsManager), nameof(FeatsManager.Awake))]
        private static class FeatsManagerPatches_Awake
        {
            private static void Postfix(FeatsManager __instance)
            {
                FeatSettingsManager.Instance.InstantiateCustomFeats(__instance);
            }
        }

        [HarmonyPatch(typeof(FeatsManager), nameof(FeatsManager.GetFeat))]
        private static class FeatsManagerPatches_GetFeat
        {
            private static void Postfix(FeatType featType, ref Feat __result)
            {
                if (__result != null) return;
                if (!FeatSettingsManager.Instance.TryGetCustomFeatDefinition(featType, out CustomFeatDefinition? definition)) return;
                __result = definition.Feat;
            }
        }

        [HarmonyPatch(typeof(Panel_MainMenu), nameof(Panel_MainMenu.LoadFeatureIcon))]
        private static class Panel_MainMenuPatches_LoadFeatureIcon
        {
            private static bool Prefix(UITexture targetTexture, AssetReferenceTexture2D spriteReference)
            {
                if (!FeatSettingsManager.Instance.TryGetCustomBadgeTexture(spriteReference, out Texture2D? texture)) return true;
                targetTexture.mainTexture = texture;
                return false;
            }
        }

        [HarmonyPatch(typeof(Panel_Badges), nameof(Panel_Badges.SetupFeatScrollList))]
        private static class Panel_BadgesPatches_SetupFeatScrollList
        {
            private static void Postfix(Panel_Badges __instance)
            {
                FeatSettingsManager.Instance.ApplyCustomBadgeTextures(__instance.m_Feats, __instance.m_FeatsScrollList);
            }
        }

        [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.IncrementValue), new Type[] { typeof(StatID), typeof(float) })]
        private static class StatsManagerPatches_IncrementValue
        {
            private static void Postfix(StatID id, float increment)
            {
                FeatSettingsManager.Instance.OnStatIncremented(id, increment);
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

            private static void Postfix(AssetReferenceTexture2D textureReference)
            {
                if (!FeatSettingsManager.Instance.TryGetCustomBadgeTexture(textureReference, out Texture2D? texture)) return;
                Panel_HUD hud = InterfaceManager.GetPanel<Panel_HUD>();
                if (hud == null) return;
                hud.m_FeatUnlockedBadgeIcon.mainTexture = texture;
            }
        }

        [HarmonyPatch(typeof(ConsoleManager), nameof(ConsoleManager.Initialize))]
        internal class ConsoleManagerPatches_Initialize
        {
            private static void Postfix()
            {
                uConsole.RegisterCommand("SetCougarKills", new Action(() => FeatSettingsManager.Instance.Console_SetCougarKills()));
                uConsole.RegisterCommand("EnableFeat", new Action(() => FeatSettingsManager.Instance.Console_EnableFeat()));
                uConsole.RegisterCommand("DisableFeat", new Action(() => FeatSettingsManager.Instance.Console_DisableFeat()));
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
            manager.UpdateActiveFeatSlots(__instance, __result);
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
