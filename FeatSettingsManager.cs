using Harmony;
using Il2Cpp;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FeatSettings
{
    public class FeatSettingsManager
    {
        #region Lazy Singleton

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly FeatSettingsManager instance = new FeatSettingsManager();
        }

        private FeatSettingsManager() { }
        public static FeatSettingsManager Instance { get { return Nested.instance; } }

        #endregion

        private Settings mSettings;
        private Dictionary<Type, FeatSpecificSettingsBase> mFeatSettingsDict = new Dictionary<Type, FeatSpecificSettingsBase>();

        public void Initialize()
        {
            InitializeSystems();
            InitializeCustomSettings();
        }

        private void InitializeCustomSettings()
        {
            mFeatSettingsDict.Add(typeof(Feat_BlizzardWalker), new BlizzardWalkerSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_BookSmarts), new BookSmartsSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_CelestialNavigator), new CelestialNavigatorSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_ColdFusion), new ColdFusionSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_EfficientMachine), new EfficientMachineSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_ExpertTrapper), new ExpertTrapperSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_FireMaster), new FireMasterSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_FreeRunner), new FreeRunnerSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_MasterHunter), new MasterHunterSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_NightWalker), new NightWalkerSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_SettledMind), new SettledMindSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_SnowWalker), new SnowWalkerSettings(this));
            mFeatSettingsDict.Add(typeof(Feat_StraightToHeart), new StraightToTheHeartSettings(this));
        }


        private void InitializeSystems()
        {
            mSettings = new Settings(this);
        }


        public void Log(string msg)
        {
            MelonLogger.Msg(msg);
            //mLogger.Log(msg, FlaggedLoggingLevel.Debug);
        }


        public void RegisterFeat(GameObject featPrefab)
        {
            try
            {
                if (featPrefab == null)
                {
                    Log("Null feat prefab, skipping.");
                    return;
                }
                if (!featPrefab.TryGetComponent<Feat>(out Feat feat))
                {
                    Log("No feat found, skipping.");
                    return;
                }
                if (feat == null)
                {
                    Log($"Somehow feat is null, skipping.");
                    return;
                }
                if (!TryRegisterFeat(feat))
                {
                    Log($"Couldnt register feat {feat.name}!");
                    return;
                }

            }
            catch (Exception e)
            {
                Log($"ERROR in FeatSettingsManager.RegisterFeat: {e}");
                return;
            }
        }

        //A simple "is this a that" check should be working here but il2cpp sorcery is hindering me... feh. have to re-grab the new stupid component i guess.
        private bool TryRegisterFeat(Feat feat)
        {
            switch (feat.m_FeatType)
            {
                case FeatType.BlizzardWalker: return TryRegisterFeat<Feat_BlizzardWalker>(feat);
                case FeatType.BookSmarts: return TryRegisterFeat<Feat_BookSmarts>(feat);
                case FeatType.CelestialNavigator: return TryRegisterFeat<Feat_CelestialNavigator>(feat);
                case FeatType.ColdFusion: return TryRegisterFeat<Feat_ColdFusion>(feat);
                case FeatType.EfficientMachine: return TryRegisterFeat<Feat_EfficientMachine>(feat);
                case FeatType.ExpertTrapper: return TryRegisterFeat<Feat_ExpertTrapper>(feat);
                case FeatType.FireMaster: return TryRegisterFeat<Feat_FireMaster>(feat);
                case FeatType.FreeRunner: return TryRegisterFeat<Feat_FreeRunner>(feat);
                case FeatType.MasterHunter: return TryRegisterFeat<Feat_MasterHunter>(feat);
                case FeatType.NightWalker: return TryRegisterFeat<Feat_NightWalker>(feat);
                case FeatType.SettledMind: return TryRegisterFeat<Feat_SettledMind>(feat);
                case FeatType.SnowWalker: return TryRegisterFeat<Feat_SnowWalker>(feat);
                case FeatType.StraightToHeart: return TryRegisterFeat<Feat_StraightToHeart>(feat);
                default:
                    Log($"Unknown feat {feat.name} of type {feat.GetType()}!");
                    return false;
            }
        }


        private bool TryRegisterFeat<T>(Feat feat) where T : Feat
        {
            if (!feat.gameObject.TryGetComponent<T>(out T tFeat))
            {
                Log($"ERROR in FeatSettingsManager.TryRegisterFeat: could not get specific feat class from gameobject!");
                return false;
            }
            if (!mFeatSettingsDict.TryGetValue(typeof(T), out FeatSpecificSettingsBase? featSettings))
            {
                Log($"ERROR during FeatSettingsManager.TryRegisterFeat: Could not fetch settings for type {typeof(T).Name}!");
                return false;
            }
            if (featSettings is not FeatSpecificSettings<T> correctFeatSettings)
            {
                Log($"ERROR during FeatSettingsManager.TryRegisterFeat: Could not convert feat settings to correct class!");
                return false;
            }
            correctFeatSettings.Initialize(tFeat);
            return true;
        }

        
        public bool TryGetFeatSpecificSettings<T>(out FeatSpecificSettings<T>? settings) where T : Feat
        {
            settings = null;
            if (!mFeatSettingsDict.TryGetValue(typeof(T), out FeatSpecificSettingsBase? baseSettings))
            {
                return false;
            }
            if (baseSettings is not FeatSpecificSettings<T> specificSettings)
            {
                return false;
            }
            settings = specificSettings;
            return true;
        }


        #region Interop

        public bool TryRegisterFeatSettings(Type type, FeatSpecificSettingsBase featSettings)
        {
            if (!mFeatSettingsDict.TryAdd(type, featSettings))
            {
                Log($"Error in FeatSettingsManager.RegisterFeat: Could not add new feat type {type.Name}'s featsettings {featSettings.GetType()} as it is already added with custom settings!");
                return false;
            }
            Log($"FeatSettingsMaanger.RegisterFeat: Registered feat type {type.Name}'s featsettings {featSettings.GetType()}!");
            return true;
        }

        #endregion
    }
}
