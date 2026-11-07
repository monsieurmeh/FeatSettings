using ModSettings;


namespace FeatSettings
{
    public abstract class CustomFeatSettings : JsonModSettings
    {
        public CustomFeatSettings(string name) : base(Path.Combine("FeatSettings", name)) { }


        protected override void OnConfirm()
        {
            base.OnConfirm();
            FeatSettingsManager.Instance.ApplyCustomFeatSettings(this);
        }
    }
}
