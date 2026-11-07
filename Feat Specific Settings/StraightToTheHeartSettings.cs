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


        [Section("GAMEPLAY_FeatStraightToTheHeart", Localize = true)]
        [Name("FEATSETTINGS_StraightToTheHeart_ConsumablesRequired_Name", Localize = true)]
        [Slider(25, 2500, 100)]
        [Description("FEATSETTINGS_StraightToTheHeart_ConsumablesRequired_Desc")]
        public int ConsumablesRequired = 250;


        [Name("FEATSETTINGS_StraightToTheHeart_DurationIncrease_Name", Localize = true)]
        [Slider(5, 100, 20)]
        [Description("FEATSETTINGS_StraightToTheHeart_DurationIncrease_Desc")]
        public int DurationIncrease = 25;
    }
}
