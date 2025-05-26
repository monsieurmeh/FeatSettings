using ModSettings;
using Il2Cpp;


namespace FeatSettings
{
    public class Settings : JsonModSettings
    {
        protected FeatSettingsManager mManager;

        public Settings(FeatSettingsManager manager)
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