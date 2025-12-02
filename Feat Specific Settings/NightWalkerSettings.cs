using Il2Cpp;
using Il2CppTLD.Gear;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class NightWalkerSettings : FeatSpecificSettings<Feat_NightWalker>
    {
        public NightWalkerSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }


        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_FatigueScaleDayMultiplier = DayFatigueScale * 0.01f;
            mFeat.m_FatigueScaleNightMultiplier = NightFatigueScale * 0.01f;
        }

        public override string FeatName { get { return "NightWalker"; } }


        [Section("Dark Walker")]
        [Name("Daytime Fatigue Scale Multiplier")]
        [Slider(10, 1000, 100)]
        [Description("Sets the daytime fatigue scale multiplier when using Dark Walker.\nDefault: 200%")]
        public int DayFatigueScale = 250;


        [Name("Nighttime Fatigue Scale Multiplier")]
        [Slider(10, 1000, 100)]
        [Description("Sets the nighttime fatigue scale multiplier when using Dark Walker.\nDefault: 50%")]
        public int NightFatigueScale = 50;
    }
}
