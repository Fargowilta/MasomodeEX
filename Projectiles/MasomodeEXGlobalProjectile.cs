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
                    projectile.position += projectile.velocity;
                    break;

                case ProjectileID.MoonLeech:
                    if (projectile.Center == Main.player[(int)projectile.ai[1]].Center)
                        Main.player[(int)projectile.ai[1]].position += Main.player[(int)projectile.ai[1]].DirectionTo(Main.npc[(int)projectile.ai[0]].Center) * 5f;
                    break;

                case ProjectileID.Sharknado:
                case ProjectileID.Cthulunado:
                    if (Main.player[Main.myPlayer].active && projectile.Distance(Main.player[Main.myPlayer].Center) < 1000)
                        Main.player[Main.myPlayer].position += Main.player[Main.myPlayer].DirectionTo(projectile.Center) * 0.1f;
                    break;

                case ProjectileID.SandnadoHostile:
                    if (Main.player[Main.myPlayer].active && projectile.Distance(Main.player[Main.myPlayer].Center) < 1000)
                        Main.player[Main.myPlayer].position += Main.player[Main.myPlayer].DirectionTo(projectile.Center) * 2;
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

                case ProjectileID.EyeFire:
                case ProjectileID.CursedFlameHostile:
                    target.AddBuff(BuffID.CursedInferno, Main.rand.Next(60, 600));
                    target.AddBuff(BuffID.Weak, Main.rand.Next(7200));
                    target.AddBuff(MasomodeEX.Souls.BuffType("Shadowflame"), Main.rand.Next(60, 600));
                    goto case ProjectileID.RocketSkeleton;

                case ProjectileID.BombSkeletronPrime:
                    if (!target.HasBuff(MasomodeEX.Souls.BuffType("Fused")))
                        target.AddBuff(MasomodeEX.Souls.BuffType("Fused"), 600);
                    goto case ProjectileID.RocketSkeleton;

                case ProjectileID.GoldenShowerHostile:
                    target.AddBuff(BuffID.Ichor, Main.rand.Next(60, 600));
                    target.AddBuff(BuffID.Bleeding, Main.rand.Next(7200));
                    target.AddBuff(MasomodeEX.Souls.BuffType("Bloodthirsty"), Main.rand.Next(300));
                    break;

                case ProjectileID.PhantasmalDeathray:
                    break;

                default:
                    break;
            }
        }
    }
}