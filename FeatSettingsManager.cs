using Harmony;
using Il2Cpp;
using Il2CppTLD.AI;
using MelonLoader;
using TLD.TinyJSON;
using MelonLoader.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ModData;
using Il2CppInterop.Runtime.Injection;
using Il2CppTLD.Stats;
using UnityEngine.AddressableAssets;
using System.Reflection;
using LocalizationUtilities;

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

        [Serializable]
        public class FeatSettingsData
        {
            [Include] int mCougarsKilled = 0;
            [Include] Dictionary<string, float> mCustomFeatProgress = new Dictionary<string, float>();

            public int CougarsKilled { get { return mCougarsKilled; } set { mCougarsKilled = value; } }
            public Dictionary<string, float> CustomFeatProgress { get { return mCustomFeatProgress; } }
        }

        private const string ModName = "FeatSettings";
        private Settings mSettings;
        private Dictionary<Type, FeatSpecificSettingsBase> mFeatSettingsDict = new Dictionary<Type, FeatSpecificSettingsBase>();
        private Dictionary<string, FeatSpecificSettingsBase> mFeatSettingsByNameDict = new Dictionary<string, FeatSpecificSettingsBase>();
        private Dictionary<FeatType, CustomFeatDefinition> mCustomFeatDefinitions = new Dictionary<FeatType, CustomFeatDefinition>();
        private List<LocalizedString> mCustomFeatLocalizedStrings = new List<LocalizedString>();
        private List<AssetReferenceTexture2D> mCustomBadgeReferences = new List<AssetReferenceTexture2D>();
        private Dictionary<IntPtr, Texture2D> mCustomBadgeTextures = new Dictionary<IntPtr, Texture2D>();
        private MelonLogger.Instance mLogger;
        private FeatSettingsData mData = new FeatSettingsData();
        private ModDataManager mModDataManager = new ModDataManager(ModName);

        public FeatSettingsData Data { get { return mData; } }
        public Settings Settings { get { return mSettings; } }

        public void Initialize(MelonLogger.Instance logger)
        {
            mLogger = logger;
            ClassInjector.RegisterTypeInIl2Cpp<CustomFeat>();
            LoadEmbeddedLocalization(typeof(FeatSettingsManager).Assembly, $"{ModName}.Localization.json");
            InitializeSystems();
            InitializeCustomSettings();
        }

        private void InitializeCustomSettings()
        {
            AddFeatSettings(typeof(Feat_BlizzardWalker), new BlizzardWalkerSettings(this, Path.Combine(ModName, nameof(BlizzardWalkerSettings)), ModName));
            AddFeatSettings(typeof(Feat_BookSmarts), new BookSmartsSettings(this, Path.Combine(ModName, nameof(BookSmartsSettings)), ModName));
            AddFeatSettings(typeof(Feat_CelestialNavigator), new CelestialNavigatorSettings(this, Path.Combine(ModName, nameof(CelestialNavigatorSettings)), ModName));
            AddFeatSettings(typeof(Feat_ColdFusion), new ColdFusionSettings(this, Path.Combine(ModName, nameof(ColdFusionSettings)), ModName));
            AddFeatSettings(typeof(Feat_EfficientMachine), new EfficientMachineSettings(this, Path.Combine(ModName, nameof(EfficientMachineSettings)), ModName));
            AddFeatSettings(typeof(Feat_ExpertTrapper), new ExpertTrapperSettings(this, Path.Combine(ModName, nameof(ExpertTrapperSettings)), ModName));
            AddFeatSettings(typeof(Feat_FireMaster), new FireMasterSettings(this, Path.Combine(ModName, nameof(FireMasterSettings)), ModName));
            AddFeatSettings(typeof(Feat_FreeRunner), new FreeRunnerSettings(this, Path.Combine(ModName, nameof(FreeRunnerSettings)), ModName));
            AddFeatSettings(typeof(Feat_MasterHunter), new MasterHunterSettings(this, Path.Combine(ModName, nameof(MasterHunterSettings)), ModName));
            AddFeatSettings(typeof(Feat_NightWalker), new NightWalkerSettings(this, Path.Combine(ModName, nameof(NightWalkerSettings)), ModName));
            AddFeatSettings(typeof(Feat_SettledMind), new SettledMindSettings(this, Path.Combine(ModName, nameof(SettledMindSettings)), ModName));
            AddFeatSettings(typeof(Feat_SnowWalker), new SnowWalkerSettings(this, Path.Combine(ModName, nameof(SnowWalkerSettings)), ModName));
            AddFeatSettings(typeof(Feat_StraightToHeart), new StraightToTheHeartSettings(this, Path.Combine(ModName, nameof(StraightToTheHeartSettings)), ModName));
        }

        private void AddFeatSettings(Type type, FeatSpecificSettingsBase settings)
        {
            mFeatSettingsDict.Add(type, settings);
            mFeatSettingsByNameDict.Add(settings.FeatName.ToLower(), settings);
        }


        private void InitializeSystems()
        {
            Directory.CreateDirectory(Path.Combine(MelonEnvironment.ModsDirectory, ModName));
            mSettings = new Settings(this);
        }

        
        public void LoadModData()
        {
            string profileData = mModDataManager.LoadFromProfile();
            if (string.IsNullOrEmpty(profileData))
                return;
            Variant variant = JSON.Load(profileData);
            JSON.Populate(variant, mData);
        }

        public void SaveModData()
        {
            string profileData = JSON.Dump(mData, EncodeOptions.NoTypeHints);
            if (string.IsNullOrEmpty(profileData))
                return;
            mModDataManager.SaveToProfile(profileData);
        }


        public void Log(string msg)
        {
            mLogger.Msg(msg);
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

        public bool TryGetFeatSpecificSettingsByName(string featName, out FeatSpecificSettingsBase settings) => mFeatSettingsByNameDict.TryGetValue(featName.ToLower(), out settings);

        #region Custom Feats

        public void InstantiateCustomFeats(FeatsManager featsManager)
        {
            foreach (CustomFeatDefinition definition in mCustomFeatDefinitions.Values)
            {
                if (definition.Feat != null)
                {
                    continue;
                }
                Feat badgeFeat = featsManager.GetFeat(definition.BadgeFeat);
                GameObject featObject = new GameObject($"Feat_{definition.Name}");
                featObject.hideFlags = HideFlags.HideInHierarchy;
                UnityEngine.Object.DontDestroyOnLoad(featObject);
                CustomFeat feat = featObject.AddComponent<CustomFeat>();
                feat.m_FeatType = definition.FeatType;
                feat.m_LocalizedDisplayName = NewLocalizedString(definition.DisplayName);
                feat.m_LocalizedDescription = NewLocalizedString(definition.Description);
                feat.m_LocalizedShortDescription = NewLocalizedString(definition.ShortDescription);
                feat.m_LocalizedBonusDescription = NewLocalizedString(definition.BonusDescription);
                feat.m_UnlockedBadge = NewBadgeReference(definition.UnlockedBadgeTexture, badgeFeat.m_UnlockedBadge);
                feat.m_LockedBadge = NewBadgeReference(definition.LockedBadgeTexture, badgeFeat.m_LockedBadge);
                feat.m_ActionsRadial = badgeFeat.m_ActionsRadial;
                FeatsManager.m_Feats.Add(feat);
                definition.Feat = feat;
                Log($"Added custom feat {definition.Name} as feat type {(int)definition.FeatType}");
            }
        }

        private AssetReferenceTexture2D NewBadgeReference(Texture2D? texture, AssetReferenceTexture2D badge)
        {
            if (texture == null)
            {
                return badge;
            }
            AssetReferenceTexture2D reference = new AssetReferenceTexture2D(badge.AssetGUID);
            texture.hideFlags = HideFlags.DontUnloadUnusedAsset;
            mCustomBadgeReferences.Add(reference);
            mCustomBadgeTextures[reference.Pointer] = texture;
            return reference;
        }

        public bool TryGetCustomBadgeTexture(AssetReferenceTexture2D? reference, [NotNullWhen(true)] out Texture2D? texture)
        {
            texture = null;
            return reference != null && mCustomBadgeTextures.TryGetValue(reference.Pointer, out texture);
        }

        public void ApplyCustomBadgeTextures(Il2CppSystem.Collections.Generic.List<BadgeInfo> badges, ScrollList scrollList)
        {
            for (int i = 0, iMax = Math.Min(badges.Count, scrollList.m_ScrollObjects.Count); i < iMax; i++)
            {
                if (!TryGetCustomBadgeTexture(badges[i].m_Sprite, out Texture2D? texture)) continue;
                UITexture uiTexture = scrollList.m_ScrollObjects[i].GetComponentInChildren<UITexture>(true);
                if (uiTexture == null) continue;
                uiTexture.mainTexture = texture;
            }
        }

        private LocalizedString NewLocalizedString(string localizationID)
        {
            LocalizedString localizedString = new LocalizedString();
            localizedString.m_LocalizationID = localizationID;
            mCustomFeatLocalizedStrings.Add(localizedString);
            return localizedString;
        }

        public bool LoadEmbeddedLocalization(Assembly assembly, string resourceName)
        {
            using Stream? stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                Log($"Error in FeatSettingsManager.LoadEmbeddedLocalization: no resource {resourceName} in {assembly.GetName().Name}!");
                return false;
            }
            using StreamReader reader = new StreamReader(stream);
            return LocalizationManager.LoadJsonLocalization(reader.ReadToEnd());
        }

        public bool TryGetCustomFeatDefinition(FeatType featType, out CustomFeatDefinition definition) => mCustomFeatDefinitions.TryGetValue(featType, out definition);

        public float GetCustomFeatProgress(CustomFeatDefinition definition) => mData.CustomFeatProgress.TryGetValue(definition.Name, out float progress) ? progress : 0.0f;

        public void SetCustomFeatProgress(CustomFeatDefinition definition, float progress)
        {
            bool wasUnlocked = GetCustomFeatProgress(definition) >= 1.0f;
            mData.CustomFeatProgress[definition.Name] = progress;
            if (!wasUnlocked && progress >= 1.0f && definition.Feat != null)
            {
                Log($"Custom feat {definition.Name} unlocked!");
                definition.Feat.HandleOnFeatUnlocked();
            }
        }

        public void ApplyCustomFeatSettings(CustomFeatSettings settings)
        {
            foreach (CustomFeatDefinition definition in mCustomFeatDefinitions.Values)
            {
                if (definition.Settings == settings)
                {
                    definition.ApplySettings();
                }
            }
        }

        public void OnStatIncremented(StatID statID, float increment)
        {
            foreach (CustomFeatDefinition definition in mCustomFeatDefinitions.Values)
            {
                definition.OnStatIncremented(statID, increment);
            }
        }

        #endregion

        #region Console Commands

        public void Console_SetCougarKills()
        {
            string command = uConsole.GetString();
            if (command == null || command.Length == 0)
            {
                Log($"Enter kill quantity!");
                return;
            }
            if (!int.TryParse(command, out int value))
            {
                Log($"Enter kill quantity as integer!");
                return;
            }
            mData.CougarsKilled = value;
            if (!TryGetFeatSpecificSettings<Feat_MasterHunter>(out FeatSpecificSettings<Feat_MasterHunter>? settings)) return;
            if (settings is not MasterHunterSettings masterHunterSettings) return;
            masterHunterSettings.MaybeUnlock();
        }

        public void Console_EnableFeat()
        {
            if (!TryGetConsoleFeat(out Feat? feat)) return;
            if (FeatEnabledTracker.m_FeatsEnabledThisSandbox.Contains(feat.m_FeatType)) return;
            FeatEnabledTracker.m_FeatsEnabledThisSandbox.Add(feat.m_FeatType);
        }

        public void Console_DisableFeat()
        {
            if (!TryGetConsoleFeat(out Feat? feat)) return;
            if (!FeatEnabledTracker.m_FeatsEnabledThisSandbox.Contains(feat.m_FeatType)) return;
            FeatEnabledTracker.m_FeatsEnabledThisSandbox.Remove(feat.m_FeatType);
        }

        private bool TryGetConsoleFeat([NotNullWhen(true)] out Feat? feat)
        {
            feat = null;
            string featName = uConsole.GetString();
            if (featName == null || featName.Length == 0)
            {
                Log($"Enter feat name!");
                return false;
            }
            if (TryGetFeatSpecificSettingsByName(featName, out FeatSpecificSettingsBase settings))
            {
                if (!settings.Vanilla)
                {
                    Log($"Non vanilla feats WIP!");
                    return false;
                }
                feat = settings.BaseFeat;
            }
            else
            {
                feat = mCustomFeatDefinitions.Values.FirstOrDefault(definition => definition.Name.ToLower() == featName.ToLower())?.Feat;
            }
            if (feat == null)
            {
                Log($"Invalid feat name!");
                return false;
            }
            if (!feat.IsUnlocked())
            {
                Log($"You must unlock this feat first!");
                return false;
            }
            return true;
        }

        #endregion

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

        public bool TryRegisterCustomFeat(CustomFeatDefinition definition)
        {
            if ((int)definition.FeatType <= (int)FeatType.CelestialNavigator || !mCustomFeatDefinitions.TryAdd(definition.FeatType, definition))
            {
                Log($"Error in FeatSettingsManager.TryRegisterCustomFeat: Could not add custom feat {definition.Name}, feat type {(int)definition.FeatType} is a vanilla feat or already added!");
                return false;
            }
            Log($"FeatSettingsManager.TryRegisterCustomFeat: Registered custom feat {definition.Name} as feat type {(int)definition.FeatType}!");
            if (definition.Settings != null)
            {
                definition.Settings.AddToModSettings(ModName);
                definition.Settings.RefreshGUI();
                definition.ApplySettings();
            }
            return true;
        }

        #endregion
    }
}
