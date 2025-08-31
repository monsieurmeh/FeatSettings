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
        }




        [Section("Big Cat Killer")]
        [Name("Kill Count Requirement")]
        [Slider(3, 30, 10)]
        [Description("Sets required of Cougar kills to unlock Big Cat Killer.\nMod default with current vanilla cougar: 9\nVanilla default from cougar 1.0: 3")]
        public int KillCountRequirement = 9;


        [Name("Sound Perception Decrease Percent")]
        [Slider(0, 95, 20)]
        [Description("Sets AI sound perception decrease percent while using Big Cat Killer.\nDefault: 50%")]
        public int SoundDecreasePercent = 50;


        [Name("Sight Perception Decrease Percent")]
        [Slider(0, 95, 20)]
        [Description("Sets AI sight perception decrease percent while using Big Cat Killer.\nDefault: 50%")]
        public int SightDecreasePercent = 50;


        [Name("Smell Perception Decrease Percent")]
        [Slider(0, 95, 20)]
        [Description("Sets AI smell perception decrease percent while using Big Cat Killer.\nDefault: 50%")]
        public int SmellDecreasePercent = 50;
    }
}
