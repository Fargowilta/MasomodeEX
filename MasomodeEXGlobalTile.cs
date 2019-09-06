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
            if (type == TileID.Meteorite && !NPC.downedBoss2)
            {
                int p = Player.FindClosest(new Vector2(i * 16, j * 16), 0, 0);
                if (p != -1)
                    NPC.SpawnOnPlayer(p, NPCID.SolarCorite);
                return false;
            }
            return true;
        }
    }
}