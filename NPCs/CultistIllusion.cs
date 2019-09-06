using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX
{
    public class CultistIllusion : ModNPC
    {
        public override string Texture => "Terraria/NPC_439";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunatic Cultist");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.CultistBoss];
        }

        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 50;
            npc.damage = 50;
            npc.defense = 42;
            npc.lifeMax = 32000;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = 75;
            npc.lifeMax = (int)(40000* bossLifeScale);
        }

        public override void AI()
        {
            if (npc.ai[0] < 0f || npc.ai[0] >= 200f)
            {
                npc.StrikeNPCNoInteraction(9999, 0f, 0);
                npc.active = false;
                return;
            }
            NPC cultist = Main.npc[(int)npc.ai[0]];
            if (!cultist.active || cultist.type != NPCID.CultistBoss)
            {
                npc.StrikeNPCNoInteraction(9999, 0f, 0);
                npc.active = false;
                for (int i = 0; i < 40; i++)
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 5);
                    Main.dust[d].velocity *= 2.5f;
                    Main.dust[d].scale += 0.5f;
                }
                for (int i = 0; i < 20; i++)
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, 229, 0f, 0f, 0, default(Color), 2f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].noLight = true;
                    Main.dust[d].velocity *= 9f;
                }
                return;
            }

            npc.target = cultist.target;
            npc.damage = cultist.damage;
            npc.defDamage = cultist.damage;
            if (npc.HasPlayerTarget)
            {
                Vector2 dist = Main.player[npc.target].Center - cultist.Center;
                npc.Center = Main.player[npc.target].Center;
                npc.position.X += dist.X * npc.ai[1];
                npc.position.Y += dist.Y * npc.ai[2];
                npc.direction = npc.spriteDirection = npc.position.X < Main.player[npc.target].position.X ? 1 : -1;

                if (cultist.ai[3] != -1f)
                {
                    switch ((int)cultist.ai[0])
                    {
                        case 2:
                            if (cultist.ai[1] == 3f && Main.netMode != 1) //ice mist, frost wave support
                            {
                                Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                distance.Normalize();
                                distance = distance.RotatedByRandom(Math.PI / 12);
                                distance *= Main.rand.NextFloat(6f, 9f);
                                Projectile.NewProjectile(npc.Center, distance,
                                    ProjectileID.FrostWave, npc.damage / 3, 0f, Main.myPlayer);
                            }
                            break;

                        case 3:
                            if (cultist.ai[1] == 3f && Main.netMode != 1) //fireballs, solar goop support
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.SolarGoop);
                                if (n < 200)
                                {
                                    Main.npc[n].velocity.X = Main.rand.Next(-10, 11);
                                    Main.npc[n].velocity.Y = Main.rand.Next(-15, -4);
                                    if (Main.netMode == 2)
                                        NetMessage.SendData(23, -1, -1, null, n);
                                }
                            }
                            break;

                        case 4:
                            if (cultist.ai[1] == 19f && Main.netMode != 1) //lightning orb, lightning support
                            {
                                Vector2 dir = Main.player[npc.target].Center - npc.Center;
                                float ai1New = Main.rand.Next(100);
                                Vector2 vel = Vector2.Normalize(dir.RotatedByRandom(Math.PI / 4)) * 7f;
                                Projectile.NewProjectile(npc.Center, vel, ProjectileID.CultistBossLightningOrbArc,
                                    npc.damage / 15 * 6, 0, Main.myPlayer, dir.ToRotation(), ai1New);
                            }
                            break;

                        case 7:
                            if (cultist.ai[1] == 3f && Main.netMode != 1) //ancient light, phantasmal eye support
                            {
                                Vector2 speed = Vector2.UnitX.RotatedByRandom(Math.PI);
                                speed *= 6f;
                                Projectile.NewProjectile(npc.Center, speed,
                                    ProjectileID.PhantasmalEye, npc.damage / 3, 0f, Main.myPlayer);
                                Projectile.NewProjectile(npc.Center, speed.RotatedBy(Math.PI * 2 / 3),
                                    ProjectileID.PhantasmalEye, npc.damage / 3, 0f, Main.myPlayer);
                                Projectile.NewProjectile(npc.Center, speed.RotatedBy(-Math.PI * 2 / 3),
                                    ProjectileID.PhantasmalEye, npc.damage / 3, 0f, Main.myPlayer);
                            }
                            break;

                        case 8:
                            if (cultist.ai[1] == 3f) //ancient doom, nebula sphere support
                            {
                                if (npc.active && npc.type == NPCID.CultistBossClone)
                                    Projectile.NewProjectile(npc.Center, Vector2.Zero,
                                        ProjectileID.NebulaSphere, npc.damage / 15 * 6, 0f, Main.myPlayer);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            else
            {
                npc.Center = cultist.Center;
            }
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreNPCLoot()
        {
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC cultist = Main.npc[(int)npc.ai[0]];
            if (cultist.active && cultist.type == NPCID.CultistBoss)
                npc.frame.Y = cultist.frame.Y;
        }
    }
}