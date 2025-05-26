using Il2Cpp;
using MelonLoader;
using ModSettings;

namespace FeatSettings
{
    public abstract class FeatSpecificSettingsBase : JsonModSettings
    {
        protected FeatSettingsManager mManager;

        public FeatSpecificSettingsBase(FeatSettingsManager manager)
        {
            mManager = manager;
            Initialize();
        }


        protected virtual void Initialize()
        {
            try
            {
                AddToModSettings("FeatSettings");
                RefreshGUI();
                ApplyAdjustedFeatSettings();
            }
            catch (Exception e)
            {
                mManager.Log($"ERROR in FeatSpecificSettingsBase.Initialize: {e}");
            }
        }


        protected override void OnConfirm()
        {
            try
            {
                ApplyAdjustedFeatSettings();
            }
            catch (Exception e)
            {
                mManager.Log($"ERROR in FeatSpecificSettingsBase.onConfirm: {e}");
            }
        }


        public abstract void ApplyAdjustedFeatSettings();
    }


    public abstract class FeatSpecificSettings<T> : FeatSpecificSettingsBase where T : Feat
    {
        protected T? mFeat;

        public T? Feat { get { return mFeat; } }

        public FeatSpecificSettings(FeatSettingsManager manager) : base(manager) { }

        public virtual void Initialize(T tFeat)
        {
            mFeat = tFeat;
        }
    }
}
