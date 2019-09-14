using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace MasomodeEX.Buffs
{
	public class MutantJudgement : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mutant Judgement");
			Description.SetDefault("You have been deemed unworthy");
			Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
        {
            player.immune = false;
            player.immuneTime = 0;
            player.hurtCooldowns[0] = 0;
            player.hurtCooldowns[1] = 0;
            player.moonLeech = true;
            player.chaosState = true;
            player.potionDelay = player.buffTime[buffIndex];
            player.GetModPlayer<FargowiltasSouls.FargoPlayer>(MasomodeEX.Souls).noDodge = true;
            player.GetModPlayer<FargowiltasSouls.FargoPlayer>(MasomodeEX.Souls).noSupersonic = true;
            player.GetModPlayer<FargowiltasSouls.FargoPlayer>(MasomodeEX.Souls).MutantPresence = true;
            player.GetModPlayer<FargowiltasSouls.FargoPlayer>(MasomodeEX.Souls).MutantNibble = true;
        }
    }
}
