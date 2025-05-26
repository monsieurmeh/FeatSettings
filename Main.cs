using MelonLoader;
using UnityEngine;
using FeatSettings;
using System.Reflection;


namespace FeatSettings
{
	public class Main : MelonMod
	{
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg(Initialize() ? "Initialized Successfully!" : "Initialization Errors!");
        }


        public override void OnDeinitializeMelon()
        {
            LoggerInstance.Msg(Shutdown() ? "Shutdown Successfully!" : "Shutdown Errors!");
        }


        protected bool Initialize()
        {
            FeatSettingsManager.Instance?.Initialize();
            return FeatSettingsManager.Instance != null;
        }


        protected bool Shutdown()
        {
            return true;
        }
    }
}