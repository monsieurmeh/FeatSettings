using Il2Cpp;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class BookSmartsSettings : FeatSpecificSettings<Feat_BookSmarts>
    {
        public BookSmartsSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }

        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_NumHoursRequired = HoursRequired;
            mFeat.m_PercentBenefit = PercentIncrease;
        }



        public override string FeatName { get { return "BookSmarts"; } }
        public override bool Vanilla { get { return true; } }
        public override Feat_BookSmarts GetFeat() => FeatsManager.m_Feat_BookSmarts;


        [Section("GAMEPLAY_FeatBookSmarts", Localize = true)]
        [Name("FEATSETTINGS_BookSmarts_HoursRequired_Name", Localize = true)]
        [Slider(25, 2500, 100)]
        [Description("FEATSETTINGS_BookSmarts_HoursRequired_Desc")]
        public int HoursRequired = 1000;


        [Name("FEATSETTINGS_BookSmarts_PercentIncrease_Name", Localize = true)]
        [Slider(5, 100, 20)]
        [Description("FEATSETTINGS_BookSmarts_PercentIncrease_Desc")]
        public int PercentIncrease = 10;
    }
}
