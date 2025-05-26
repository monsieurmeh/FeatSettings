using HarmonyLib;
using Il2Cpp;
using UnityEngine;
using Il2CppTLD.AI;


namespace FeatSettings
{
    internal class Patches
    {
        [HarmonyPatch(typeof(FeatsManager), nameof(FeatsManager.InstantiateFeatPrefab), new Type[] { typeof(GameObject) })]
        private static class FeatsManagerPatches_InstantiateFeatPrefab
        {
            private static void Postfix(GameObject __result)
            {
                FeatSettingsManager.Instance.RegisterFeat(__result);
            }
        }


        [HarmonyPatch(typeof(CougarManager), nameof(CougarManager.OnCougarDeath), new Type[] { typeof(SpawnRegion) })]
        private static class CougarManagerPatches_OnCougarDeath
        {
            private static void Postfix(SpawnRegion spawnRegion)
            {
                if (spawnRegion == null)
                {
                    return;
                }
                if (!FeatSettingsManager.Instance.TryGetFeatSpecificSettings<Feat_MasterHunter>(out FeatSpecificSettings<Feat_MasterHunter>? settings))
                {
                    return;
                }
                if (settings is not MasterHunterSettings masterHunterSettings)
                {
                    return;
                }
                FeatSettingsManager.Instance.Log($"Cougar killed, increasing Big Cat Killer progress! Current: {masterHunterSettings.Feat?.GetNormalizedProgress()} | after: {(float)(masterHunterSettings.Feat.GetNormalizedProgress() + (1f / masterHunterSettings.KillCountRequirement))}");
                masterHunterSettings.Feat?.SetNormalizedProgress((float)(masterHunterSettings.Feat.GetNormalizedProgress() + (1f / masterHunterSettings.KillCountRequirement)));
            }
        }
    }
}
