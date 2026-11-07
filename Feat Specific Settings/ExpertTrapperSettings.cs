using Il2Cpp;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class ExpertTrapperSettings : FeatSpecificSettings<Feat_ExpertTrapper>
    {
        public ExpertTrapperSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }

        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_NumSnaredRabbitsRequired = NumSnaredRabbitsRequired;
            mFeat.m_ChanceIncreaseToCatchRabbitsPercent = ChanceIncreaseToCatchRabbitsPercent;
        }

        public override string FeatName { get { return "ExpertTrapper"; } }
        public override bool Vanilla { get { return true; } }
        public override Feat_ExpertTrapper GetFeat() => FeatsManager.m_Feat_ExpertTrapper;


        [Section("GAMEPLAY_FeatExpertTrapper", Localize = true)]
        [Name("FEATSETTINGS_ExpertTrapper_NumSnaredRabbitsRequired_Name", Localize = true)]
        [Slider(5, 100, 20)]
        [Description("FEATSETTINGS_ExpertTrapper_NumSnaredRabbitsRequired_Desc")]
        public int NumSnaredRabbitsRequired = 20;


        [Name("FEATSETTINGS_ExpertTrapper_ChanceIncreaseToCatchRabbitsPercent_Name", Localize = true)]
        [Slider(5, 100, 20)]
        [Description("FEATSETTINGS_ExpertTrapper_ChanceIncreaseToCatchRabbitsPercent_Desc")]
        public int ChanceIncreaseToCatchRabbitsPercent = 25;
    }
}
