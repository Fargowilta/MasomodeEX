using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX.Projectiles
{
    public class MasomodeEXGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void AI(Projectile projectile)
        {
            switch (projectile.type)
            {
                case ProjectileID.EyeFire:
                    projectile.position += projectile.velocity / 2f;
                    break;

                case ProjectileID.MoonLeech:
                    if (projectile.Center == Main.player[(int)projectile.ai[1]].Center)
                        Main.player[(int)projectile.ai[1]].position += Main.player[(int)projectile.ai[1]].DirectionTo(Main.npc[(int)projectile.ai[0]].Center) * 3f;
                    break;

                default:
                    break;
            }
        }

        public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
        {
            switch (projectile.type)
            {
                case ProjectileID.PinkLaser:
                case ProjectileID.DeathLaser:
                case ProjectileID.BombSkeletronPrime:
                case ProjectileID.CursedFlameHostile:
                case ProjectileID.EyeFire:
                case ProjectileID.VortexLightning:
                case ProjectileID.RocketSkeleton:
                case ProjectileID.FlamesTrap:
                    for (int i = 0; i < 200; i++) //look for any mech boss, if alive then debuff
                    {
                        if (Main.npc[i].active && (Main.npc[i].type == NPCID.SkeletronPrime || Main.npc[i].type == NPCID.TheDestroyer || Main.npc[i].type == NPCID.Retinazer || Main.npc[i].type == NPCID.Spazmatism))
                        {
                            target.AddBuff(MasomodeEX.Souls.BuffType("ClippedWings"), 2);
                            break;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}