using Il2Cpp;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class FireMasterSettings : FeatSpecificSettings<Feat_FireMaster>
    {
        public FireMasterSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }

        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_NumFiresRequired = FiresRequired;
            mFeat.m_DefaultFireStartingSkillLevel = StartingLevel;
        }

        public override string FeatName { get { return "FireMaster"; } }
        public override bool Vanilla { get { return true; } }
        public override Feat_FireMaster GetFeat() => FeatsManager.m_Feat_FireMaster;


        [Section("GAMEPLAY_Feat_FireMaster", Localize = true)]
        [Name("FEATSETTINGS_FireMaster_FiresRequired_Name", Localize = true)]
        [Slider(100, 10000, 100)]
        [Description("FEATSETTINGS_FireMaster_FiresRequired_Desc")]
        public int FiresRequired = 1000;


        [Name("FEATSETTINGS_FireMaster_StartingLevel_Name", Localize = true)]
        [Slider(2, 5)]
        [Description("FEATSETTINGS_FireMaster_StartingLevel_Desc")]
        public int StartingLevel = 3;
    }
}
