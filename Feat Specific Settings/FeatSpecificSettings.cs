using Il2Cpp;
using MelonLoader;
using ModSettings;

namespace FeatSettings
{
    public abstract class FeatSpecificSettingsBase : JsonModSettings
    {
        protected FeatSettingsManager mManager;
        protected string mMenuName;

        public FeatSpecificSettingsBase(FeatSettingsManager manager, string path, string menuName) : base (path)
        {
            mManager = manager;
            mMenuName = menuName;
            Initialize();
        }


        protected virtual void Initialize()
        {
            try
            {
                AddToModSettings(mMenuName);
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
                base.OnConfirm();
            }
            catch (Exception e)
            {
                mManager.Log($"ERROR in FeatSpecificSettingsBase.onConfirm: {e}");
            }
        }


        public abstract void ApplyAdjustedFeatSettings();

        public abstract string FeatName { get; }
        public abstract bool Vanilla { get; }
        public abstract Feat BaseFeat { get; }
    }


    public abstract class FeatSpecificSettings<T> : FeatSpecificSettingsBase where T : Feat
    {
        protected T? mFeat;

        public T? Feat { get { return mFeat; } }
        public override Feat BaseFeat { get { return mFeat; } }

        public FeatSpecificSettings(FeatSettingsManager manager, string path, string menuName) : base(manager, path, menuName) { }

        public virtual void Initialize(T tFeat)
        {
            mFeat = tFeat;
        }
    }
}
