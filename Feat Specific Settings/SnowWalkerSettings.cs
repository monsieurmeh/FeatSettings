using Il2Cpp;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class SnowWalkerSettings : FeatSpecificSettings<Feat_SnowWalker>
    {
        public SnowWalkerSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }

        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_NumKilometersRequired = KilometersRequired;
            mFeat.m_StaminaRechargeFasterPercent = StartingLevel;
        }

        public override string FeatName { get { return "SnowWalker"; } }
        public override bool Vanilla { get { return true; } }


        [Section("Snow Walker")]
        [Name("Number of Kilometers Required")]
        [Slider(100, 10000, 100)]
        [Description("Sets required number of kilometers traveled to unlock Snow Walker.\nDefault: 1000km")]
        public int KilometersRequired = 1000;


        [Name("Stamina Recharge Rate Increase")]
        [Slider(5, 100, 20)]
        [Description("Sets the percent increase to stamina regeneration while using Snow Walker.\nDefault: 20% ")]
        public int StartingLevel = 20;
    }
}
