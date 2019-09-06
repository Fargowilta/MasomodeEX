using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX
{
    internal partial class MasomodeEXWorld : ModWorld
    {
        public override void PreUpdate()
        {
            Main.expertMode = true;
            FargowiltasSouls.FargoSoulsWorld.MasochistMode = true;

            if (Main.time == 1 && Main.netMode != 1)
            {
                if (Main.dayTime) //all bosses become DG at daybreak
                    for (int i = 0; i < Main.maxNPCs; i++)
                        if (Main.npc[i].active && Main.npc[i].boss)
                            Main.npc[i].Transform(NPCID.DungeonGuardian);

                if (Main.rand.Next(4) == 0) //bloodmoon, eclipse
                {
                    if (!Main.dayTime)
                        Main.bloodMoon = true;
                    else if (Main.hardMode)
                        Main.eclipse = true;

                    if (Main.netMode == 2)
                        NetMessage.SendData(7); //sync world
                }
            }
        }
    }
}