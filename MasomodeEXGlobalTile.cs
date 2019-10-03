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

                case TileID.Trees:
                    NPC.NewNPC(i * 16, j * 16, Main.rand.Next(2) == 0 ? NPCID.Bee : NPCID.BeeSmall);
                    break;

                default:
                    break;
            }
            return true;
        }
    }
}