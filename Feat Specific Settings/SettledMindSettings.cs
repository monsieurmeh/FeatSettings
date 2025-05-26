using Il2Cpp;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class SettledMindSettings : FeatSpecificSettings<Feat_SettledMind>
    {
        public SettledMindSettings(FeatSettingsManager manager) : base(manager) { }

        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_ReadSpeedBoostPercent = PercentIncrease;
        }

        [Section("Settled Mind")]
        [Name("Reading Speed Increase")]
        [Slider(5, 100, 20)]
        [Description("Sets the percent increase to reading speed while using Settled Mind.\nDefault: 20")]
        public int PercentIncrease = 20;

        //Hinterland buried the toggle for disabling cabin fever with this perk in the middle of a gigantic update method, so this isn't going to be a thing for a long time. Sorry :/
        //[Name("Enable Cabin Fever")]
        //[Description("Enables or disables Cabin Fever while using the feat Settled Mind.\nDefault: false")]
        //public bool EnableCabinFever = false;
    }
}
