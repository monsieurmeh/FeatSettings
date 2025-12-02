using Il2Cpp;
using Il2CppTLD.Gear;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class ColdFusionSettings : FeatSpecificSettings<Feat_ColdFusion>
    {
        public ColdFusionSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }


        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_NumDaysRequired = NumDaysRequired;
            mFeat.m_TemperatureCelsiusBenefit = TemperatureCelsiusBenefit;
        }

        public override string FeatName { get { return "ColdFusion"; } }
        public override bool Vanilla { get { return true; } }


        [Section("Cold Fusion")]
        [Name("Days Spent Outside Requirement")]
        [Slider(10, 1000, 100)]
        [Description("Sets required number of days spend outside to unlock Cold Fusion.\nDefault: 100")]
        public int NumDaysRequired = 100;


        [Name("Temperature Benefit (Celsius)")]
        [Slider(1, 10, 10)]
        [Description("Sets the player temperature increase in degrees Celsius while using Cold Fusion.\nDefault: 2")]
        public int TemperatureCelsiusBenefit = 2;
    }
}
