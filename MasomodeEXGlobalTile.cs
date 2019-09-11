using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
                    break;

                case TileID.BlueDungeonBrick:
                case TileID.GreenDungeonBrick:
                case TileID.PinkDungeonBrick:
                case TileID.LihzahrdBrick:
                    if (!NPC.downedMoonlord)
                        return false;
                    break;

                case TileID.Hellstone:
                    if (!Main.hardMode)
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