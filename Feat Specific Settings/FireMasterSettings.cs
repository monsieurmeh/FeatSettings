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


        [Section("Fire Master")]
        [Name("Number of Fires Required to Unlock")]
        [Slider(100, 10000, 100)]
        [Description("Sets required number of fires started to unlock Fire Master.\nDefault: 1000")]
        public int FiresRequired = 1000;


        [Name("Fire Starting Skill Level Increase")]
        [Slider(2, 5)]
        [Description("Sets the starting level of Fire Starting skill when using Fire Master in a new game.\nDefault: 3")]
        public int StartingLevel = 3;
    }
}
