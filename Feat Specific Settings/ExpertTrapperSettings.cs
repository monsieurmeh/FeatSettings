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


        [Section("Expert Trapper")]
        [Name("Rabbits Snared Required")]
        [Slider(5, 100, 20)]
        [Description("Sets required number of rabbits snared to unlock Expert Trapper.\nDefault: 20")]
        public int NumSnaredRabbitsRequired = 20;


        [Name("Snare Success Increase Benefit")]
        [Slider(5, 100, 20)]
        [Description("Sets the percent increase to snare success chance while using Expert Trapper.\nDefault: 25%")]
        public int ChanceIncreaseToCatchRabbitsPercent = 25;
    }
}
