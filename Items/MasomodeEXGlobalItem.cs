using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX.Items
{
    class MasomodeEXGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public bool spawned;
        public int lavaCounter;

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (!spawned)
            {
                spawned = true;
                if (item.type == ItemID.Heart && Main.rand.Next(4) == 0 && Main.netMode != 1)
                    Projectile.NewProjectile(item.Center.X, item.Center.Y, Main.rand.Next(-30, 31) * .1f, Main.rand.Next(-40, -15) * .1f,
                        MasomodeEX.Souls.ProjectileType("FakeHeart"), 40, 0f, Main.myPlayer);
            }
            if (item.lavaWet && ++lavaCounter > 5)
                item.active = false;
        }

        public override void UpdateInventory(Item item, Player player)
        {
            switch(item.type)
            {
                case ItemID.GuideVoodooDoll:
                    if (player.lavaWet)
                    {
                        int guide = NPC.FindFirstNPC(NPCID.Guide);
                        if (guide != -1 && Main.npc[guide].active)
                        {
                            if (--item.stack <= 0)
                                item.SetDefaults();
                            Main.npc[guide].StrikeNPC(9999, 0f, 0);
                            if (player.ZoneUnderworldHeight)
                                NPC.SpawnWOF(player.Center);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (MasomodeEX.Instance.LuiLoaded && (item.type == ModLoader.GetMod("Luiafk").ItemType("UnlimitedGrandDesign")
                || item.type == ModLoader.GetMod("Luiafk").ItemType("ComboRod")))
                return false;
            switch(item.type)
            {
                case ItemID.WireCutter:
                case ItemID.WireKite:
                    if (player.ZoneJungle && !NPC.downedGolemBoss)
                        return false;
                    break;

                default:
                    break;
            }
            return true;
        }
    }
}
