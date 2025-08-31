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


        protected void Initialize()
        {
            AddToModSettings("FeatSettings");
            RefreshGUI();
        }
    }
}