using Il2Cpp;
using MelonLoader;
using Il2CppTLD.Stats;


namespace FeatSettings
{
    public abstract class CustomFeatDefinition
    {
        public abstract FeatType FeatType { get; }
        public abstract string Name { get; }
        public abstract string DisplayName { get; }
        public abstract string Description { get; }
        public abstract string ShortDescription { get; }
        public abstract string BonusDescription { get; }
        public abstract FeatType BadgeFeat { get; }

        public CustomFeat? Feat { get; internal set; }
    }


    public class CustomFeat : Feat
    {
        public CustomFeat(IntPtr ptr) : base(ptr) { }
    }
}
