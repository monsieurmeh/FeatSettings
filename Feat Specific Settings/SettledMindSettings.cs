using Il2Cpp;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class SettledMindSettings : FeatSpecificSettings<Feat_SettledMind>
    {
        public SettledMindSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }

        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_ReadSpeedBoostPercent = PercentIncrease;
        }

        public override string FeatName { get { return "SettledMind"; } }
        public override bool Vanilla { get { return true; } }
        public override Feat_SettledMind GetFeat() => FeatsManager.m_Feat_SettledMind;

        [Section("GAMEPLAY_Feat_SettledMind", Localize = true)]
        [Name("FEATSETTINGS_SettledMind_PercentIncrease_Name", Localize = true)]
        [Slider(5, 100, 20)]
        [Description("FEATSETTINGS_SettledMind_PercentIncrease_Desc")]
        public int PercentIncrease = 20;

        //Hinterland buried the toggle for disabling cabin fever with this perk in the middle of a gigantic update method, so this isn't going to be a thing for a long time. Sorry :/
        //[Name("Enable Cabin Fever")]
        //[Description("Enables or disables Cabin Fever while using the feat Settled Mind.\nDefault: false")]
        //public bool EnableCabinFever = false;
    }
}
