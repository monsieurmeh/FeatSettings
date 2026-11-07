using Il2Cpp;
using Il2CppTLD.Gear;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class ColdFusionSettings : FeatSpecificSettings<Feat_ColdFusion>
    {
        public ColdFusionSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }


        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_NumDaysRequired = NumDaysRequired;
            mFeat.m_TemperatureCelsiusBenefit = TemperatureCelsiusBenefit;
        }

        public override string FeatName { get { return "ColdFusion"; } }
        public override bool Vanilla { get { return true; } }
        public override Feat_ColdFusion GetFeat() => FeatsManager.m_Feat_ColdFusion;


        [Section("GAMEPLAY_FeatColdFusion", Localize = true)]
        [Name("FEATSETTINGS_ColdFusion_NumDaysRequired_Name", Localize = true)]
        [Slider(10, 1000, 100)]
        [Description("FEATSETTINGS_ColdFusion_NumDaysRequired_Desc")]
        public int NumDaysRequired = 100;


        [Name("FEATSETTINGS_ColdFusion_TemperatureCelsiusBenefit_Name", Localize = true)]
        [Slider(1, 10, 10)]
        [Description("FEATSETTINGS_ColdFusion_TemperatureCelsiusBenefit_Desc")]
        public int TemperatureCelsiusBenefit = 2;
    }
}
