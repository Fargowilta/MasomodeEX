using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX
{
    public class MasomodeEXGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public bool[] masoBool = new bool[4];
        public int[] Counter = new int[3];

        public override void SetDefaults(NPC npc)
        {
            /*switch (npc.type)
            {
                case NPCID.MoonLordCore:
                    npc.GivenName = "Earth Lord";
                    npc.lifeMax = 6000000;
                    npc.defense = 320;
                    break;

                case NPCID.MoonLordHand:
                    npc.GivenName = "Earth Lord";
                    npc.lifeMax = 650000;
                    npc.defense = 160;
                    break;

                case NPCID.MoonLordHead:
                    npc.GivenName = "Earth Lord";
                    npc.lifeMax = 800000;
                    npc.defense = 200;
                    break;

                default:
                    break;
            }*/

            if (!npc.friendly)
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.5);
                npc.damage = (int)(npc.damage * 1.5);
                npc.defense = (int)(npc.defense * 1.5);
            }
        }

        public override void AI(NPC npc)
        {
            FargowiltasSouls.NPCs.FargoSoulsGlobalNPC fargoNPC = npc.GetGlobalNPC<FargowiltasSouls.NPCs.FargoSoulsGlobalNPC>();

            if (npc.townNPC && Main.bloodMoon && npc.type != MasomodeEX.Fargo.NPCType("Abominationn") && npc.type != MasomodeEX.Fargo.NPCType("Mutant"))
            {
                if (++Counter[0] > 60)
                {
                    Counter[0] = 0;
                    int p = Player.FindClosest(npc.Center, 0, 0);
                    if (p > -1 && Main.player[p].active && npc.Distance(Main.player[p].Center) < 300)
                        npc.Transform(NPCID.Werewolf);
                }
            }

            switch (npc.type)
            {
                case NPCID.Werewolf:
                    npc.position.X += npc.velocity.X;
                    if (npc.velocity.Y < 0)
                        npc.position.Y += npc.velocity.Y;
                    break;

                case NPCID.Lihzahrd:
                case NPCID.LihzahrdCrawler:
                case NPCID.FlyingSnake:
                    if (!NPC.downedPlantBoss)
                        npc.Transform(NPCID.DungeonGuardian);
                    break;

                case NPCID.KingSlime:
                    Aura(npc, 600, BuffID.Slimed, true, 33);
                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 offset = new Vector2();
                        double angle = Main.rand.NextDouble() * 2d * Math.PI;
                        offset.X += (float)(Math.Sin(angle) * 600);
                        offset.Y += (float)(Math.Cos(angle) * 600);
                        Dust dust = Main.dust[Dust.NewDust(
                            npc.Center + offset - new Vector2(4, 4), 0, 0,
                            33, 0, 0, 0, Color.White, 2f
                            )];
                        dust.velocity = npc.velocity;
                        if (Main.rand.Next(3) == 0)
                            dust.velocity += Vector2.Normalize(offset) * -5f;
                        dust.noGravity = true;
                    }

                    npc.position.X += npc.velocity.X;

                    if (masoBool[1])
                    {
                        if (npc.velocity.Y == 0f) //start attack
                        {
                            masoBool[1] = false;
                            if (npc.HasPlayerTarget && Main.netMode != 1)
                            {
                                const float gravity = 0.15f;
                                const float time = 60f;
                                Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                distance += Main.player[npc.target].velocity * 30f;
                                distance.X = distance.X / time;
                                distance.Y = distance.Y / time - 0.5f * gravity * time;
                                for (int i = 0; i < 10; i++)
                                {
                                    Projectile.NewProjectile(npc.Center, distance + Main.rand.NextVector2Square(-1f, 1f),
                                        MasomodeEX.Souls.ProjectileType("RainbowSlimeSpike"), npc.damage / 5, 0f, Main.myPlayer);
                                }
                            }
                        }
                    }
                    else if (npc.velocity.Y > 0)
                    {
                        masoBool[1] = true;
                    }
                    break;

                case NPCID.EyeofCthulhu:
                    Aura(npc, 600, MasomodeEX.Souls.BuffType("Berserked"), true);
                    if (WorldGen.crimson)
                    {
                        if (++Counter[0] > 240)
                        {
                            Counter[0] = 0;
                            if (Main.netMode != 1 && npc.HasPlayerTarget && npc.life < npc.lifeMax * .6)
                            {
                                const float gravity = 0.075f;
                                const float time = 120f * 3;
                                Vector2 distance = Main.player[npc.target].Center - npc.Center;
                                distance += Main.player[npc.target].velocity * 30f;
                                distance.X = distance.X / time;
                                distance.Y = distance.Y / time - 0.5f * gravity * time;
                                for (int i = 0; i < 10; i++)
                                {
                                    Projectile.NewProjectile(npc.Center, distance + Main.rand.NextVector2Square(-1f, 1f),
                                        ProjectileID.GoldenShowerHostile, npc.damage / 5, 0f, Main.myPlayer);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (++Counter[1] > 60)
                        {
                            Counter[1] = 0;
                            if (Main.netMode != 1 && npc.HasPlayerTarget && npc.life < npc.lifeMax * .6)
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.VileSpit);
                                if (n != 200 && Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }
                        if (++Counter[0] > 3)
                        {
                            Counter[0] = 0;
                            if (Main.netMode != 1 && npc.life < npc.lifeMax * .6)
                                Projectile.NewProjectile(npc.Center, npc.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-6f, 6f))) * 0.66f, ProjectileID.EyeFire, npc.damage / 5, 0f, Main.myPlayer);
                        }
                    }
                    if (++Counter[2] > 600)
                    {
                        Counter[2] = 0;
                        if (npc.HasValidTarget)
                        {
                            Player player = Main.player[npc.target];
                            Main.PlaySound(29, (int)player.position.X, (int)player.position.Y, 104, 1f, 0f);
                            if (Main.netMode != 1)
                            {
                                Vector2 spawnPos = player.Center;
                                int direction;
                                if (player.velocity.X == 0f)
                                    direction = player.direction;
                                else
                                    direction = Math.Sign(player.velocity.X);
                                spawnPos.X += 600 * direction;
                                spawnPos.Y -= 600;
                                Vector2 speed = Vector2.UnitY;
                                for (int i = 0; i < 30; i++)
                                {
                                    Projectile.NewProjectile(spawnPos, speed, MasomodeEX.Souls.ProjectileType("BloodScythe"), npc.damage / 4, 1f, Main.myPlayer);
                                    spawnPos.X += 72 * direction;
                                    speed.Y += 0.15f;
                                }
                            }
                        }
                    }
                    break;

                case NPCID.EaterofWorldsHead:
                    Aura(npc, 250, BuffID.ShadowFlame, false, DustID.Shadowflame);

                    //FUCKING FLYYYYYYY
                    if (npc.HasValidTarget)
                    {
                        npc.position -= npc.velocity;

                        int cornerX1 = (int)npc.position.X / 16 - 1;
                        int cornerX2 = (int)(npc.position.X + npc.width) / 16 + 2;
                        int cornerY1 = (int)npc.position.Y / 16 - 1;
                        int cornerY2 = (int)(npc.position.Y + npc.height) / 16 + 2;

                        //out of bounds checks
                        if (cornerX1 < 0)
                            cornerX1 = 0;
                        if (cornerX2 > Main.maxTilesX)
                            cornerX2 = Main.maxTilesX;
                        if (cornerY1 < 0)
                            cornerY1 = 0;
                        if (cornerY2 > Main.maxTilesY)
                            cornerY2 = Main.maxTilesY;

                        bool isOnSolidTile = false;

                        //for every tile npc npc occupies
                        for (int x = cornerX1; x < cornerX2; ++x)
                        {
                            for (int y = cornerY1; y < cornerY2; ++y)
                            {
                                Tile tile = Main.tile[x, y];
                                if (tile != null && (tile.nactive() && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] && tile.frameY == 0) || tile.liquid > 64))
                                {
                                    Vector2 tilePos = new Vector2(x * 16f, y * 16f);
                                    if (npc.position.X + npc.width > tilePos.X && npc.position.X < tilePos.X + 16f && npc.position.Y + npc.height > tilePos.Y && npc.position.Y < tilePos.Y + 16f)
                                    {
                                        isOnSolidTile = true;
                                        break;
                                    }
                                }
                            }

                            if (isOnSolidTile)
                                break;
                        }

                        const float num14 = 12f;    //max speed?
                        const float num15 = 0.1f;   //turn speed?
                        const float num16 = 0.15f;   //acceleration?
                        float num17 = Main.player[npc.target].Center.X;
                        float num18 = Main.player[npc.target].Center.Y;

                        float num21 = num17 - npc.Center.X;
                        float num22 = num18 - npc.Center.Y;
                        float num23 = (float)Math.Sqrt((double)num21 * (double)num21 + (double)num22 * (double)num22);

                        if (!isOnSolidTile)
                        {
                            //negating default air behaviour
                            npc.velocity.Y -= 0.15f;

                            if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num14 * 0.4f)
                            {
                                if (npc.velocity.X < 0f)
                                    npc.velocity.X += num15 * 1.1f;
                                else
                                    npc.velocity.X -= num15 * 1.1f;
                            }
                            else if (npc.velocity.Y == num14)
                            {
                                if (npc.velocity.X < num21)
                                    npc.velocity.X -= num15;
                                else if (npc.velocity.X > num21)
                                    npc.velocity.X += num15;
                            }
                            else if (npc.velocity.Y > 4f)
                            {
                                if (npc.velocity.X < 0f)
                                    npc.velocity.X -= num15 * 0.9f;
                                else
                                    npc.velocity.X += num15 * 0.9f;
                            }
                        }

                        //ground movement code but it always runs
                        float num2 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                        float num3 = Math.Abs(num21);
                        float num4 = Math.Abs(num22);
                        float num5 = num14 / num2;
                        float num6 = num21 * num5;
                        float num7 = num22 * num5;
                        if ((npc.velocity.X > 0f && num6 > 0f || npc.velocity.X < 0f && num6 < 0f) && (npc.velocity.Y > 0f && num7 > 0f || npc.velocity.Y < 0f && num7 < 0f))
                        {
                            if (npc.velocity.X < num6)
                                npc.velocity.X += num16;
                            else if (npc.velocity.X > num6)
                                npc.velocity.X -= num16;
                            if (npc.velocity.Y < num7)
                                npc.velocity.Y += num16;
                            else if (npc.velocity.Y > num7)
                                npc.velocity.Y -= num16;
                        }
                        if (npc.velocity.X > 0f && num6 > 0f || npc.velocity.X < 0f && num6 < 0f || npc.velocity.Y > 0f && num7 > 0f || npc.velocity.Y < 0f && num7 < 0f)
                        {
                            if (npc.velocity.X < num6)
                                npc.velocity.X += num15;
                            else if (npc.velocity.X > num6)
                                npc.velocity.X -= num15;
                            if (npc.velocity.Y < num7)
                                npc.velocity.Y += num15;
                            else if (npc.velocity.Y > num7)
                                npc.velocity.Y -= num15;

                            if (Math.Abs(num7) < num14 * 0.2f && (npc.velocity.X > 0f && num6 < 0f || npc.velocity.X < 0f && num6 > 0f))
                            {
                                if (npc.velocity.Y > 0f)
                                    npc.velocity.Y += num15 * 2f;
                                else
                                    npc.velocity.Y -= num15 * 2f;
                            }
                            if (Math.Abs(num6) < num14 * 0.2f && (npc.velocity.Y > 0f && num7 < 0f || npc.velocity.Y < 0f && num7 > 0f))
                            {
                                if (npc.velocity.X > 0f)
                                    npc.velocity.X += num15 * 2f;
                                else
                                    npc.velocity.X -= num15 * 2f;
                            }
                        }
                        else if (num3 > num4)
                        {
                            if (npc.velocity.X < num6)
                                npc.velocity.X += num15 * 1.1f;
                            else if (npc.velocity.X > num6)
                                npc.velocity.X -= num15 * 1.1f;

                            if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num14 * 0.5f)
                            {
                                if (npc.velocity.Y > 0f)
                                    npc.velocity.Y += num15;
                                else
                                    npc.velocity.Y -= num15;
                            }
                        }
                        else
                        {
                            if (npc.velocity.Y < num7)
                                npc.velocity.Y += num15 * 1.1f;
                            else if (npc.velocity.Y > num7)
                                npc.velocity.Y -= num15 * 1.1f;

                            if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num14 * 0.5f)
                            {
                                if (npc.velocity.X > 0f)
                                    npc.velocity.X += num15;
                                else
                                    npc.velocity.X -= num15;
                            }
                        }
                        npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
                        npc.netUpdate = true;
                        npc.localAI[0] = 1f;

                        float ratio = (float)npc.life / npc.lifeMax;
                        if (ratio > 0.5f)
                            ratio = 0.5f;
                        npc.position += npc.velocity * (1.5f - ratio);
                    }
            break;

                case NPCID.BrainofCthulhu:
                    if (npc.buffType[0] != 0)
                        npc.DelBuff(0);
                    npc.knockBackResist = 0f;
                    break;

                case NPCID.SkeletronHead:
                    Aura(npc, 420, MasomodeEX.Souls.BuffType("Lethargic"), true, 60);
                    if (++Counter[0] > 300)
                    {
                        Counter[0] = 0;
                        if (Main.netMode != 1 && npc.HasPlayerTarget)
                        {
                            Vector2 vel = npc.DirectionTo(Main.player[npc.target].Center) * 3f;
                            for (int i = 0; i < 8; i++)
                                Projectile.NewProjectile(npc.Center, vel.RotatedBy(2 * Math.PI / 8 * i), ProjectileID.Skull, npc.damage / 5, 0, Main.myPlayer, -1, 0f);
                        }
                    }
                    npc.reflectingProjectiles = npc.ai[1] == 1f || npc.ai[1] == 2f; //spinning or DG mode
                    break;

                case NPCID.SkeletronHand:
                    Aura(npc, 140, BuffID.Dazed, false);
                    break;

                case NPCID.DungeonGuardian:
                    if (npc.HasValidTarget)
                        npc.position += npc.DirectionTo(Main.player[npc.target].Center) * 5f;
                    npc.reflectingProjectiles = true;
                    break;

                case NPCID.QueenBee:
                    if (!masoBool[0] && npc.HasPlayerTarget)
                    {
                        masoBool[0] = true;
                        if (Main.netMode != 1)
                        {
                            NPC.SpawnOnPlayer(npc.target, MasomodeEX.Souls.NPCType("RoyalSubject"));
                            NPC.SpawnOnPlayer(npc.target, MasomodeEX.Souls.NPCType("RoyalSubject"));
                        }
                    }
                    if (!masoBool[1] && npc.life < npc.lifeMax / 2 && npc.HasPlayerTarget)
                    {
                        masoBool[1] = true;
                        if (Main.netMode != 1)
                        {
                            NPC.SpawnOnPlayer(npc.target, MasomodeEX.Souls.NPCType("RoyalSubject"));
                            NPC.SpawnOnPlayer(npc.target, MasomodeEX.Souls.NPCType("RoyalSubject"));
                        }
                    }
                    if (--Counter[0] < 0)
                    {
                        Counter[0] = 60;
                        masoBool[2] = NPC.AnyNPCs(MasomodeEX.Souls.NPCType("RoyalSubject"));
                    }
                    break;

                case NPCID.Bee:
                case NPCID.BeeSmall:                    
                    switch (Main.rand.Next(21))
                    {
                        case 0: npc.Transform(NPCID.Hornet); break;
                        case 1: npc.Transform(NPCID.HornetFatty); break;
                        case 2: npc.Transform(NPCID.HornetHoney); break;
                        case 3: npc.Transform(NPCID.HornetLeafy); break;
                        case 4: npc.Transform(NPCID.HornetSpikey); break;
                        case 5: npc.Transform(NPCID.HornetStingy); break;
                        case 6: npc.Transform(NPCID.LittleHornetFatty); break;
                        case 7: npc.Transform(NPCID.LittleHornetHoney); break;
                        case 8: npc.Transform(NPCID.LittleHornetLeafy); break;
                        case 9: npc.Transform(NPCID.LittleHornetSpikey); break;
                        case 10: npc.Transform(NPCID.LittleHornetStingy); break;
                        case 11: npc.Transform(NPCID.BigHornetFatty); break;
                        case 12: npc.Transform(NPCID.BigHornetHoney); break;
                        case 13: npc.Transform(NPCID.BigHornetLeafy); break;
                        case 14: npc.Transform(NPCID.BigHornetSpikey); break;
                        case 15: npc.Transform(NPCID.BigHornetStingy); break;
                        case 16: npc.Transform(NPCID.MossHornet); break;
                        case 17: npc.Transform(NPCID.BigMossHornet); break;
                        case 18: npc.Transform(NPCID.GiantMossHornet); break;
                        case 19: npc.Transform(NPCID.LittleMossHornet); break;
                        case 20: npc.Transform(NPCID.TinyMossHornet); break;
                    }
                    break;

                case NPCID.WallofFleshEye:
                    if (npc.ai[2] != 2f) //dont speed up while deathray is actually present
                        npc.ai[1]++;
                    break;

                case NPCID.TheHungry:
                case NPCID.TheHungryII:
                    Aura(npc, 100, BuffID.Burning, false, DustID.Fire);
                    break;

                case NPCID.Retinazer:
                    if (npc.ai[0] < 4f)
                        npc.ai[0] = 4f;
                    Aura(npc, 900, BuffID.Ichor, true, 90);
                    if (Counter[0]++ > 240)
                    {
                        Counter[0] = 0;
                        if (Main.netMode != 1 && npc.HasPlayerTarget)
                        {
                            Vector2 distance = Main.player[npc.target].Center - npc.Center;
                            distance.Normalize();
                            distance *= 10f;
                            for (int i = 0; i < 12; i++)
                                Projectile.NewProjectile(npc.Center, distance.RotatedBy(2 * Math.PI / 12 * i),
                                    MasomodeEX.Souls.ProjectileType("DarkStar"), npc.damage / 5, 0f, Main.myPlayer);
                        }
                    }
                    break;

                case NPCID.Spazmatism:
                    if (npc.ai[0] < 4f)
                        npc.ai[0] = 4f;
                    Aura(npc, 900, BuffID.CursedInferno, true, 89);
                    if (Counter[0]++ > 120)
                    {
                        Counter[0] = 0;
                        if (Main.netMode != 1 && npc.HasPlayerTarget)
                        {
                            Vector2 distance = Main.player[npc.target].Center - npc.Center;
                            distance.Normalize();
                            distance *= 14f;
                            for (int i = 0; i < 8; i++)
                                Projectile.NewProjectile(npc.Center, distance.RotatedBy(2 * Math.PI / 8 * i),
                                    MasomodeEX.Souls.ProjectileType("DarkStar"), npc.damage / 5, 0f, Main.myPlayer);
                        }
                    }
                    break;

                case NPCID.TheDestroyer:
                    fargoNPC.masoBool[0] = true;
                    break;

                case NPCID.TheDestroyerBody:
                case NPCID.TheDestroyerTail:
                    if (npc.ai[2] != 0 && ++Counter[0] > 420)
                    {
                        Counter[0] = 0;
                        if (Main.netMode != 1)
                            for (int i = 0; i < 6; i++)
                                Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.position - npc.oldPosition).RotatedBy(2 * Math.PI / 8 * i) * 10f,
                                    MasomodeEX.Souls.ProjectileType("DarkStar"), npc.damage / 5, 0f, Main.myPlayer);
                    }
                    break;

                case NPCID.SkeletronPrime:
                    Aura(npc, 100, MasomodeEX.Souls.BuffType("Stunned"));
                    npc.reflectingProjectiles = npc.ai[1] == 1f || npc.ai[1] == 2f; //spinning or DG mode
                    break;

                case NPCID.Plantera:
                    npc.life += 5;
                    if (npc.life > npc.lifeMax)
                        npc.life = npc.lifeMax;
                    if (++Counter[0] > 75)
                    {
                        Counter[0] = Main.rand.Next(30);
                        CombatText.NewText(npc.Hitbox, CombatText.HealLife, 300);
                    }
                    if (npc.life < npc.lifeMax / 2 && ++Counter[1] > 300)
                    {
                        Counter[1] = 0;
                        if (Main.netMode != 1)
                        {
                            int tentaclesToSpawn = 12;
                            for (int i = 0; i < 200; i++)
                                if (Main.npc[i].active && Main.npc[i].type == NPCID.PlanterasTentacle && Main.npc[i].ai[3] == 0f)
                                    tentaclesToSpawn--;

                            for (int i = 0; i < tentaclesToSpawn; i++)
                            {
                                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PlanterasTentacle, npc.whoAmI);
                                if (Main.netMode == 2)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }
                    }
                    break;

                case NPCID.PlanterasTentacle:
                    npc.position += npc.velocity;
                    Aura(npc, 200, MasomodeEX.Souls.BuffType("IvyVenom"), false, 188);
                    break;

                case NPCID.Golem:
                    if (!npc.dontTakeDamage)
                    {
                        npc.position.X += npc.velocity.X;
                        if (npc.velocity.Y < 0)
                            npc.position.Y += npc.velocity.Y;
                    }
                    break;

                case NPCID.GolemHead:
                case NPCID.GolemFistLeft:
                case NPCID.GolemFistRight:
                    Aura(npc, 200, MasomodeEX.Souls.BuffType("ClippedWings"));
                    break;

                case NPCID.GolemHeadFree:
                    Aura(npc, 200, MasomodeEX.Souls.BuffType("ClippedWings"));
                    break;

                case NPCID.DukeFishron:
                    if (!MasomodeEX.Instance.HyperLoaded)
                    {
                        masoBool[0] = !masoBool[0];
                        if (masoBool[0])
                        {
                            npc.position += npc.velocity * 1.5f;
                            npc.AI();
                        }
                    }
                    break;

                case NPCID.CultistBoss:
                    Aura(npc, 600, MasomodeEX.Souls.BuffType("MarkedforDeath"), false, 199);
                    Aura(npc, 600, MasomodeEX.Souls.BuffType("Hexed"));
                    if (++Counter[0] > 60)
                    {
                        Counter[0] = 0;
                        if (Main.netMode != 1 && npc.HasPlayerTarget && !NPC.AnyNPCs(NPCID.CultistDragonHead))
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.CultistDragonHead);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }
                    if (!masoBool[0])
                    {
                        masoBool[0] = true;
                        if (Main.netMode != 1)
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CultistIllusion"), npc.whoAmI, npc.whoAmI, -1, 1);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CultistIllusion"), npc.whoAmI, npc.whoAmI, 1, -1);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CultistIllusion"), npc.whoAmI, npc.whoAmI, 1, 1);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }
                    break;

                case NPCID.MoonLordCore:
                    fargoNPC.Counter++;
                    fargoNPC.Timer++;
                    if (masoBool[0])
                    {
                        if (--Counter[0] < 0)
                        {
                            Counter[0] = 300;
                            int pillar0 = Main.rand.Next(4);
                            switch(FargowiltasSouls.NPCs.FargoSoulsGlobalNPC.masoStateML)
                            {
                                case 0: pillar0 = 1; break; //melee
                                case 1: pillar0 = 2; break; //ranged
                                case 2: pillar0 = 0; break; //magic
                                case 3: pillar0 = 3; break; //summoner
                                default: break;
                            }
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center, Vector2.UnitY * -10f, MasomodeEX.Souls.ProjectileType("CelestialPillar"),
                                    (int)(75 * (1 + FargowiltasSouls.FargoSoulsWorld.MoonlordCount * .0125)), 0f, Main.myPlayer, pillar0);
                        }
                    }
                    else
                    {
                        masoBool[0] = !npc.dontTakeDamage;
                    }
                    break;

                case NPCID.MoonLordHand:
                case NPCID.MoonLordHead:
                    Aura(npc, 50, MasomodeEX.Souls.BuffType("Unstable"), false, 111);
                    if (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].Distance(npc.Center) < 50)
                        Main.player[Main.myPlayer].AddBuff(MasomodeEX.Souls.BuffType("Flipped"), 2);
                    if (npc.ai[0] == -2f) //eye socket is empty
                    {
                        if (npc.ai[1] == 0f //happens every 32 ticks
                            && Main.npc[(int)npc.ai[3]].ai[0] != 2f) //will stop when ML dies
                        {
                            Counter[0]++;
                            if (Counter[0] >= 29) //warning dust, reset timer
                            {
                                Counter[0] = 0;
                                int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                                if (t != -1)
                                {
                                    npc.localAI[0] = (Main.player[t].Center - npc.Center).ToRotation();
                                    Vector2 offset = Vector2.UnitX.RotatedBy(npc.localAI[0]) * 10f;
                                    for (int i = 0; i < 300; i++) //dust warning line for laser
                                    {
                                        int d = Dust.NewDust(npc.Center + offset * i, 1, 1, 111, 0f, 0f, 0, default(Color), 1.5f);
                                        Main.dust[d].noGravity = true;
                                        Main.dust[d].velocity *= 0.5f;
                                    }
                                }
                            }
                            if (Counter[0] == 2) //FIRE LASER
                            {
                                int t = npc.HasPlayerTarget ? npc.target : npc.FindClosestPlayer();
                                if (t != -1)
                                {
                                    if (Main.netMode != 1)
                                    {
                                        float newRotation = (Main.player[t].Center - npc.Center).ToRotation();
                                        float difference = newRotation - npc.localAI[0];
                                        const float PI = (float)Math.PI;
                                        float rotationDirection = PI / 3f / 120f; //positive is CW, negative is CCW
                                        if (difference < -PI)
                                            difference += 2f * PI;
                                        if (difference > PI)
                                            difference -= 2f * PI;
                                        if (difference < 0f)
                                            rotationDirection *= -1f;
                                        Vector2 speed = Vector2.UnitX.RotatedBy(npc.localAI[0]);
                                        int damage = 60;
                                        if (Main.netMode != 1)
                                            Projectile.NewProjectile(npc.Center, speed, MasomodeEX.Souls.ProjectileType("PhantasmalDeathrayML"),
                                                damage, 0f, Main.myPlayer, rotationDirection, npc.whoAmI);
                                    }
                                }
                            }
                            npc.netUpdate = true;
                        }
                    }
                    break;

                case NPCID.DemonEye:
                    npc.noTileCollide = true;
                    if (!masoBool[0])
                    {
                        masoBool[0] = true;
                        if (Main.rand.Next(4) == 0)
                            npc.Transform(NPCID.WanderingEye);
                    }
                    break;

                case NPCID.Clown:
                    if (npc.Distance(Main.player[Main.myPlayer].Center) > 500)
                    {
                        Main.player[Main.myPlayer].AddBuff(MasomodeEX.Souls.BuffType("Atrophied"), 2);
                        Main.player[Main.myPlayer].AddBuff(MasomodeEX.Souls.BuffType("Jammed"), 2);
                        Main.player[Main.myPlayer].AddBuff(MasomodeEX.Souls.BuffType("ReverseManaFlow"), 2);
                        Main.player[Main.myPlayer].AddBuff(MasomodeEX.Souls.BuffType("Antisocial"), 2);
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        int type = 127;
                        switch (Main.rand.Next(4))
                        {
                            case 0: break;
                            case 1: type = 229; break;
                            case 2: type = 242; break;
                            case 3: type = 135; break;
                        }
                        Vector2 offset = new Vector2();
                        double angle = Main.rand.NextDouble() * 2d * Math.PI;
                        offset.X += (float)(Math.Sin(angle) * 450);
                        offset.Y += (float)(Math.Cos(angle) * 450);
                        Dust dust = Main.dust[Dust.NewDust(
                            npc.Center + offset - new Vector2(4, 4), 0, 0,
                            type, 0, 0, 100, Color.White, 1f
                            )];
                        dust.velocity = npc.velocity;
                        if (Main.rand.Next(3) == 0)
                            dust.velocity += Vector2.Normalize(offset) * 5f;
                        dust.noGravity = true;
                    }
                    if (++Counter[0] > 1100)
                    {
                        npc.life = 0;
                        Main.PlaySound(npc.DeathSound, npc.Center);
                        npc.active = false;

                        bool bossAlive = false;
                        for (int i = 0; i < 200; i++)
                        {
                            if (Main.npc[i].boss)
                            {
                                bossAlive = true;
                                break;
                            }
                        }

                        if (Main.netMode != 1)
                        {
                            if (bossAlive)
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.BouncyBomb, 100, 8f, Main.myPlayer);
                            }
                            else
                            {
                                for (int i = 0; i < 100; i++)
                                {
                                    int type = ProjectileID.Bomb;
                                    int damage = 100;
                                    float knockback = 8f;
                                    switch (Main.rand.Next(6))
                                    {
                                        case 0: break;
                                        case 1: type = ProjectileID.StickyBomb; break;
                                        case 2: type = ProjectileID.BouncyBomb; break;
                                        case 3: type = ProjectileID.Grenade; damage = 60;  break;
                                        case 4: type = ProjectileID.StickyGrenade; damage = 60; break;
                                        case 5: type = ProjectileID.BouncyGrenade; damage = 60; break;
                                    }
                                    int p = Projectile.NewProjectile(npc.position.X + Main.rand.Next(npc.width), npc.position.Y + Main.rand.Next(npc.height), Main.rand.Next(-1000, 1001) / 50f, Main.rand.Next(-2000, 101) / 50f, type, damage, knockback, Main.myPlayer);
                                    Main.projectile[p].timeLeft += Main.rand.Next(180);
                                }
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            switch (npc.type)
            {
                case NPCID.SlimeSpiked:
                    npc.Transform(NPCID.KingSlime);
                    break;

                case NPCID.WanderingEye:
                    npc.Transform(NPCID.EyeofCthulhu);
                    break;

                case NPCID.Corruptor:
                    npc.Transform(NPCID.EaterofWorldsHead);
                    break;

                case NPCID.IchorSticker:
                    npc.Transform(NPCID.BrainofCthulhu);
                    break;

                case NPCID.PigronCorruption:
                case NPCID.PigronCrimson:
                case NPCID.PigronHallow:
                    npc.Transform(NPCID.DukeFishron);
                    break;

                case NPCID.EaterofSouls:
                    //if eater is alive, can vore player below 100 life
                    break;

                case NPCID.EaterofWorldsHead:
                    if (target.statLife + damage < 150)
                        target.KillMe(PlayerDeathReason.ByCustomReason(target.name + " was eaten alive by Eater of Worlds."), 999, 0);
                    break;

                case NPCID.Retinazer:
                case NPCID.Spazmatism:
                case NPCID.SkeletronPrime:
                case NPCID.PrimeCannon:
                case NPCID.PrimeLaser:
                case NPCID.PrimeSaw:
                case NPCID.PrimeVice:
                case NPCID.TheDestroyerBody:
                case NPCID.TheDestroyerTail:
                case NPCID.Probe:
                    target.AddBuff(MasomodeEX.Souls.BuffType("ClippedWings"), 2);
                    break;

                case NPCID.TheDestroyer:
                    target.AddBuff(MasomodeEX.Souls.BuffType("ClippedWings"), 2);
                    if (target.statLife + damage < 300)
                        target.KillMe(PlayerDeathReason.ByCustomReason(target.name + " was eaten alive by The Destroyer."), 999, 0);
                    break;

                case NPCID.Nutcracker:
                case NPCID.NutcrackerSpinning:
                    if (target.Male)
                        target.KillMe(PlayerDeathReason.ByCustomReason(target.name + " got his nuts cracked."), 9999, 0);
                    break;

                default:
                    break;
            }
        }

        public void ModifyHitByEither(NPC npc, ref int damage)
        {
            switch (npc.type)
            {
                case NPCID.BrainofCthulhu:
                    if (Main.netMode != 1 && Main.rand.Next(2) == 0 & npc.HasPlayerTarget)
                    {
                        Vector2 distance = Main.player[npc.target].Center - npc.Center;
                        if (Main.rand.Next(2) == 0)
                            distance.X *= -1f;
                        if (Main.rand.Next(2) == 0)
                            distance.Y *= -1f;
                        npc.Center = Main.player[npc.target].Center + distance;
                        npc.netUpdate = true;
                    }
                    break;

                case NPCID.QueenBee:
                    if (masoBool[2])
                        damage = 0;
                    break;

                default:
                    break;
            }
        }

        public override bool CheckDead(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Corruptor:
                    if (Main.netMode != 1)
                        for (int i = 0; i < 8; i++)
                            Projectile.NewProjectile(npc.Center, Vector2.UnitY.RotatedBy(2 * Math.PI / 8 * i) * 4f, ProjectileID.CorruptSpray, 0, 0f, Main.myPlayer, 8f);
                    break;

                case NPCID.IchorSticker:
                    if (Main.netMode != 1)
                        for (int i = 0; i < 8; i++)
                            Projectile.NewProjectile(npc.Center, Vector2.UnitY.RotatedBy(2 * Math.PI / 8 * i) * 4f, ProjectileID.CrimsonSpray, 0, 0f, Main.myPlayer, 8f);
                    break;

                case NPCID.MeteorHead:
                    if (!NPC.downedBoss2)
                    {
                        Main.PlaySound(npc.DeathSound, npc.Center);
                        npc.active = false;
                        return false;
                    }
                    break;

                default:
                    break;
            }
            if (npc.townNPC)
            {
                if (npc.type == MasomodeEX.Fargo.NPCType("Abominationn"))
                {
                    int mutant = NPC.FindFirstNPC(MasomodeEX.Fargo.NPCType("Mutant"));
                    if (mutant > -1 && Main.npc[mutant].active)
                    {
                        Main.npc[mutant].Transform(MasomodeEX.Souls.NPCType("MutantBoss"));
                        if (Main.netMode == 0)
                            Main.NewText("Mutant has been enraged by the death of his brother!", 175, 75, 255);
                        else if (Main.netMode == 2)
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Mutant has been enraged by the death of his brother!"), new Color(175, 75, 255));
                    }
                }
                else if (npc.type == MasomodeEX.Fargo.NPCType("MutantBoss"))
                {
                    npc.active = true;
                    npc.life = 1;
                    npc.Transform(MasomodeEX.Souls.NPCType("MutantBoss"));
                    if (Main.netMode == 0)
                        Main.NewText("Mutant has awoken!", 175, 75, 255);
                    else if (Main.netMode == 2)
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Mutant has awoken!"), new Color(175, 75, 255));
                    return false;
                }
            }
            return true;
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            ModifyHitByEither(npc, ref damage);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            ModifyHitByEither(npc, ref damage);

            if (npc.aiStyle == 37)
            {
                if (projectile.penetrate > 0)
                    damage /= projectile.penetrate;
                else if (projectile.penetrate < 0)
                    damage /= 5;
            }
        }

        private void Aura(NPC npc, float distance, int buff, bool reverse = false, int dustid = DustID.GoldFlame)
        {
            //works because buffs are client side anyway :ech:
            Player p = Main.player[Main.myPlayer];
            float range = npc.Distance(p.Center);
            if (reverse ? range > distance && range < 3000f : range < distance)
                p.AddBuff(buff, 2);

            for (int i = 0; i < 20; i++)
            {
                Vector2 offset = new Vector2();
                double angle = Main.rand.NextDouble() * 2d * Math.PI;
                offset.X += (float)(Math.Sin(angle) * distance);
                offset.Y += (float)(Math.Cos(angle) * distance);
                Dust dust = Main.dust[Dust.NewDust(
                    npc.Center + offset - new Vector2(4, 4), 0, 0,
                    dustid, 0, 0, 100, Color.White, 1f
                    )];
                dust.velocity = npc.velocity;
                if (Main.rand.Next(3) == 0)
                    dust.velocity += Vector2.Normalize(offset) * (reverse ? 5f : -5f);
                dust.noGravity = true;
            }
        }

        public static void PrintAI(NPC npc)
        {
            Main.NewText("ai: " + npc.ai[0].ToString() + " " + npc.ai[1].ToString() + " " + npc.ai[2].ToString() + " " + npc.ai[3].ToString()
                + ", local: " + npc.localAI[0].ToString() + " " + npc.localAI[1].ToString() + " " + npc.localAI[2].ToString() + " " + npc.localAI[2].ToString());
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            spawnRate /= 2;
        }

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            if (npc.type == NPCID.Nurse && firstButton) //tried to heal
                for (int i = 0; i < 200; i++)
                    if (Main.npc[i].active && Main.npc[i].boss) //during boss fight
                    {
                        npc.life = 0;
                        npc.checkDead();
                        npc.active = false;
                        Main.PlaySound(15, Main.player[Main.myPlayer].Center, 0);
                        for (int j = 0; j < 10; j++)
                            NPC.SpawnOnPlayer(Main.myPlayer, NPCID.DungeonGuardian);
                        break;
                    }
        }
    }
}