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
                FeatSettingsManager.Instance.Log($"Cougar killed, increasing Big Cat Killer progress! Current: {(Math.Max(cougarsKilled, cougarsNeeded) / cougarsNeeded)} | after: {Math.Max(cougarsKilled + 1, cougarsNeeded)/cougarsNeeded}");
                FeatSettingsManager.Instance.Data.CougarsKilled++;
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
    }
}
