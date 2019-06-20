using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX
{
    internal partial class MasomodeEXPlayer : ModPlayer
    {
        private readonly Mod fargos = ModLoader.GetMod("FargowiltasSouls");

        public override void PreUpdate()
        {
            if (player.ZoneUnderworldHeight)
            {
                player.AddBuff(BuffID.Burning, 2);
            }
        }
    }
}