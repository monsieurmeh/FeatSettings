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


        [Section("Efficient Machine")]
        [Name("Days Survived Required")]
        [Slider(10, 1000, 100)]
        [Description("Sets required number of days survived to unlock Efficient Machine.\nDefault: 500")]
        public int NumDaysRequired = 500;


        [Name("Calorie Reduction Benefit")]
        [Slider(1, 100, 100)]
        [Description("Sets the percent decrease in calorie usage while using Efficient Machine.\nDefault: 10%")]
        public int CalorieReductionBenefit = 10;
    }
}
