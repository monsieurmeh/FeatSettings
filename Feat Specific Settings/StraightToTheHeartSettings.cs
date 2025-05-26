using Il2Cpp;
using Il2CppTLD.Gear;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class StraightToTheHeartSettings : FeatSpecificSettings<Feat_StraightToHeart>
    {
        public StraightToTheHeartSettings(FeatSettingsManager manager) : base(manager) { }


        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_ItemConsumedCountRequired = ConsumablesRequired;
            mFeat.m_EffectiveLengthIncreasePercent = DurationIncrease;
        }


        [Section("Straight To The Heart")]
        [Name("Consumable Count Requirement")]
        [Slider(25, 2500, 100)]
        [Description("Sets required number of consumables to unlock Straight to the Heart.\nDefault: 250")]
        public int ConsumablesRequired = 250;


        [Name("Duration Increase")]
        [Slider(5, 500, 100)]
        [Description("Sets the duration increase percent of the feat.\nDefault: 25%")]
        public int DurationIncrease = 25;
    }
}
