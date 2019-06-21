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

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (!spawned)
            {
                spawned = true;
                if (Main.rand.Next(4) == 0 && Main.netMode != 1)
                    Projectile.NewProjectile(item.Center.X, item.Center.Y, Main.rand.Next(-30, 31) * .1f, Main.rand.Next(-40, -15) * .1f,
                        MasomodeEX.Souls.ProjectileType("FakeHeart"), 40, 0f, Main.myPlayer);
            }
        }
    }
}
