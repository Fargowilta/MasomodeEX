using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

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
        public bool FirstTick;

        public override void SetDefaults(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.MoonLordLeechBlob:
                    npc.damage = 50;
                    break;

                default:
                    break;
            }

            if (!npc.friendly)
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.5);
                //npc.damage = (int)(npc.damage * 1.5);
                npc.defense = (int)(npc.defense * 1.5);
                npc.knockBackResist *= 0.5f;
            }

            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Suffocation] = true;
            npc.GetGlobalNPC<FargowiltasSouls.NPCs.EModeGlobalNPC>().isWaterEnemy = true;
        }

        public override bool PreAI(NPC npc)
        {
            FargowiltasSouls.NPCs.EModeGlobalNPC fargoNPC = npc.GetGlobalNPC<FargowiltasSouls.NPCs.EModeGlobalNPC>();
            if (fargoNPC.RegenTimer > 240)
                fargoNPC.RegenTimer = 240;

            if (npc.townNPC && Main.bloodMoon)
            {
                if (++Counter[0] > 15)
                {
                    Counter[0] = 0;
                    int p = Player.FindClosest(npc.Center, 0, 0);
                    if (p > -1 && Main.player[p].active && npc.Distance(Main.player[p].Center) < 500)
                    {
                        switch (npc.type)
                        {
                            case NPCID.Dryad:
                                npc.Transform(NPCID.Nymph);
                                break;

                            case NPCID.Pirate:
                                npc.Transform(NPCID.PirateCaptain);
                                break;

                            case NPCID.TaxCollector:
                                npc.Transform(NPCID.DemonTaxCollector);
                                break;

                            case NPCID.WitchDoctor:
                                npc.Transform(NPCID.Lihzahrd);
                                break;

                            case NPCID.Clothier:
                                npc.Transform(NPCID.SkeletronHead);
                                break;

                            default:
                                if (npc.type == MasomodeEX.Fargo.NPCType("Mutant"))
                                    npc.Transform(MasomodeEX.Souls.NPCType("MutantBoss"));
                                else if (npc.type != MasomodeEX.Fargo.NPCType("Abominationn") && npc.type != MasomodeEX.Fargo.NPCType("Deviantt"))
                                    npc.Transform(NPCID.Werewolf);
                                break;
                        }
                    }
                }
            }

            if (!FirstTick)
            {
                FirstTick = true;
                if ((npc.boss || npc.type == NPCID.EaterofWorldsHead) && Main.netMode != 1)
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("Arena"), npc.damage / 4, 0f, Main.myPlayer, 0f, npc.whoAmI);
            }

            switch (npc.type)
            {
                case NPCID.IceGolem:
                    if (npc.HasPlayerTarget && npc.Distance(Main.player[npc.target].Center) > 1000 && npc.Distance(Main.player[npc.target].Center) < 3000)
                    {
                        Main.player[npc.target].position += npc.DirectionFrom(Main.player[npc.target].Center) * 20;
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        const float distance = 1000;
                        Vector2 offset = new Vector2();
                        double angle = Main.rand.NextDouble() * 2d * Math.PI;
                        offset.X += (float)(Math.Sin(angle) * distance);
                        offset.Y += (float)(Math.Cos(angle) * distance);
                        Dust dust = Main.dust[Dust.NewDust(
                            npc.Center + offset - new Vector2(4, 4), 0, 0,
                            DustID.Ice, 0, 0, 100, Color.White, 1f
                            )];
                        dust.velocity = npc.velocity;
                        if (Main.rand.Next(3) == 0)
                            dust.velocity += Vector2.Normalize(offset) * 5f;
                        dust.noGravity = true;
                    }
                    if (npc.life > 0)
                    {
                        int cap = npc.lifeMax / npc.life;
                        Counter[0] += Main.rand.Next(2 + cap) + 1;
                        if (Counter[0] >= Main.rand.Next(1400, 26000))
                        {
                            Counter[0] = 0;
                            if (Main.netMode != 1 && npc.HasPlayerTarget) //shoot a laser
                            {
                                double num2 = 15.0;
                                Vector2 vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + 20f);
                                vector2.X += (float)(10 * npc.direction);
                                float num3 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector2.X;
                                float num4 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector2.Y;
                                float num5 = num3 + (float)Main.rand.Next(-40, 41);
                                float num6 = num4 + (float)Main.rand.Next(-40, 41);
                                float num7 = (float)Math.Sqrt((double)num5 * (double)num5 + (double)num6 * (double)num6);
                                double num8 = (double)num7;
                                float num9 = (float)(num2 / num8);
                                float SpeedX = num5 * num9;
                                float SpeedY = num6 * num9;
                                int Damage = 32;
                                int Type = 257;
                                vector2.X += SpeedX * 3f;
                                vector2.Y += SpeedY * 3f;
                                Projectile.NewProjectile(vector2.X, vector2.Y, SpeedX, SpeedY, Type, Damage, 0.0f, Main.myPlayer, 0.0f, 0.0f);
                            }
                        }
                    }
                    if (++Counter[1] > 240)
                    {
                        Counter[1] = 0;
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, Vector2.UnitY, mod.ProjectileType("PhantasmalDeathrayIce"), npc.damage / 4, 0f, Main.myPlayer, 2f * (float)Math.PI * 1f / 4f / 60f, npc.whoAmI);
                    }
                    break;

                case NPCID.FaceMonster:
                    Aura(npc, 600, BuffID.Blackout, true, 199);
                    Aura(npc, 600, BuffID.Darkness, true, 199);
                    break;
                
                case NPCID.DD2EterniaCrystal:
                    if (npc.lifeMax > 100)
                        npc.lifeMax = 100;
                    if (npc.life > npc.lifeMax)
                        npc.life = npc.lifeMax;
                    break;

                case NPCID.DD2OgreT2:
                case NPCID.DD2OgreT3:
                    Aura(npc, 500, BuffID.OgreSpit, true, 188);
                    break;

                case NPCID.DD2DarkMageT1:
                    Aura(npc, 900, MasomodeEX.Souls.BuffType("Hexed"), true, 254);
                    break;

                case NPCID.DD2DarkMageT3:
                    Aura(npc, 600, MasomodeEX.Souls.BuffType("Hexed"), true, 254);
                    break;

                case NPCID.Medusa:
                    Aura(npc, 400, BuffID.Stoned, false, DustID.Stone);
                    break;

                case NPCID.Harpy:
                    npc.noTileCollide = true;
                    break;

                case NPCID.LunarTowerNebula:
                case NPCID.LunarTowerSolar:
                case NPCID.LunarTowerStardust:
                case NPCID.LunarTowerVortex:
                    if (npc.position.Y < Main.maxTilesY * 16 / 2)
                        npc.position.Y += 16f / 60f; //sink one block per second
                    break;

                case NPCID.BloodJelly:
                case NPCID.BlueJellyfish:
                case NPCID.GreenJellyfish:
                case NPCID.PinkJellyfish:
                case NPCID.FungoFish:
                    Aura(npc, 200, BuffID.Electrified, false, DustID.Electric);
                    break;

                case NPCID.Werewolf:
                case NPCID.GoblinWarrior:
                case NPCID.PirateCaptain:
                case NPCID.Krampus:
                case NPCID.UndeadMiner:
                    npc.position.X += npc.velocity.X;
                    if (npc.velocity.Y < 0)
                        npc.position.Y += npc.velocity.Y;
                    break;

                case NPCID.Lihzahrd:
                case NPCID.LihzahrdCrawler:
                case NPCID.FlyingSnake:
                    if (!NPC.downedPlantBoss)
                    {
                        Main.PlaySound(15, npc.Center, 0);
                        npc.Transform(MasomodeEX.Souls.NPCType("MutantBoss"));
                    }
                    break;

                case NPCID.Pumpking:
                    Aura(npc, 300, MasomodeEX.Souls.BuffType("MarkedforDeath"));
                    break;

                case NPCID.PumpkingBlade:
                    Aura(npc, 200, MasomodeEX.Souls.BuffType("LivingWasteland"));
                    break;

                case NPCID.IceQueen:
                    Aura(npc, 300, BuffID.Frozen);
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
                            dust.velocity += Vector2.Normalize(offset) * 5f;
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
                    Aura(npc, 600, BuffID.Obstructed, true, DustID.ToxicBubble);
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
                    Aura(npc, 250, MasomodeEX.Souls.BuffType("Shadowflame"), false, DustID.Shadowflame);

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
                                        WorldGen.KillTile(x, y);
                                        if (Main.netMode == 2)
                                            NetMessage.SendData(17, -1, -1, null, 0, x, y);
                                    }
                                }
                            }
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
                    Aura(npc, 600, MasomodeEX.Souls.BuffType("Lethargic"), true, 60);
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
                    if (++Counter[1] > 360)
                    {
                        Counter[1] = 0;
                        if (Main.netMode != 1)
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.UndeadMiner);
                            if (n < 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }
                    /*if (++Counter[2] > 2)
                    {
                        Counter[2] = 0;
                        if (Main.netMode != 1)
                        {
                            Vector2 speed = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                            speed.Normalize();
                            speed *= 6f;
                            speed += npc.velocity * 1.25f;
                            speed.Y -= Math.Abs(speed.X) * 0.2f;
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center, speed, MasomodeEX.Souls.ProjectileType("SkeletronBone"), npc.damage / 4, 0f, Main.myPlayer);
                        }
                    }*/
                    npc.localAI[2]++;
                    npc.reflectingProjectiles = npc.ai[1] == 1f || npc.ai[1] == 2f; //spinning or DG mode
                    break;

                case NPCID.SkeletronHand:
                    Aura(npc, 140, BuffID.Dazed, false);
                    fargoNPC.Counter--;
                    fargoNPC.Counter2--;
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

                case NPCID.WallofFlesh:
                    if (Main.netMode != 1)
                    {
                        int x = (int)npc.Center.X / 16;
                        int startY = (int)npc.Center.Y / 16 + ((int)npc.Center.Y / 16 - Main.maxTilesY);
                        for (int y = startY; y < Main.maxTilesY; y++)
                            WorldGen.PlaceWall(x, y, WallID.Flesh);
                        float velX = Math.Abs(npc.velocity.X);
                        if (velX > 16)
                            velX += 16; //make sure it closes the gap with the last check
                        while (velX > 16)
                        {
                            x += npc.velocity.X > 0 ? -1 : 1;
                            for (int y = startY; y < Main.maxTilesY; y++)
                                WorldGen.PlaceWall(x, y, WallID.Flesh);
                            velX -= 16;
                        }
                    }
                    fargoNPC.masoBool[0] = true;
                    fargoNPC.Counter++;
                    Main.player[Main.myPlayer].AddBuff(MasomodeEX.Souls.BuffType("LowGround"), 2);
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
                    if (++Counter[1] > 150)
                    {
                        Counter[1] = 0;
                        if (Main.netMode != 1)
                        {
                            const float retiRad = 500;
                            float retiSpeed = 2 * (float)Math.PI * retiRad / 240;
                            float retiAcc = retiSpeed * retiSpeed / retiRad * 1;
                            for (int i = 0; i < 4; i++)
                                Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(Math.PI / 2 * i) * retiSpeed, MasomodeEX.Souls.ProjectileType("MutantRetirang"), npc.damage / 4, 0f, Main.myPlayer, retiAcc, 240);
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
                    if (++Counter[1] > 150)
                    {
                        Counter[1] = 0;
                        if (Main.netMode != 1)
                        {
                            const float spazRad = 250;
                            float spazSpeed = 2 * (float)Math.PI * spazRad / 120;
                            float spazAcc = spazSpeed * spazSpeed / spazRad * -1;
                            for (int i = 0; i < 4; i++)
                                Projectile.NewProjectile(npc.Center, Vector2.UnitX.RotatedBy(Math.PI / 2 * i + Math.PI / 4) * spazSpeed, MasomodeEX.Souls.ProjectileType("MutantSpazmarang"), npc.damage / 4, 0f, Main.myPlayer, spazAcc, 120);
                        }
                    }
                    break;

                case NPCID.TheDestroyer:
                    if (++Counter[0] > 240)
                    {
                        fargoNPC.masoBool[0] = true;
                        Counter[0] = 0;
                        Main.PlaySound(4, (int)npc.Center.X, (int)npc.Center.Y, 13);
                        if (npc.HasPlayerTarget && Main.netMode != 1) //spawn worm
                        {
                            Vector2 vel = npc.DirectionTo(Main.player[npc.target].Center) * 15f;
                            int current = Projectile.NewProjectile(npc.Center, vel, MasomodeEX.Souls.ProjectileType("MutantDestroyerHead"), npc.damage / 4, 0f, Main.myPlayer, npc.target);
                            for (int i = 0; i < 18; i++)
                                current = Projectile.NewProjectile(npc.Center, vel, MasomodeEX.Souls.ProjectileType("MutantDestroyerBody"), npc.damage / 4, 0f, Main.myPlayer, current);
                            int previous = current;
                            current = Projectile.NewProjectile(npc.Center, vel, MasomodeEX.Souls.ProjectileType("MutantDestroyerTail"), npc.damage / 4, 0f, Main.myPlayer, current);
                            Main.projectile[previous].localAI[1] = current;
                            Main.projectile[previous].netUpdate = true;
                        }
                    }
                    if (fargoNPC.masoBool[0])
                    {
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
                                        WorldGen.KillTile(x, y);
                                        if (Main.netMode == 2)
                                            NetMessage.SendData(17, -1, -1, null, 0, x, y);
                                    }
                                }
                            }
                        }
                    }
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
                    if (!masoBool[0])
                    {
                        masoBool[0] = true;
                        if (Main.netMode != 1) //spawn four more limbs at start
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeLaser, npc.whoAmI, -1f, npc.whoAmI, 0f, 150f, npc.target);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeSaw, npc.whoAmI, -1f, npc.whoAmI, 0f, 0f, npc.target);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeCannon, npc.whoAmI, 1f, npc.whoAmI, 0f, 150f, npc.target);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeVice, npc.whoAmI, 1f, npc.whoAmI, 0f, 0f, npc.target);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }
                    if (!masoBool[1] && npc.ai[0] == 2f && ++Counter[0] > 300)
                    {
                        masoBool[1] = true;
                        if (Main.netMode != 1) //spawn four MORE limbs in phase 2
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeLaser, npc.whoAmI, -1f, npc.whoAmI, 0f, 150f, npc.target);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeSaw, npc.whoAmI, -1f, npc.whoAmI, 0f, 0f, npc.target);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeCannon, npc.whoAmI, 1f, npc.whoAmI, 0f, 150f, npc.target);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeVice, npc.whoAmI, 1f, npc.whoAmI, 0f, 0f, npc.target);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                        npc.ai[3] = -2;
                        npc.netUpdate = true;
                    }
                    if (++Counter[1] > 120)
                    {
                        Counter[1] = 0;
                        if (Main.netMode != 1 && npc.HasPlayerTarget)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 18f, MasomodeEX.Souls.ProjectileType("MutantGuardian"), npc.damage / 3, 0f, Main.myPlayer);
                    }
                    break;

                case NPCID.PrimeLaser:
                case NPCID.PrimeSaw:
                case NPCID.PrimeCannon:
                case NPCID.PrimeVice:
                    if (!masoBool[0])
                    {
                        masoBool[0] = true;
                        if (Main.npc[(int)npc.ai[1]].type == NPCID.SkeletronPrime && Main.npc[(int)npc.ai[1]].ai[0] == 2f && Main.npc[(int)npc.ai[1]].ai[3] == -1f)
                            masoBool[1] = true;
                    }
                    if (masoBool[1] && !MasomodeEX.Instance.HyperLoaded)
                    {
                        masoBool[2] = !masoBool[2];
                        if (masoBool[2])
                        {
                            npc.position += npc.velocity * 1.5f;
                            npc.AI();
                        }
                    }
                    if (++Counter[0] > 240)
                    {
                        Counter[0] = 0;
                        if (Main.netMode != 1 && npc.HasPlayerTarget)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 14, MasomodeEX.Souls.ProjectileType("DarkStar"), npc.damage / 4, 0f, Main.myPlayer);
                    }
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
                    if (npc.life < npc.lifeMax / 2)
                    {
                        if (++Counter[1] > 300)
                        {
                            Counter[1] = 0;
                            if (Main.netMode != 1)
                            {
                                int tentaclesToSpawn = 6;
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
                        if (--Counter[2] < 0)
                        {
                            Counter[2] = 180;
                            if (Main.netMode != 1)
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, MasomodeEX.Souls.ProjectileType("MutantMark2"), npc.damage / 4, 0f, Main.myPlayer, 30, 30 + 120);
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
                        npc.position.X += npc.velocity.X * 0.5f;
                        if (npc.velocity.Y < 0)
                        {
                            npc.position.Y += npc.velocity.Y * 0.5f;
                            if (npc.velocity.Y > -2)
                                npc.velocity.Y = 20;
                        }
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
                    npc.position += npc.velocity;
                    if (--Counter[0] < 0)
                    {
                        Counter[0] = 300;
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, Vector2.UnitY * -3f, MasomodeEX.Souls.ProjectileType("MutantFishron"), npc.damage / 4, 0f, Main.myPlayer, npc.target);
                    }
                    if (fargoNPC.masoBool[3] && --Counter[1] < 0)
                    {
                        Counter[1] = 240;
                        if (Main.netMode != 1 && npc.HasPlayerTarget)
                            Projectile.NewProjectile(Main.player[npc.target].Center + Vector2.UnitY * -500, Vector2.UnitY * -3f, MasomodeEX.Souls.ProjectileType("MutantFishron"), npc.damage / 4, 0f, Main.myPlayer, npc.target);
                    }
                    /*if (!MasomodeEX.Instance.HyperLoaded)
                    {
                        masoBool[0] = !masoBool[0];
                        if (masoBool[0])
                        {
                            npc.position += npc.velocity * 1.5f;
                            npc.AI();
                        }
                    }*/
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

                case NPCID.MoonLordLeechBlob:
                    if (!masoBool[0])
                    {
                        masoBool[0] = true;
                        Main.NewText("WHAT ARE YOU DOING TO ME!?! Healing Leech Clots, Recover me at once!", Color.LimeGreen);
                    }
                    break;

                case NPCID.MoonLordCore:
                    if (Main.LocalPlayer.active && Main.LocalPlayer.mount.Active)
                        Main.LocalPlayer.mount.Dismount(Main.LocalPlayer);
                    if (!masoBool[0])
                    {
                        npc.TargetClosest(false);
                        masoBool[0] = true;
                        if (NPC.CountNPCS(NPCID.MoonLordCore) == 1)
                            Main.LocalPlayer.GetModPlayer<MasomodeEXPlayer>().hitcap = 25;
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("MoonLordText"), 0, 0f, Main.myPlayer, npc.whoAmI);
                    }
                    if (!masoBool[1] && npc.ai[0] == 2)
                    {
                        masoBool[1] = true;
                        npc.ai[0] = 1;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                        npc.life = npc.lifeMax;
                        npc.netUpdate = true;
                        Main.NewText("Hehehehehehe! I GUARANTEE YOU DON'T HAVE ENOUGH POWER TO DEFEAT ME!!!", Color.LimeGreen);
                        return false;
                    }
                    if (++Counter[0] > 90)
                    {
                        Counter[0] = 0;
                        if (npc.HasPlayerTarget && Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 8, ProjectileID.PhantasmalBolt, 30, 0f, Main.myPlayer);
                    }
                    npc.position += npc.velocity * (1f - (float)npc.life / npc.lifeMax);
                    if (masoBool[1])
                        npc.position += npc.velocity;
                    break;

                case NPCID.MoonLordFreeEye:
                    if (!masoBool[0])
                    {
                        masoBool[0] = true;
                        string text;
                        switch (Main.rand.Next(5))
                        {
                            case 0: text = "Guys, Will you assist me for a moment?"; break;
                            case 1: text = "I'm going to bring more of my servants to assist me!"; break;
                            case 2: text = "Go servants, what are you all waiting for?"; break;
                            case 3: text = "Servants! Get this little peasant out of my sight at once!"; break;
                            default: text = "Mwahahahahahahahahaha! Trap this tiny struggle at once!"; break;
                        }
                        Main.NewText(text, Color.LimeGreen);
                        if (NPC.CountNPCS(NPCID.MoonLordFreeEye) >= 3)
                            Main.NewText("Ahhhh, My eyes!", Color.LimeGreen);
                    }
                    if (++Counter[0] > 90)
                    {
                        Counter[0] = 0;
                        if (npc.HasPlayerTarget && Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 8, ProjectileID.PhantasmalBolt, 30, 0f, Main.myPlayer);
                    }
                    break;

                case NPCID.MoonLordHand:
                    if (!masoBool[0])
                    {
                        masoBool[0] = true;
                        if (Main.netMode != 1)
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, npc.ai[2] == 0 ? mod.NPCType("PhantomPortal") : mod.NPCType("PurityPortal"), npc.whoAmI, npc.whoAmI);
                            if (n != Main.maxNPCs && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }
                    if (npc.dontTakeDamage)
                    {
                        if (masoBool[1])
                        {
                            masoBool[1] = false;
                            string text;
                            switch (Main.rand.Next(3))
                            {
                                case 0: text = "It looks like you can't defeat me!"; break;
                                case 1: text = "I see that you have annihilated my son!,You wimp!"; break;
                                default: text = "I still have plenty of gimmicks and tricks left!"; break;
                            }
                            Main.NewText(text, Color.LimeGreen);
                        }
                    }
                    else
                    {
                        masoBool[1] = true;
                    }
                    break;

                case NPCID.MoonLordHead:
                    if (!masoBool[0])
                    {
                        masoBool[0] = true;
                        if (Main.netMode != 1)
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("MoonLordEyeL"), npc.whoAmI, npc.whoAmI);
                            if (n != Main.maxNPCs && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                            n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("MoonLordEyeR"), npc.whoAmI, npc.whoAmI);
                            if (n != Main.maxNPCs && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }
                    if (npc.dontTakeDamage)
                    {
                        if (masoBool[1])
                        {
                            masoBool[1] = false;
                            string text;
                            switch (Main.rand.Next(3))
                            {
                                case 0: text = "It looks like you can't defeat me!"; break;
                                case 1: text = "I see that you have annihilated my son!,You wimp!"; break;
                                default: text = "I still have plenty of gimmicks and tricks left!"; break;
                            }
                            Main.NewText(text, Color.LimeGreen);
                        }
                    }
                    else
                    {
                        masoBool[1] = true;
                    }
                    break;

                /*case NPCID.MoonLordCore:
                    Aura(npc, 100, MasomodeEX.Souls.BuffType("GodEater"), false, 86);
                    Aura(npc, 100, MasomodeEX.Souls.BuffType("Unstable"), false, 111);
                    if (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].Distance(npc.Center) < 100)
                    {
                        int d = Main.rand.Next(MasomodeEX.DebuffIDs.Count);
                        Main.player[Main.myPlayer].AddBuff(MasomodeEX.DebuffIDs[d], Main.rand.Next(60, 600));
                    }
                    fargoNPC.Counter++;
                    fargoNPC.Timer++;
                    if (masoBool[0])
                    {
                        if (--Counter[0] < 0)
                        {
                            Counter[0] = 600;
                            int pillar0 = Main.rand.Next(4);
                            switch(FargowiltasSouls.NPCs.EModeGlobalNPC.masoStateML)
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
                        if (--Counter[1] < 0)
                        {
                            Counter[1] = 180;
                            if (Main.netMode != 1)
                            {
                                const int max = 8;
                                const float speed = 10f;
                                int damage = (int)(50 * (1 + FargowiltasSouls.FargoSoulsWorld.MoonlordCount * .0125));
                                const float rotationModifier = 0.75f;
                                float rotation = 2f * (float)Math.PI / max;
                                Vector2 vel = Vector2.UnitY * speed;
                                int type = MasomodeEX.Souls.ProjectileType("MutantSphereRing");
                                for (int i = 0; i < max; i++)
                                {
                                    vel = vel.RotatedBy(rotation);
                                    Projectile.NewProjectile(npc.Center, vel, type, damage, 0f, Main.myPlayer, rotationModifier * npc.spriteDirection, speed);
                                    Projectile.NewProjectile(npc.Center, vel, type, damage, 0f, Main.myPlayer, -rotationModifier * npc.spriteDirection, speed);
                                }
                                Main.PlaySound(SoundID.Item84, npc.Center);
                            }
                        }
                    }
                    else
                    {
                        masoBool[0] = !npc.dontTakeDamage;
                        if (--Counter[2] < 0)
                        {
                            Counter[2] = 360;
                            if (Main.netMode != 1 && npc.HasPlayerTarget)
                            {
                                int damage = (int)(50 * (1 + FargowiltasSouls.FargoSoulsWorld.MoonlordCount * .0125));
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, MasomodeEX.Souls.ProjectileType("MutantTrueEyeL"), damage, 0f, Main.myPlayer, npc.target);
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, MasomodeEX.Souls.ProjectileType("MutantTrueEyeR"), damage, 0f, Main.myPlayer, npc.target);
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, MasomodeEX.Souls.ProjectileType("MutantTrueEyeS"), damage, 0f, Main.myPlayer, npc.target);
                            }
                        }
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

                case NPCID.MoonLordFreeEye:
                    Aura(npc, 150, MasomodeEX.Souls.BuffType("Unstable"), false, 111);
                    if (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].Distance(npc.Center) < 150)
                        Main.player[Main.myPlayer].AddBuff(MasomodeEX.Souls.BuffType("Flipped"), 2);
                    break;*/

                case NPCID.DD2Betsy:
                    Aura(npc, 700, BuffID.OnFire, true, DustID.Fire);
                    Aura(npc, 700, BuffID.Burning, true);
                    if (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].Distance(npc.Center) < 700)
                    {
                        Main.player[Main.myPlayer].AddBuff(BuffID.WitheredArmor, 2);
                        Main.player[Main.myPlayer].AddBuff(BuffID.WitheredWeapon, 2);
                    }
                    break;

                case NPCID.DemonEye:
                    //npc.noTileCollide = true;
                    if (!masoBool[0])
                    {
                        masoBool[0] = true;
                        if (Main.rand.Next(4) == 0)
                            npc.Transform(NPCID.WanderingEye);
                    }
                    break;

                case NPCID.Mothron:
                case NPCID.MothronSpawn:
                    for (int i = 0; i < 2; i++)
                    {
                        int d = Dust.NewDust(npc.position, npc.width, npc.height, 70);
                        Main.dust[d].scale += 1f;
                        Main.dust[d].noGravity = true;
                        Main.dust[d].velocity *= 5f;
                    }
                    if (++Counter[0] > 6)
                    {
                        Counter[0] = 0;
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, Main.rand.NextVector2Unit() * 12f,
                                mod.ProjectileType("MothDust"), npc.damage / 5, 0f, Main.myPlayer);
                    }
                    break;

                case NPCID.Paladin:
                    npc.reflectingProjectiles = true;
                    break;

                case NPCID.WanderingEye:
                    npc.noTileCollide = true;
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
                            if (Main.npc[i].boss && Main.npc[i].active)
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

                case NPCID.PirateShip:
                    if (++Counter[0] > 300)
                    {
                        Counter[0] = 0;
                        if (Main.netMode != 1)
                        {
                            Vector2 speed = Main.player[npc.target].Center - npc.Center;
                            speed.Y -= Math.Abs(speed.X) * 0.2f; //account for gravity
                            speed.X += Main.rand.Next(-20, 21);
                            speed.Y += Main.rand.Next(-20, 21);
                            speed.Normalize();
                            speed *= 11f;
                            npc.localAI[2] = 0f;
                            for (int i = 0; i < 15; i++)
                            {
                                Vector2 cannonSpeed = speed;
                                cannonSpeed.X += Main.rand.Next(-10, 11) * 0.3f;
                                cannonSpeed.Y += Main.rand.Next(-10, 11) * 0.3f;
                                Projectile.NewProjectile(npc.Center, cannonSpeed, ProjectileID.CannonballHostile, Main.expertMode ? 80 : 100, 0f, Main.myPlayer);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            return true;
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            if (npc.type != NPCID.DemonEye && npc.type != NPCID.DemonEyeOwl && npc.type != NPCID.DemonEyeSpaceship)
                target.AddBuff(BuffID.Stoned, 15);

            switch (npc.type)
            {
                case NPCID.Harpy:
                    if (target.whoAmI == Main.myPlayer && !target.GetModPlayer<FargowiltasSouls.FargoPlayer>().SecurityWallet)
                    {
                        if (!StealFromInventory(target, ref Main.mouseItem))
                            StealFromInventory(target, ref target.inventory[target.selectedItem]);

                        byte extraTries = 30;
                        bool successfulSteal = StealFromInventory(target, ref target.inventory[Main.rand.Next(target.inventory.Length)]);
                        while (!successfulSteal && extraTries > 0)
                        {
                            extraTries--;
                            successfulSteal = StealFromInventory(target, ref target.inventory[Main.rand.Next(target.inventory.Length)]);
                        }
                    }
                    break;

                case NPCID.SolarCrawltipedeHead:
                case NPCID.SolarCrawltipedeBody:
                case NPCID.SolarCrawltipedeTail:
                case NPCID.VortexHornet:
                case NPCID.NebulaHeadcrab:
                    target.AddBuff(BuffID.VortexDebuff, Main.rand.Next(300));
                    break;

                case NPCID.SolarSolenian:
                case NPCID.SolarCorite:
                    target.AddBuff(MasomodeEX.Souls.BuffType("ClippedWings"), Main.rand.Next(300));
                    target.AddBuff(MasomodeEX.Souls.BuffType("Flipped"), Main.rand.Next(300));
                    break;

                case NPCID.BlueSlime:
                    if (npc.netID == NPCID.Pinky)
                    {
                        target.AddBuff(MasomodeEX.Souls.BuffType("Stunned"), 60);
                        target.velocity.X = target.Center.X < npc.Center.X ? -500f : 500f;
                        target.velocity.Y = -100f;
                    }
                    break;

                case NPCID.SlimeSpiked:
                    npc.Transform(NPCID.KingSlime);
                    break;

                case NPCID.DemonEye:
                    target.AddBuff(BuffID.Stoned, Main.rand.Next(300));
                    target.AddBuff(MasomodeEX.Souls.BuffType("Hexed"), Main.rand.Next(300));
                    npc.Transform(NPCID.WanderingEye);
                    break;

                case NPCID.WanderingEye:
                    target.AddBuff(MasomodeEX.Souls.BuffType("Hexed"), Main.rand.Next(300));
                    npc.Transform(NPCID.EyeofCthulhu);
                    break;

                case NPCID.Corruptor:
                    npc.Transform(NPCID.EaterofWorldsHead);
                    target.AddBuff(BuffID.CursedInferno, Main.rand.Next(60, 600));
                    target.AddBuff(BuffID.Weak, Main.rand.Next(7200));
                    target.AddBuff(MasomodeEX.Souls.BuffType("Shadowflame"), Main.rand.Next(60, 600));
                    break;

                case NPCID.IchorSticker:
                    npc.Transform(NPCID.BrainofCthulhu);
                    target.AddBuff(BuffID.Ichor, Main.rand.Next(60, 600));
                    target.AddBuff(BuffID.Bleeding, Main.rand.Next(7200));
                    target.AddBuff(MasomodeEX.Souls.BuffType("Bloodthirsty"), Main.rand.Next(300));
                    break;

                case NPCID.PigronCorruption:
                case NPCID.PigronCrimson:
                case NPCID.PigronHallow:
                    npc.Transform(NPCID.DukeFishron);
                    break;

                case NPCID.EaterofSouls:
                    target.AddBuff(BuffID.CursedInferno, Main.rand.Next(60, 600));
                    target.AddBuff(BuffID.Weak, Main.rand.Next(7200));
                    target.AddBuff(MasomodeEX.Souls.BuffType("Shadowflame"), Main.rand.Next(60, 600));
                    if (target.statLife + damage < 100 && NPC.AnyNPCs(NPCID.EaterofWorldsHead))
                        target.KillMe(PlayerDeathReason.ByCustomReason(target.name + " was eaten alive by Eater of Souls."), 999, 0);
                    npc.Transform(NPCID.Corruptor);
                    break;

                case NPCID.EaterofWorldsHead:
                    target.AddBuff(BuffID.CursedInferno, Main.rand.Next(60, 600));
                    target.AddBuff(BuffID.Weak, 7200);
                    target.AddBuff(MasomodeEX.Souls.BuffType("Shadowflame"), Main.rand.Next(60, 600));
                    if (target.statLife + damage < 150)
                        target.KillMe(PlayerDeathReason.ByCustomReason(target.name + " was eaten alive by Eater of Worlds."), 999, 0);
                    break;

                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    target.AddBuff(BuffID.CursedInferno, Main.rand.Next(60, 600));
                    target.AddBuff(BuffID.Weak, 7200);
                    target.AddBuff(MasomodeEX.Souls.BuffType("Shadowflame"), Main.rand.Next(60, 600));
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
                    target.AddBuff(MasomodeEX.Souls.BuffType("ClippedWings"), 2);
                    break;

                case NPCID.Probe:
                    target.AddBuff(MasomodeEX.Souls.BuffType("ClippedWings"), 2);
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        if (Main.npc[i].active && (Main.npc[i].type == NPCID.SkeletronPrime || Main.npc[i].type == NPCID.Retinazer || Main.npc[i].type == NPCID.Spazmatism || Main.npc[i].type == NPCID.TheDestroyer))
                            break;
                    }
                    NPC.SpawnOnPlayer(target.whoAmI, NPCID.SkeletronPrime);
                    NPC.SpawnOnPlayer(target.whoAmI, NPCID.Retinazer);
                    NPC.SpawnOnPlayer(target.whoAmI, NPCID.Spazmatism);
                    NPC.SpawnOnPlayer(target.whoAmI, NPCID.TheDestroyer);
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

                case NPCID.ChaosElemental:
                case NPCID.IlluminantBat:
                case NPCID.IlluminantSlime:
                case NPCID.EnchantedSword:
                case NPCID.BigMimicHallow:
                case NPCID.Pixie:
                case NPCID.Unicorn:
                case NPCID.Gastropod:
                case NPCID.LightMummy:
                case NPCID.RainbowSlime:
                case NPCID.DesertGhoulHallow:
                case NPCID.SandsharkHallow:
                    target.AddBuff(MasomodeEX.Souls.BuffType("Unstable"), Main.rand.Next(60, 300));
                    target.AddBuff(BuffID.Confused, Main.rand.Next(300, 1200));
                    break;

                case NPCID.CorruptGoldfish:
                case NPCID.DevourerBody:
                case NPCID.DevourerHead:
                case NPCID.DevourerTail:
                case NPCID.CorruptBunny:
                case NPCID.CorruptPenguin:
                case NPCID.BigMimicCorruption:
                case NPCID.CorruptSlime:
                case NPCID.DesertGhoulCorruption:
                case NPCID.SandsharkCorrupt:
                case NPCID.Slimer:
                case NPCID.Slimer2:
                case NPCID.Slimeling:
                case NPCID.CursedHammer:
                case NPCID.Clinger:
                case NPCID.SeekerHead:
                case NPCID.SeekerBody:
                case NPCID.SeekerTail:
                case NPCID.VileSpit:
                    target.AddBuff(BuffID.CursedInferno, Main.rand.Next(60, 600));
                    target.AddBuff(BuffID.Weak, Main.rand.Next(7200));
                    target.AddBuff(MasomodeEX.Souls.BuffType("Shadowflame"), Main.rand.Next(60, 600));
                    break;

                case NPCID.Creeper:
                case NPCID.BrainofCthulhu:
                case NPCID.BloodCrawler:
                case NPCID.BloodCrawlerWall:
                case NPCID.Crimslime:
                case NPCID.CrimsonAxe:
                case NPCID.CrimsonBunny:
                case NPCID.CrimsonGoldfish:
                case NPCID.CrimsonPenguin:
                case NPCID.DesertGhoulCrimson:
                case NPCID.SandsharkCrimson:
                case NPCID.FaceMonster:
                case NPCID.Herpling:
                case NPCID.BloodJelly:
                case NPCID.BloodFeeder:
                case NPCID.FloatyGross:
                case NPCID.BigMimicCrimson:
                    target.AddBuff(BuffID.Ichor, Main.rand.Next(60, 600));
                    target.AddBuff(BuffID.Bleeding, Main.rand.Next(7200));
                    target.AddBuff(MasomodeEX.Souls.BuffType("Bloodthirsty"), Main.rand.Next(300));
                    break;

                case NPCID.Crimera:
                    target.AddBuff(BuffID.Ichor, Main.rand.Next(60, 600));
                    target.AddBuff(BuffID.Bleeding, Main.rand.Next(7200));
                    target.AddBuff(MasomodeEX.Souls.BuffType("Bloodthirsty"), Main.rand.Next(300));
                    npc.Transform(NPCID.IchorSticker);
                    break;

                case NPCID.EyeofCthulhu:
                case NPCID.ServantofCthulhu:
                    target.AddBuff(MasomodeEX.Souls.BuffType("Hexed"), Main.rand.Next(300));
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

                case NPCID.SolarSolenian:
                    if (npc.ai[2] <= -6f)
                        damage = 0;
                    break;

                case NPCID.MoonLordCore:
                case NPCID.MoonLordHand:
                case NPCID.MoonLordHead:
                    if (damage > npc.lifeMax / 10)
                    {
                        damage = 0;
                        Main.NewText("YOU THINK YOU CAN BUTCHER A GREAT LORD!?!", Color.LimeGreen);
                    }
                    break;

                default:
                    break;
            }
        }

        public override bool CheckDead(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.ChaosElemental:
                case NPCID.IlluminantBat:
                case NPCID.IlluminantSlime:
                case NPCID.EnchantedSword:
                case NPCID.BigMimicHallow:
                case NPCID.Pixie:
                case NPCID.Unicorn:
                case NPCID.Gastropod:
                case NPCID.LightMummy:
                case NPCID.RainbowSlime:
                case NPCID.DesertGhoulHallow:
                case NPCID.SandsharkHallow:
                    if (Main.netMode != 1)
                        for (int i = 0; i < 8; i++)
                            Projectile.NewProjectile(npc.Center, Vector2.UnitY.RotatedBy(2 * Math.PI / 8 * i) * 4f, ProjectileID.HallowSpray, 0, 0f, Main.myPlayer, 8f);
                    break;

                case NPCID.CorruptGoldfish:
                case NPCID.DevourerBody:
                case NPCID.DevourerHead:
                case NPCID.DevourerTail:
                case NPCID.CorruptBunny:
                case NPCID.CorruptPenguin:
                case NPCID.BigMimicCorruption:
                case NPCID.CorruptSlime:
                case NPCID.DesertGhoulCorruption:
                case NPCID.SandsharkCorrupt:
                case NPCID.Slimer:
                case NPCID.Slimer2:
                case NPCID.Slimeling:
                case NPCID.CursedHammer:
                case NPCID.Clinger:
                case NPCID.SeekerHead:
                case NPCID.SeekerBody:
                case NPCID.SeekerTail:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                case NPCID.EaterofSouls:
                case NPCID.VileSpit:
                case NPCID.Corruptor:
                    if (Main.netMode != 1)
                        for (int i = 0; i < 8; i++)
                            Projectile.NewProjectile(npc.Center, Vector2.UnitY.RotatedBy(2 * Math.PI / 8 * i) * 4f, ProjectileID.CorruptSpray, 0, 0f, Main.myPlayer, 8f);
                    break;

                case NPCID.Creeper:
                case NPCID.BrainofCthulhu:
                case NPCID.BloodCrawler:
                case NPCID.BloodCrawlerWall:
                case NPCID.Crimera:
                case NPCID.Crimslime:
                case NPCID.CrimsonAxe:
                case NPCID.CrimsonBunny:
                case NPCID.CrimsonGoldfish:
                case NPCID.CrimsonPenguin:
                case NPCID.DesertGhoulCrimson:
                case NPCID.SandsharkCrimson:
                case NPCID.FaceMonster:
                case NPCID.Herpling:
                case NPCID.BloodJelly:
                case NPCID.BloodFeeder:
                case NPCID.FloatyGross:
                case NPCID.BigMimicCrimson:
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
                else if (npc.type == MasomodeEX.Fargo.NPCType("Deviantt"))
                {
                    int mutant = NPC.FindFirstNPC(MasomodeEX.Fargo.NPCType("Mutant"));
                    if (mutant > -1 && Main.npc[mutant].active)
                    {
                        Main.npc[mutant].Transform(MasomodeEX.Souls.NPCType("MutantBoss"));
                        if (Main.netMode == 0)
                            Main.NewText("Mutant has been enraged by the death of his sister!", 175, 75, 255);
                        else if (Main.netMode == 2)
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Mutant has been enraged by the death of his sister!"), new Color(175, 75, 255));
                    }
                }
                else if (npc.type == MasomodeEX.Fargo.NPCType("Mutant"))
                {
                    npc.active = true;
                    npc.life = 1;
                    npc.Transform(MasomodeEX.Souls.NPCType("MutantBoss"));
                    if (Main.netMode == 0)
                        Main.NewText("Mutant has been enraged!", 175, 75, 255);
                    else if (Main.netMode == 2)
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Mutant has been enraged!"), new Color(175, 75, 255));
                    return false;
                }
            }
            return true;
        }

        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.MoonLordCore)
            {
                Main.NewText("Ahhhhh! It was a mistake to cum here!", Color.LimeGreen);
                Main.NewText("The enemy souls are possessed by ethereal spirits...", Color.LimeGreen);
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            ModifyHitByEither(npc, ref damage);
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            ModifyHitByEither(npc, ref damage);

            if (!NPC.downedBoss3 && FargowiltasSouls.NPCs.EModeGlobalNPC.AnyBossAlive() && projectile.type == ProjectileID.WaterBolt)
                NPC.SpawnOnPlayer(projectile.owner, MasomodeEX.Souls.NPCType("MutantBoss"));

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

        private bool StealFromInventory(Player target, ref Item item)
        {
            if (!item.IsAir)
            {
                int i = Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, item.type, 1, false, 0, false, false);
                Main.item[i].netDefaults(item.netID);
                Main.item[i].Prefix(item.prefix);
                Main.item[i].stack = item.stack;
                Main.item[i].velocity.X = Main.rand.Next(-20, 21) * 0.2f;
                Main.item[i].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
                Main.item[i].noGrabDelay = 100;
                Main.item[i].newAndShiny = false;

                if (Main.netMode == 1)
                    NetMessage.SendData(21, -1, -1, null, i, 0.0f, 0.0f, 0.0f, 0, 0, 0);

                item = new Item();

                return true;
            }
            else
            {
                return false;
            }
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
                        npc.StrikeNPC(9999, 0f, 0);
                        Main.PlaySound(15, Main.player[Main.myPlayer].Center, 0);
                        Main.player[Main.myPlayer].AddBuff(mod.BuffType("MutantJudgement"), 3600);
                        for (int j = 0; j < 10; j++)
                            NPC.SpawnOnPlayer(Main.myPlayer, MasomodeEX.Souls.NPCType("MutantBoss"));
                        break;
                    }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (npc.type == MasomodeEX.Fargo.NPCType("Mutant") && Main.rand.Next(3) == 0)
                chat = "What you're doing is a crime against the universe, mortal.";
            else if (npc.type == MasomodeEX.Fargo.NPCType("Deviantt") && Main.rand.Next(3) == 0)
                chat = "Play a real game mode, would you? Not this... thing.";
        }

        public override void OnCatchNPC(NPC npc, Player player, Item item)
        {
            if (npc.type == MasomodeEX.Fargo.NPCType("Deviantt"))
            {
                int mutant = NPC.FindFirstNPC(MasomodeEX.Fargo.NPCType("Mutant"));
                if (mutant > -1 && Main.npc[mutant].active)
                {
                    Main.npc[mutant].Transform(MasomodeEX.Souls.NPCType("MutantBoss"));
                    if (Main.netMode == 0)
                        Main.NewText("Mutant has been enraged by the abduction of his sister!", 175, 75, 255);
                    else if (Main.netMode == 2)
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Mutant has been enraged by the abduction of his sister!"), new Color(175, 75, 255));
                }
                else
                {
                    NPC.SpawnOnPlayer(player.whoAmI, MasomodeEX.Souls.NPCType("MutantBoss"));
                }
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            pool[MasomodeEX.Souls.NPCType("MutantBoss")] = .0001f;

            if (!NPC.downedBoss3)
            {
                switch(Framing.GetTileSafely(spawnInfo.playerFloorX, spawnInfo.playerFloorY).type)
                {
                    case TileID.BlueDungeonBrick:
                    case TileID.GreenDungeonBrick:
                    case TileID.PinkDungeonBrick:
                        pool[NPCID.DungeonGuardian] = 1000f;
                        break;

                    default:
                        break;
                }

                switch (Framing.GetTileSafely(spawnInfo.player.Center).wall)
                {
                    case WallID.BlueDungeonSlabUnsafe:
                    case WallID.BlueDungeonTileUnsafe:
                    case WallID.BlueDungeonUnsafe:
                    case WallID.GreenDungeonSlabUnsafe:
                    case WallID.GreenDungeonTileUnsafe:
                    case WallID.GreenDungeonUnsafe:
                    case WallID.PinkDungeonSlabUnsafe:
                    case WallID.PinkDungeonTileUnsafe:
                    case WallID.PinkDungeonUnsafe:
                        pool[NPCID.DungeonGuardian] = 1000f;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}