﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX
{
    internal partial class MasomodeEXPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            if (player.wet)
                player.AddBuff(MasomodeEX.Souls.BuffType("Lethargic"), 2);

            if (player.lavaWet)
                player.AddBuff(BuffID.Burning, 2);

            if (player.honeyWet)
                player.AddBuff(BuffID.Slow, 2);

            if (player.adjLava)
                player.AddBuff(BuffID.OnFire, 2);

            if (player.ZoneOverworldHeight || player.ZoneSkyHeight)
            {
                if (Main.dayTime)
                {
                    if (Main.eclipse)
                        player.AddBuff(MasomodeEX.Souls.BuffType("LivingWasteland"), 2);
                }
                else
                {
                    Tile currentTile = Framing.GetTileSafely(player.Center);
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
                    Tile currentTile = Framing.GetTileSafely(player.Center);
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
                    Tile currentTile = Framing.GetTileSafely(player.Center);
                    if (currentTile.wall == WallID.None)
                        player.AddBuff(BuffID.WindPushed, 2);
                }
                player.AddBuff(BuffID.Weak, 2);
            }

            if (player.ZoneJungle)
            {
                player.AddBuff(BuffID.Poisoned, 2);
                if (player.wet)
                    player.AddBuff(mod.BuffType("Infested"), 2);
            }

            if (player.ZoneSnow)
                player.AddBuff(BuffID.Chilled, 2);

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
    }
}