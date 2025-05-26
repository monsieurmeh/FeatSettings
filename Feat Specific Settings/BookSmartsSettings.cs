using Il2Cpp;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class BookSmartsSettings : FeatSpecificSettings<Feat_BookSmarts>
    {
        public BookSmartsSettings(FeatSettingsManager manager) : base(manager) { }

        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_NumHoursRequired = HoursRequired;
            mFeat.m_PercentBenefit = PercentIncrease;
        }


        [Section("Book Smarts")]
        [Name("Research Time Requirement")]
        [Slider(25, 2500, 100)]
        [Description("Sets required number of research hours to unlock Book Smarts.\nDefault: 250")]
        public int HoursRequired = 1000;


        [Name("Learning Benefit Incrase")]
        [Slider(10, 1000, 100)]
        [Description("Sets the percent increase to skill gain .\nDefault: 10")]
        public int PercentIncrease = 10;
    }
}
