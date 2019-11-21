using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX
{
    public class MasomodeEXGlobalTile : GlobalTile
    {
        public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
        {
            switch (type)
            {
                case TileID.Meteorite:
                    if (!NPC.downedBoss2)
                    {
                        int p = Player.FindClosest(new Vector2(i * 16, j * 16), 0, 0);
                        if (p != -1)
                            NPC.SpawnOnPlayer(p, NPCID.SolarCorite);
                        return false;
                    }
                    if (MasomodeEX.Instance.VeinMinerLoaded)
                    {
                        int p = Player.FindClosest(new Vector2(i * 16, j * 16), 0, 0);
                        string text = "Don't even try to bring that vein-mining cheese in here!";
                        if (Main.netMode == 0)
                            Main.NewText(text, Color.LimeGreen);
                        else if (Main.netMode == 2)
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.LimeGreen);
                        if (p != -1)
                            NPC.SpawnOnPlayer(p, MasomodeEX.Souls.NPCType("MutantBoss"));
                    }
                    break;

                case TileID.BlueDungeonBrick:
                case TileID.GreenDungeonBrick:
                case TileID.PinkDungeonBrick:
                case TileID.LihzahrdBrick:
                    if (!NPC.downedMoonlord)
                        return false;
                    break;

                case TileID.Hellstone:
                    if (!(NPC.downedBoss2 && NPC.downedBoss3))
                        return false;
                    break;
                    
                case TileID.Traps:
                case TileID.PressurePlates:
                    if (Framing.GetTileSafely(i, j).wall == WallID.LihzahrdBrickUnsafe)
                        return NPC.downedGolemBoss;
                    break;

                default:
                    break;
            }
            return true;
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            switch(type)
            {
                case TileID.Trees:
                    NPC.NewNPC(i * 16, j * 16, Main.rand.Next(2) == 0 ? NPCID.Bee : NPCID.BeeSmall);
                    break;

                case TileID.Pots:
                    for (int a = 0; a < 5; a++)
                    {
                        int p = Projectile.NewProjectile(new Vector2(i * 16, j * 16), Main.rand.NextVector2Unit() * 6f, MasomodeEX.Souls.ProjectileType("MothDust"), 10, 0f, Main.myPlayer);
                        if (p != 1000)
                            Main.projectile[p].timeLeft = 30;
                    }
                    break;

                case TileID.Explosives:
                    Projectile.NewProjectile(i * 16 + 8, j * 16 + 8, 0f, 0f, ProjectileID.Explosives, 500, 10, Main.myPlayer);
                    break;

                default:
                    break;
            }
        }

        public override void RightClick(int i, int j, int type)
        {
            if (type == TileID.Containers || type == TileID.Containers2)
                if (Main.rand.Next(100) == 0)
                    Projectile.NewProjectile(i * 16 + 8, j * 16 + 8, 0f, 0f, ProjectileID.Explosives, 500, 10, Main.myPlayer);
        }
    }
}