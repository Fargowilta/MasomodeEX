using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
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

        public bool[] masoBool = new bool[2];
        public int[] Counter = new int[2];

        public override void SetDefaults(NPC npc)
        {
            if (!npc.friendly)
            {
                npc.lifeMax = (int)(npc.lifeMax * 1.5);
                npc.damage = (int)(npc.damage * 1.5);
                npc.defense = (int)(npc.defense * 1.5);
            }
        }

        public override void AI(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.KingSlime:
                    Aura(npc, 600, BuffID.Slimed, false, 33);
                    npc.position.X += npc.velocity.X;
                    if (npc.velocity.Y < 0)
                        npc.position.Y += npc.velocity.Y;

                    if (masoBool[1])
                    {
                        if (npc.velocity.Y == 0f) //start attack
                        {
                            masoBool[1] = false;
                            if (npc.HasPlayerTarget && Main.netMode != 1)
                            {
                                const float gravity = 0.15f;
                                const float time = 120f;
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
                    break;

                case NPCID.EaterofWorldsHead:
                    //FUCKING FLYYYYYYY
                    break;

                case NPCID.BrainofCthulhu:
                    if (npc.buffType[0] != 0)
                        npc.DelBuff(0);
                    if (++Counter[0] > 60)
                    {
                        Counter[0] = 0;
                        for (int i = 0; i < 200; i++)
                        {
                            if (Main.npc[i].active && Main.npc[i].damage < npc.damage
                                && Main.npc[i].type == MasomodeEX.Souls.NPCType("BrainIllusion"))
                                Main.npc[i].damage = npc.damage;
                        }
                    }
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
                    if (!masoBool[1] && npc.life < npc.lifeMax / 2)
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
                        npc.defense = NPC.AnyNPCs(MasomodeEX.Souls.NPCType("RoyalSubject")) ? 9999 : npc.defDefense;
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
                    npc.ai[1]++;
                    break;

                case NPCID.TheHungry:
                case NPCID.TheHungryII:
                    Aura(npc, 100, BuffID.Burning, false, DustID.Fire);
                    break;

                case NPCID.Retinazer:
                    if (npc.ai[0] >= 4f)
                        Aura(npc, 500, BuffID.Ichor, true, 90);
                    break;

                case NPCID.Spazmatism:
                    if (npc.ai[0] >= 4f)
                        Aura(npc, 500, BuffID.CursedInferno, true, 89);
                    break;

                case NPCID.TheDestroyerBody:
                case NPCID.TheDestroyerTail:
                    if (npc.ai[2] != 0 && ++Counter[0] > 420)
                    {
                        Counter[0] = 0;
                        if (Main.netMode != 1)
                            for (int i = 0; i < 8; i++)
                                Projectile.NewProjectile(npc.Center, Vector2.Normalize(npc.velocity).RotatedBy(2 * Math.PI / 8 * i),
                                    MasomodeEX.Souls.ProjectileType("DarkStar"), npc.damage / 5, 0f, Main.myPlayer);
                    }
                    break;

                case NPCID.SkeletronPrime:
                    Aura(npc, 100, MasomodeEX.Souls.BuffType("Stunned"));
                    npc.reflectingProjectiles = npc.ai[1] == 1f || npc.ai[1] == 2f; //spinning or DG mode
                    break;

                case NPCID.Plantera:
                    if (!masoBool[0])
                    {
                        masoBool[0] = true;
                        if (Main.netMode != 1)
                        {
                            const int max = 15;
                            const float distance = 1300f;
                            float rotation = 2f * (float)Math.PI / max;
                            for (int i = 0; i < max; i++)
                            {
                                Vector2 spawnPos = npc.Center + new Vector2(distance, 0f).RotatedBy(rotation * i);
                                int n = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, mod.NPCType("CrystalLeaf"), 0, npc.whoAmI, distance, 0, rotation * i);
                                if (Main.netMode == 2 && n < 200)
                                    NetMessage.SendData(23, -1, -1, null, n);
                            }
                        }
                    }
                    npc.life += 5;
                    if (npc.life > npc.lifeMax)
                        npc.life = npc.lifeMax;
                    if (++Counter[0] > 75)
                    {
                        Counter[0] = Main.rand.Next(30);
                        CombatText.NewText(npc.Hitbox, CombatText.HealLife, 300);
                    }
                    break;

                case NPCID.PlanterasTentacle:
                    npc.position += npc.velocity;
                    Aura(npc, 200, BuffID.Venom, false, 188);
                    break;

                case NPCID.Golem:
                    npc.position.X += npc.velocity.X;
                    if (npc.velocity.Y < 0)
                        npc.position.Y += npc.velocity.Y;
                    break;

                case NPCID.GolemHead:
                case NPCID.GolemHeadFree:
                    Aura(npc, 300, MasomodeEX.Souls.BuffType("ClippedWings"));
                    break;

                case NPCID.DukeFishron:
                    if (!MasomodeEX.Instance.HyperLoaded)
                    {
                        masoBool[0] = !masoBool[0];
                        if (masoBool[0])
                            npc.AI();
                    }
                    break;

                case NPCID.CultistBoss:
                    Aura(npc, 400, MasomodeEX.Souls.BuffType("MarkedforDeath"), false, 199);
                    Aura(npc, 400, MasomodeEX.Souls.BuffType("Hexed"));
                    if (++Counter[0] > 60)
                    {
                        Counter[0] = 0;
                        if (Main.netMode != 1 && npc.HasPlayerTarget && NPC.AnyNPCs(NPCID.CultistDragonHead))
                        {
                            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.CultistDragonHead);
                            if (n != 200 && Main.netMode == 2)
                                NetMessage.SendData(23, -1, -1, null, n);
                        }
                    }
                    break;

                case NPCID.MoonLordCore:
                    break;

                case NPCID.MoonLordHand:
                case NPCID.MoonLordHead:
                    Aura(npc, 150, MasomodeEX.Souls.BuffType("Flipped"), false, 111);
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
                        Main.player[Main.myPlayer].AddBuff(MasomodeEX.Souls.BuffType("Asocial"), 2);
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
                                        case 2: type = ProjectileID.BouncyBomb: break;
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

                default:
                    break;
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
                        for (int j = 0; j < 10; j++)
                            NPC.SpawnOnPlayer(Main.myPlayer, NPCID.DungeonGuardian);
                        break;
                    }
        }
    }
}