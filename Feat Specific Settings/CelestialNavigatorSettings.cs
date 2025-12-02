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


        [Section("Celestial Navigator")]
        [Name("Walking Speed Increase")]
        [Slider(5, 100, 20)]
        [Description("Sets the percent increase to walking speed during auroras or on clear nights while using Celestial Navigator.\nDefault: 10%")]
        public int WalkingSpeedBoostPercent = 10;
    }
}
