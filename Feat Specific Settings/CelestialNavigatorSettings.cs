using Il2Cpp;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class CelestialNavigatorSettings : FeatSpecificSettings<Feat_CelestialNavigator>
    {
        public CelestialNavigatorSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }

        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_WalkingSpeedBoostPercent = WalkingSpeedBoostPercent;
        }

        public override string FeatName { get { return "CelestialNavigator"; } }
        public override bool Vanilla { get { return true; } }
        public override Feat_CelestialNavigator GetFeat() => FeatsManager.m_Feat_CelestialNavigator;


        [Section("GAMEPLAY_Feat_CelestialNavigator", Localize = true)]
        [Name("FEATSETTINGS_CelestialNavigator_WalkingSpeedBoostPercent_Name", Localize = true)]
        [Slider(5, 100, 20)]
        [Description("FEATSETTINGS_CelestialNavigator_WalkingSpeedBoostPercent_Desc")]
        public int WalkingSpeedBoostPercent = 10;
    }
}
