using Il2Cpp;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class EfficientMachineSettings : FeatSpecificSettings<Feat_EfficientMachine>
    {
        public EfficientMachineSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }

        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_CalorieReductionBenefit = CalorieReductionBenefit;
            mFeat.m_NumDaysRequired = NumDaysRequired;
        }

        public override string FeatName { get { return "EfficientMachine"; } }
        public override bool Vanilla { get { return true; } }
        public override Feat_EfficientMachine GetFeat() => FeatsManager.m_Feat_EfficientMachine;


        [Section("GAMEPLAY_FeatEfficientMachine", Localize = true)]
        [Name("FEATSETTINGS_EfficientMachine_NumDaysRequired_Name", Localize = true)]
        [Slider(10, 1000, 100)]
        [Description("FEATSETTINGS_EfficientMachine_NumDaysRequired_Desc")]
        public int NumDaysRequired = 500;


        [Name("FEATSETTINGS_EfficientMachine_CalorieReductionBenefit_Name", Localize = true)]
        [Slider(1, 100, 100)]
        [Description("FEATSETTINGS_EfficientMachine_CalorieReductionBenefit_Desc")]
        public int CalorieReductionBenefit = 10;
    }
}
