using Il2Cpp;
using MelonLoader;
using Il2CppTLD.Stats;
using UnityEngine;


namespace FeatSettings
{
    public abstract class CustomFeatDefinition
    {
        public abstract FeatType FeatType { get; }
        public abstract string Name { get; }
        public abstract string DisplayName { get; }
        public abstract string Description { get; }
        public abstract string ShortDescription { get; }
        public abstract string BonusDescription { get; }
        public abstract FeatType BadgeFeat { get; }
        public virtual Texture2D? UnlockedBadgeTexture { get { return null; } }
        public virtual Texture2D? LockedBadgeTexture { get { return null; } }
        public virtual CustomFeatSettings? Settings { get { return null; } }

        public CustomFeat? Feat { get; internal set; }
        public float Progress { get { return FeatSettingsManager.Instance.GetCustomFeatProgress(this); } set { FeatSettingsManager.Instance.SetCustomFeatProgress(this, value); } }
        public bool IsUnlockedAndEnabled { get { return Feat != null && Feat.IsUnlockedAndEnabled(); } }

        public virtual void OnStatIncremented(StatID statID, float increment) { }
        public virtual void ApplySettings() { }
        public virtual string FormatText(string text) { return text; }
        public virtual void OnSave() { }
        public virtual void OnLoad() { }

        public void Save(string key, string value) { FeatSettingsManager.Instance.SaveCustomFeatData(this, key, value); }
        public string? Load(string key) { return FeatSettingsManager.Instance.LoadCustomFeatData(this, key); }
    }


    public class CustomFeat : Feat
    {
        public CustomFeat(IntPtr ptr) : base(ptr) { }

        public override bool IsUnlocked()
        {
            return FeatSettingsManager.Instance.TryGetCustomFeatDefinition(m_FeatType, out CustomFeatDefinition? definition) && definition.Progress >= 1.0f;
        }

        public override float GetNormalizedProgress()
        {
            return FeatSettingsManager.Instance.TryGetCustomFeatDefinition(m_FeatType, out CustomFeatDefinition? definition) ? Math.Clamp(definition.Progress, 0.0f, 1.0f) : 0.0f;
        }

        public override void SetNormalizedProgress(float normalizedProgress)
        {
            if (!FeatSettingsManager.Instance.TryGetCustomFeatDefinition(m_FeatType, out CustomFeatDefinition? definition))
            {
                return;
            }
            definition.Progress = normalizedProgress;
        }

        public override string GetDescription()
        {
            return GetText(m_LocalizedDescription);
        }

        public override string GetShortDescription()
        {
            return GetText(m_LocalizedShortDescription);
        }

        public override string GetBonusDescription()
        {
            return GetText(m_LocalizedBonusDescription);
        }

        private string GetText(LocalizedString? localizedString)
        {
            string text = localizedString == null ? string.Empty : Localization.Get(localizedString.m_LocalizationID);
            return FeatSettingsManager.Instance.TryGetCustomFeatDefinition(m_FeatType, out CustomFeatDefinition? definition) ? definition.FormatText(text) : text;
        }
    }
}
