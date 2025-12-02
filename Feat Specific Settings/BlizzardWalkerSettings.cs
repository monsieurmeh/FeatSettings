using Il2Cpp;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class BlizzardWalkerSettings : FeatSpecificSettings<Feat_BlizzardWalker>
    {
        public BlizzardWalkerSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }

        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_BlizzardHoursOutsideRequired = DaysRequired * 24.0f;
            mFeat.m_WalkingSpeedInWindReductionPercent = PenaltyReductionPercent;
        }

        public override string FeatName { get { return "BlizzardWalker"; } }
        public override bool Vanilla { get { return true; } }


        [Section("Blizzard Walker")]
        [Name("Days Outside Required")]
        [Slider(5, 100, 20)]
        [Description("Sets required number of days spent walking through blizzards to unlock BLizzard Walker.\nDefault: 20")]
        public int DaysRequired = 20;


        [Name("Wind Speed Movement Penalty Reduction")]
        [Slider(5, 100, 20)]
        [Description("Sets the percent decrease to movement speed penalty in high winds while using Blizzard Walker.\nDefault: 25%")]
        public int PenaltyReductionPercent = 25;
    }
}
