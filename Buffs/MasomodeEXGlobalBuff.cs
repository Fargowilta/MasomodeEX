using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX.Buffs
{
    class MasomodeEXGlobalBuff : GlobalBuff
    {
        public override bool ReApply(int type, Player player, int time, int buffIndex)
        {
            if (Main.debuff[type] && type != BuffID.PeaceCandle
                && player.buffTime[buffIndex] < 18000 + 60) //extra second to hopefully prevent duration flickering
                player.buffTime[buffIndex] += time;
            return false;
        }

        public override void Update(int type, Player player, ref int buffIndex)
        {
            switch (type)
            {
                case BuffID.Rabies:
                    if (player.buffTime[buffIndex] > 1)
                    {
                        player.AddBuff(MasomodeEX.Souls.BuffType("MutantNibble"), player.buffTime[buffIndex]);
                        player.buffTime[buffIndex] = 1;
                    }
                    break;

                case BuffID.Frozen:
                case BuffID.Stoned:
                    if (player.buffTime[buffIndex] > 3600)
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " shattered."), 9999, 0);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
