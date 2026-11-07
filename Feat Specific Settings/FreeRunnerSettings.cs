using Il2Cpp;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class FreeRunnerSettings : FeatSpecificSettings<Feat_FreeRunner>
    {
        public FreeRunnerSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }

        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_NumKilometersRequired = NumKilometersRequired;
            mFeat.m_CalorieReductionBenefit = CalorieReductionBenefit;
        }

        public override string FeatName { get { return "FreeRunner"; } }
        public override bool Vanilla { get { return true; } }
        public override Feat_FreeRunner GetFeat() => FeatsManager.m_Feat_FreeRunner;


        [Section("GAMEPLAY_Feat_FreeRunner", Localize = true)]
        [Name("FEATSETTINGS_FreeRunner_NumKilometersRequired_Name", Localize = true)]
        [Slider(5, 500, 100)]
        [Description("FEATSETTINGS_FreeRunner_NumKilometersRequired_Desc")]
        public int NumKilometersRequired = 50;


        [Name("FEATSETTINGS_FreeRunner_CalorieReductionBenefit_Name", Localize = true)]
        [Slider(1, 100, 100)]
        [Description("FEATSETTINGS_FreeRunner_CalorieReductionBenefit_Desc")]
        public int CalorieReductionBenefit = 25;
    }
}
