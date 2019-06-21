using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX.Buffs
{
    class MasomodeEXGlobalBuff : GlobalBuff
    {
        public override bool ReApply(int type, Player player, int time, int buffIndex)
        {
            if (Main.debuff[type] && type != BuffID.PeaceCandle
                && player.buffTime[buffIndex] < 3600 + 60) //extra second to hopefully prevent duration flickering
                player.buffTime[buffIndex] += time;
            return false;
        }
    }
}
