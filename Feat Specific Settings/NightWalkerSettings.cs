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
        public override bool Vanilla { get { return true; } }
        public override Feat_NightWalker GetFeat() => FeatsManager.m_Feat_NightWalker;


        [Section("GAMEPLAY_Feat_NightWalker", Localize = true)]
        [Name("FEATSETTINGS_NightWalker_DayFatigueScale_Name", Localize = true)]
        [Slider(10, 1000, 100)]
        [Description("FEATSETTINGS_NightWalker_DayFatigueScale_Desc")]
        public int DayFatigueScale = 250;


        [Name("FEATSETTINGS_NightWalker_NightFatigueScale_Name", Localize = true)]
        [Slider(10, 1000, 100)]
        [Description("FEATSETTINGS_NightWalker_NightFatigueScale_Desc")]
        public int NightFatigueScale = 50;
    }
}
