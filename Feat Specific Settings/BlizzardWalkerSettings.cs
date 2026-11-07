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
        public override Feat_BlizzardWalker GetFeat() => FeatsManager.m_Feat_BlizzardWalker;


        [Section("GAMEPLAY_FeatBlizzardWalker", Localize = true)]
        [Name("FEATSETTINGS_BlizzardWalker_DaysRequired_Name", Localize = true)]
        [Slider(5, 100, 20)]
        [Description("FEATSETTINGS_BlizzardWalker_DaysRequired_Desc")]
        public int DaysRequired = 20;


        [Name("FEATSETTINGS_BlizzardWalker_PenaltyReductionPercent_Name", Localize = true)]
        [Slider(5, 100, 20)]
        [Description("FEATSETTINGS_BlizzardWalker_PenaltyReductionPercent_Desc")]
        public int PenaltyReductionPercent = 25;
    }
}
