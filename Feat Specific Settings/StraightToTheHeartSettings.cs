using Il2Cpp;
using Il2CppTLD.Gear;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class StraightToTheHeartSettings : FeatSpecificSettings<Feat_StraightToHeart>
    {
        public StraightToTheHeartSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }


        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_ItemConsumedCountRequired = ConsumablesRequired;
            mFeat.m_EffectiveLengthIncreasePercent = DurationIncrease;
        }

        public override string FeatName { get { return "StraightToHeart"; } }
        public override bool Vanilla { get { return true; } }
        public override Feat_StraightToHeart GetFeat() => GameManager.m_FeatsManager.m_Feat_StraightToHeart_Prefab.GetComponent<Feat_StraightToHeart>();


        [Section("Straight To The Heart")]
        [Name("Consumable Count Requirement")]
        [Slider(25, 2500, 100)]
        [Description("Sets required number of consumables to unlock Straight to the Heart.\nDefault: 250")]
        public int ConsumablesRequired = 250;


        [Name("Duration Increase")]
        [Slider(5, 100, 20)]
        [Description("Sets the percent duration increase of consumables while using Straight to the Heart.\nDefault: 25%")]
        public int DurationIncrease = 25;
    }
}
