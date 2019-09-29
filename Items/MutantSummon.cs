using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX.Items
{
    public class MutantSummon : ModItem
    {
        public override string Texture => "FargowiltasSouls/Items/Placeholder";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
        }

        public override void UpdateInventory(Player player)
        {
            if (--item.stack < 1)
                item.SetDefaults();
            NPC.SpawnOnPlayer(player.whoAmI, MasomodeEX.Souls.NPCType("MutantBoss"));
        }

        public override bool CanPickup(Player player)
        {
            return true;
        }

        public override bool OnPickup(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, MasomodeEX.Souls.NPCType("MutantBoss"));
            return false;
        }
    }
}
