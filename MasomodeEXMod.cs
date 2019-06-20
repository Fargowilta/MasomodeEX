using Terraria.ModLoader;

namespace MasomodeEX
{
    internal partial class MasomodeEXMod : Mod
    {
        internal static MasomodeEXMod Instance
        {
            get; private set;
        }

        public override void Load()
        {
            Instance = this;
        }
        public override void Unload()
        {
            Instance = null;
        }
    }
}