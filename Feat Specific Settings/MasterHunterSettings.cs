using Il2Cpp;
using Il2CppTLD.Gear;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class MasterHunterSettings : FeatSpecificSettings<Feat_MasterHunter>
    {
        public MasterHunterSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }


        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_AiSoundRangeScale = Math.Clamp(100 - SoundDecreasePercent, 5, 100);
            mFeat.m_AiSightRangeScale = Math.Clamp(100 - SightDecreasePercent, 5, 100);
            mFeat.m_AiSmellRangeScale = Math.Clamp(100 - SmellDecreasePercent, 5, 100);
            MaybeUnlock();
        }

        public override string FeatName { get { return "MasterHunter"; } }
        public override bool Vanilla { get { return true; } }
        public override Feat_MasterHunter GetFeat() => FeatsManager.m_Feat_MasterHunter;


        public void MaybeUnlock()
        {
            if (mManager.Data.CougarsKilled >= KillCountRequirement)
            {
                mFeat.Unlock();
            }
            else
            {
                mFeat.m_Unlocked = false;
            }
        }




        [Section("GAMEPLAY_Feat_MasterHunter", Localize = true)]
        [Name("FEATSETTINGS_MasterHunter_KillCountRequirement_Name", Localize = true)]
        [Slider(3, 30, 10)]
        [Description("FEATSETTINGS_MasterHunter_KillCountRequirement_Desc")]
        public int KillCountRequirement = 9;


        [Name("FEATSETTINGS_MasterHunter_SoundDecreasePercent_Name", Localize = true)]
        [Slider(0, 95, 20)]
        [Description("FEATSETTINGS_MasterHunter_SoundDecreasePercent_Desc")]
        public int SoundDecreasePercent = 50;


        [Name("FEATSETTINGS_MasterHunter_SightDecreasePercent_Name", Localize = true)]
        [Slider(0, 95, 20)]
        [Description("FEATSETTINGS_MasterHunter_SightDecreasePercent_Desc")]
        public int SightDecreasePercent = 50;


        [Name("FEATSETTINGS_MasterHunter_SmellDecreasePercent_Name", Localize = true)]
        [Slider(0, 95, 20)]
        [Description("FEATSETTINGS_MasterHunter_SmellDecreasePercent_Desc")]
        public int SmellDecreasePercent = 50;
    }
}
