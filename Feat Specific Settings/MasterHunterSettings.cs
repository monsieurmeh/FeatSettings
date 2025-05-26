using Il2Cpp;
using Il2CppTLD.Gear;
using MelonLoader;
using ModSettings;


namespace FeatSettings
{
    public class MasterHunterSettings : FeatSpecificSettings<Feat_MasterHunter>
    {
        public MasterHunterSettings(FeatSettingsManager manager) : base(manager) { }


        public override void ApplyAdjustedFeatSettings()
        {
            if (mFeat == null)
            {
                return;
            }
            mFeat.m_AiSoundRangeScale = Math.Clamp(100 - SoundDecreasePercent, 0, 100);
            mFeat.m_AiSightRangeScale = Math.Clamp(100 - SightDecreasePercent, 0, 100);
            mFeat.m_AiSmellRangeScale = Math.Clamp(100 - SmellDecreasePercent, 0, 100);
        }




        [Section("Big Cat Killer")]
        [Name("Kill Count Requirement")]
        [Slider(3, 30, 10)]
        [Description("Sets required of Cougar kills to unlock Big Cat Killer.\nDefault: 9")]
        public int KillCountRequirement = 9;


        [Name("Sound Perception Decrease Percent")]
        [Slider(0, 100)]
        [Description("Sets ai sound perception decrease percent while using Big Cat Killer.\nDefault: 50%")]
        public int SoundDecreasePercent = 50;


        [Name("Sight Perception Decrease Percent")]
        [Slider(0, 100)]
        [Description("Sets ai sight perception decrease percent while using Big Cat Killer.\nDefault: 50%")]
        public int SightDecreasePercent = 50;


        [Name("Smell Perception Decrease Percent")]
        [Slider(0, 100)]
        [Description("Sets ai smell perception decrease percent while using Big Cat Killer.\nDefault: 50%")]
        public int SmellDecreasePercent = 50;
    }
}
