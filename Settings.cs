using ModSettings;
using Il2Cpp;


namespace FeatSettings
{
    public class Settings : JsonModSettings
    {
        protected FeatSettingsManager mManager;

        public Settings(FeatSettingsManager manager) : base(Path.Combine("FeatSettings", "FeatSettings"))
        {
            mManager = manager;
            Initialize();
        }

        [Section("Feat Count")]
        [Name("Pilgrim Feat Count")]
        [Slider(1, 6, 6)]
        public int PilgrimFeatCount = 5;

        [Name("Voyager Feat Count")]
        [Slider(1, 6, 6)]
        public int VoyagerFeatCount = 4;

        [Name("Stalker Feat Count")]
        [Slider(1, 6, 6)]
        public int StalkerFeatCount = 3;

        [Name("Interloper Feat Count")]
        [Slider(1, 6, 6)]
        public int InterloperFeatCount = 2;

        [Name("Misery Feat Count")]
        [Slider(1, 6, 6)]
        public int MiseryFeatCount = 0;

        [Name("Custom Feat Count")]
        [Slider(1, 6, 6)]
        public int CustomFeatCount = 5;

        protected void Initialize()
        {
            AddToModSettings("FeatSettings");
            RefreshGUI();
        }
    }
}