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

        [Section("FEATSETTINGS_FeatCount_Section", Localize = true)]
        [Name("FEATSETTINGS_FeatCount_PilgrimFeatCount_Name", Localize = true)]
        [Slider(1, 20, 20)]
        public int PilgrimFeatCount = 5;

        [Name("FEATSETTINGS_FeatCount_VoyagerFeatCount_Name", Localize = true)]
        [Slider(1, 20, 20)]
        public int VoyagerFeatCount = 4;

        [Name("FEATSETTINGS_FeatCount_StalkerFeatCount_Name", Localize = true)]
        [Slider(1, 20, 20)]
        public int StalkerFeatCount = 3;

        [Name("FEATSETTINGS_FeatCount_InterloperFeatCount_Name", Localize = true)]
        [Slider(1, 20, 20)]
        public int InterloperFeatCount = 2;

        [Name("FEATSETTINGS_FeatCount_MiseryFeatCount_Name", Localize = true)]
        [Slider(0, 20, 21)]
        public int MiseryFeatCount = 0;

        [Name("FEATSETTINGS_FeatCount_CustomFeatCount_Name", Localize = true)]
        [Slider(1, 20, 20)]
        public int CustomFeatCount = 5;

        protected void Initialize()
        {
            AddToModSettings("FeatSettings");
            RefreshGUI();
        }
    }
}