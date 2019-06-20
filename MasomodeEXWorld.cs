using Terraria;
using Terraria.ModLoader;

namespace MasomodeEX
{
    internal partial class MasomodeEXWorld : ModWorld
    {
        public override void PreUpdate()
        {
            Main.expertMode = true;
            //also force masomode to true

            if (Main.time == 0 && Main.netMode != 1 && Main.rand.Next(4) == 0)
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