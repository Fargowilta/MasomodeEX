using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX
{
    internal partial class MasomodeEXPlayer : ModPlayer
    {
        public int hitcap = 25;

        public override void PreUpdate()
        {
            if (player.wet)
                player.AddBuff(MasomodeEX.Souls.BuffType("Lethargic"), 2);

            if (player.lavaWet)
            {
                player.AddBuff(BuffID.Burning, 2);
                if (player.ZoneUnderworldHeight)
                    player.AddBuff(MasomodeEX.Souls.BuffType("Shadowflame"), 2);
            }

            if (player.honeyWet)
                player.AddBuff(BuffID.Slow, 2);

            if (player.adjLava)
                player.AddBuff(BuffID.OnFire, 2);

            Tile currentTile = Framing.GetTileSafely(player.Center);
            if (currentTile.wall == WallID.GraniteUnsafe)
                player.AddBuff(MasomodeEX.Souls.BuffType("Crippled"), 2);
            if (currentTile.wall == WallID.MarbleUnsafe)
                player.AddBuff(MasomodeEX.Souls.BuffType("ClippedWings"), 2);
            if (currentTile.type == TileID.Trees || currentTile.type == TileID.PalmTree)
            {
                if (player.hurtCooldowns[0] <= 0) //same i-frames as spike tiles
                    player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was pricked by a Tree."), 20, 0, false, false, false, 0);
            }
            if (currentTile.wall == WallID.LihzahrdBrickUnsafe)
                player.AddBuff(MasomodeEX.Souls.BuffType("LowGround"), 2);

            if (currentTile.type == TileID.DemonAltar && player.hurtCooldowns[0] <= 0)
            {
                int def = player.statDefense;
                float end = player.endurance;
                player.statDefense = 0;
                player.endurance = 0;
                player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " was slain."), player.statLife / 2, 0, false, false, false, 0);
                player.statDefense = def;
                player.endurance = end;
            }

            if (player.ZoneOverworldHeight || player.ZoneSkyHeight)
            {
                if (Main.dayTime)
                {
                    if (Main.eclipse)
                        player.AddBuff(MasomodeEX.Souls.BuffType("LivingWasteland"), 2);
                }
                else
                {
                    player.AddBuff(currentTile.wall == WallID.None ? BuffID.Blackout : BuffID.Darkness, 2);
                    if (Main.bloodMoon)
                        player.AddBuff(MasomodeEX.Souls.BuffType("Bloodthirsty"), 2);
                    if (Main.pumpkinMoon)
                        player.AddBuff(MasomodeEX.Souls.BuffType("Rotting"), 2);
                    if (Main.snowMoon)
                        player.AddBuff(BuffID.Frostburn, 2);
                }
                if (Main.raining)
                {
                    if (currentTile.wall == WallID.None)
                    {
                        if (player.ZoneSnow)
                        {
                            player.AddBuff(BuffID.Chilled, 2);
                            int d = player.FindBuffIndex(BuffID.Chilled);
                            if (d != -1 && player.buffTime[d] > 7200)
                            {
                                player.ClearBuff(BuffID.Chilled);
                                player.AddBuff(BuffID.Frozen, Main.expertDebuffTime > 1 ? 150 : 300);
                            }
                            player.AddBuff(BuffID.Frostburn, 2);
                        }
                        else
                        {
                            player.AddBuff(BuffID.Wet, 2);
                            player.AddBuff(MasomodeEX.Souls.BuffType("LightningRod"), 2);
                        }
                    }
                }
            }

            if (player.ZoneSkyHeight && player.breath > 0)
                player.breath--;

            if (player.ZoneUnderworldHeight && !player.fireWalk && !player.buffImmune[BuffID.OnFire])
                player.AddBuff(BuffID.Burning, 2);

            if (player.ZoneBeach)
            {
                if (player.GetModPlayer<FargowiltasSouls.FargoPlayer>().MaxLifeReduction < 50)
                    player.GetModPlayer<FargowiltasSouls.FargoPlayer>().MaxLifeReduction = 50;
                player.AddBuff(MasomodeEX.Souls.BuffType("OceanicMaul"), 2);
                if (player.wet)
                    player.AddBuff(MasomodeEX.Souls.BuffType("MutantNibble"), 2);
            }
            else if (player.ZoneDesert)
            {
                if (player.ZoneOverworldHeight)
                {
                    if (currentTile.wall == WallID.None)
                        player.AddBuff(BuffID.WindPushed, 2);
                }
                player.AddBuff(BuffID.Weak, 2);
            }

            if (player.ZoneJungle)
            {
                player.AddBuff(MasomodeEX.Souls.BuffType("Swarming"), 2);
                if (player.wet)
                {
                    player.AddBuff(MasomodeEX.Souls.BuffType("Infested"), 2);
                    if (Main.hardMode)
                        player.AddBuff(BuffID.Venom, 2);
                }
            }

            if (player.ZoneSnow)
            {
                player.AddBuff(BuffID.Chilled, 2);
                if (player.wet)
                    player.AddBuff(BuffID.Frostburn, 2);
            }

            if (player.ZoneDungeon)
                player.AddBuff(BuffID.WaterCandle, 2);

            if (player.ZoneCorrupt)
            {
                player.AddBuff(BuffID.Darkness, 2);
                if (player.wet)
                    player.AddBuff(BuffID.CursedInferno, 2);
            }

            if (player.ZoneCrimson)
            {
                player.AddBuff(BuffID.Bleeding, 2);
                if (player.wet)
                    player.AddBuff(BuffID.Ichor, 2);
            }

            if (player.ZoneHoly)
            {
                player.AddBuff(MasomodeEX.Souls.BuffType("FlippedHallow"), 120);
                if (player.wet)
                    player.AddBuff(BuffID.Confused, 2);
            }

            if (player.ZoneMeteor && !player.fireWalk)
                player.AddBuff(BuffID.OnFire, 2);

            if (!player.buffImmune[BuffID.Webbed] && player.stickyBreak > 0)
            {
                player.AddBuff(BuffID.Webbed, 30);
                player.stickyBreak = 0;
                //player.stickyBreak = 1000;
                Vector2 vector = Collision.StickyTiles(player.position, player.velocity, player.width, player.height);
                if (vector.X != -1 && vector.Y != -1)
                {
                    int num3 = (int)vector.X;
                    int num4 = (int)vector.Y;
                    WorldGen.KillTile(num3, num4, false, false, false);
                    if (Main.netMode == 1 && !Main.tile[num3, num4].active())
                        NetMessage.SendData(17, -1, -1, null, 0, num3, num4, 0f, 0, 0, 0);
                }
            }
        }

        public override void UpdateDead()
        {
            hitcap = 25;
        }

        public override void PostUpdateMiscEffects()
        {
            Tile currentTile = Framing.GetTileSafely(player.Center);
            if (currentTile.wall == WallID.LihzahrdBrickUnsafe)
            {
                player.dangerSense = false;
                player.InfoAccMechShowWires = false;
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            damage = (int)(damage * 1.5);
            return true;
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (NPC.AnyNPCs(NPCID.MoonLordCore) && --hitcap <= 0)
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " got terminated."), 19998, 0);
                Main.NewText("Mwahahahahahaha! You survived nothing!!", Color.LimeGreen);
            }
            //Main.player[Main.myPlayer].AddBuff(MasomodeEX.DebuffIDs[Main.rand.Next(MasomodeEX.DebuffIDs.Count)], Main.rand.Next(60, 600));
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            int baseCheckX = (int) player.Center.X / 16 - 1;
            int baseCheckY = (int) player.Center.Y / 16 - 2;

            for (int i = 0; i < 3; i++) //MAKE SURE nothing in the way
                for (int j = 0; j < 4; j++)
                    WorldGen.KillTile(baseCheckX + i, baseCheckY + j);

            WorldGen.PlaceTile(baseCheckX, baseCheckY + 4, TileID.GrayBrick, false, true);
            WorldGen.PlaceTile(baseCheckX + 1, baseCheckY + 4, TileID.GrayBrick, false, true);
            WorldGen.PlaceTile(baseCheckX + 2, baseCheckY + 4, TileID.GrayBrick, false, true);
            Main.tile[baseCheckX, baseCheckY + 4].slope(0);
            Main.tile[baseCheckX + 1, baseCheckY + 4].slope(0);
            Main.tile[baseCheckX + 2, baseCheckY + 4].slope(0);
            WorldGen.PlaceTile(baseCheckX + 1, baseCheckY + 3, MasomodeEX.Souls.TileType("MutantStatueGift"), false, true);

            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (NPC.AnyNPCs(MasomodeEX.Souls.NPCType("MutantBoss")))
                MasomodeEXWorld.MutantPlayerKills++;
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (proj.type == MasomodeEX.Souls.ProjectileType("MutantSpearAim")
                || proj.type == MasomodeEX.Souls.ProjectileType("MutantSpearDash")
                || proj.type == MasomodeEX.Souls.ProjectileType("MutantSpearSpin")
                || proj.type == MasomodeEX.Souls.ProjectileType("MutantSpearThrown"))
            {
                player.AddBuff(MasomodeEX.Souls.BuffType("TimeFrozen"), 60);
                player.AddBuff(mod.BuffType("MutantJudgement"), 3600);
            }
        }

        public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
        {
            if (Main.rand.Next(200) == 0)
                caughtType = mod.ItemType("MutantSummon");
        }

        public override void OnEnterWorld(Player player)
        {
            Main.NewText("Welcome to Masochist Mode EX!", Color.LimeGreen);
            Main.NewText("This is NOT regular Masochist Mode!", Color.LimeGreen);
            Main.NewText("This is an unbalanced, impossibly difficult EX mode!!", Color.LimeGreen);
            Main.NewText("Disable the Masochist Mode EX mod for the correct Masochist Mode experience!!!", Color.LimeGreen);
        }
    }
}