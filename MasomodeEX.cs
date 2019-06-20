using Terraria.ModLoader;

namespace MasomodeEX
{
    internal partial class MasomodeEX : Mod
    {
        internal static MasomodeEX Instance
        {
            get; private set;
        }

        internal static Mod Souls;

        internal bool HyperLoaded;

        public override void Load()
        {
            Instance = this;
        }
        public override void Unload()
        {
            Instance = null;
        }

        public override void PostSetupContent()
        {
            Souls = ModLoader.GetMod("FargowiltasSouls");
            HyperLoaded = ModLoader.GetMod("HyperMode") != null;
        }
    }
}