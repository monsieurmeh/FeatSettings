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


        [Section("Free Runner")]
        [Name("Sprint Requirement (Kilometers)")]
        [Slider(5, 500, 100)]
        [Description("Sets required number of kilometers sprinted to unlock Free Runner.\nDefault: 50")]
        public int NumKilometersRequired = 50;


        [Name("Sprinting Calorie Reduction Benefit")]
        [Slider(1, 100, 100)]
        [Description("Sets the percent decrease to calorie consumption while spriting using Free Runner.\nDefault: 25")]
        public int CalorieReductionBenefit = 25;
    }
}
