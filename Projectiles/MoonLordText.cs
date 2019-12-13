using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX.Projectiles
{
    public class MoonLordText : ModProjectile
    {
        public bool[] text = new bool[17];

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Lord Text");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2;
            projectile.hide = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            int ai0 = (int)projectile.ai[0];
            if (!(ai0 > -1 && ai0 < 200 && Main.npc[ai0].active && Main.npc[ai0].type == NPCID.MoonLordCore))
            {
                projectile.Kill();
                return;
            }
            NPC npc = Main.npc[ai0];
            Player player = Main.player[npc.target];
            projectile.Center = npc.Center;
            projectile.timeLeft = 2;
            if (!text[0])
            {
                text[0] = true;
                EdgyBossText(npc, "Your fate has been sealed...");
                EdgyBossText(npc, "A godly fate approaches your essence...");
                EdgyBossText(npc, "Well well well, You spawned me! How could you!?!");
                EdgyBossText(npc, "If you keep doing this to me, You have Cthulhu to contend with!");
                if (NPC.downedMoonlord)
                {
                    EdgyBossText(npc, "Guess who's back, that's right it's me! Mwahahahahahaha!");
                    EdgyBossText(npc, "Ugh, not again!");
                }
            }
            EdgyBossText(npc, ref text[1], 1, "Aaahhh, What are you even doing to me?");
            EdgyBossText(npc, ref text[2], 1, "Not my heart!");
            EdgyBossText(npc, ref text[3], 0.75, "Why are you ever attacking me ignorantly?");
            EdgyBossText(npc, ref text[4], 0.5, "Grrrrrr! What are you doing to me you little peasant!?!");
            EdgyBossText(npc, ref text[5], 0.25, "Owwwww!! HELP ME!! I'm getting badly hurt by this little struggle!!");
            EdgyBossText(npc, ref text[6], 0.15, "NOOOOOO! Stop trying to defeat me you silly little peasant!!!");
            EdgyBossText(npc, ref text[7], 0.1, "THAT'S IT!!! I'LL JUST REGENERATE MORE HEALTH EVERY SECOND!!!!!!");
            EdgyBossText(npc, ref text[8], 0.05, "GGRRRRR!!!! ARE YOU DONE ATTACKING ME YET!?!?!");
            EdgyBossText(npc, ref text[9], 0.01, "SURRENDER NOW!");
            EdgyBossText(npc, ref text[10], 0.005, "THIS IS MY LAST ATTACK -- MY TRUMP CARD AS LIBTARDS WOULD SAY. SURRENDER NOW OR FACE MY GAMER DAB!");
            EdgyBossText(npc, ref text[11], 0.01, "This isn’t over!");

            if (!text[12] && player.statLife < player.statLifeMax2 * .5)
            {
                text[12] = true;
                EdgyBossText(npc, "The Grand Hero will fall, Hahahahahaha!!!");
            }
            if (!text[13] && player.statLife < 200)
            {
                text[13] = true;
                EdgyBossText(npc, "Do you know how I obtained my power? Oh, you don't know the strength I hold? Well here you go!!");
            }
            if (!text[14] && player.statLife < player.statLifeMax2 * .25)
            {
                text[14] = true;
                EdgyBossText(npc, "THE OCTOPUS GOD RULES THE DEPTHS, THE TWINS RULE THE SKIES. BUT I, THE GREAT EARTH LORD, RULE THE EARTH ITSELF!!!");
            }

            if (!text[15])
            {
                if (!player.active || player.dead)
                {
                    text[15] = true;
                    EdgyBossText(npc, "Weakling!!");
                    if (NPC.AnyNPCs(NPCID.MoonLordFreeEye))
                        EdgyBossText(npc, "You can’t even survive them! What hope do you have against me!?");
                }
            }

            if (!text[16] && npc.GetGlobalNPC<MasomodeEXGlobalNPC>().masoBool[1])
            {
                for (int i = 0; i < text.Length; i++)
                    text[i] = false;
                text[0] = true;
                text[16] = true;
            }
        }

        private void EdgyBossText(NPC npc, ref bool check, double threshold, string text)
        {
            if (!check && npc.life < npc.lifeMax * threshold)
            {
                check = true;
                EdgyBossText(npc, text);
            }
        }

        private void EdgyBossText(NPC npc, string text)
        {
            if (Main.netMode == 0)
                Main.NewText(text, Color.LimeGreen);
            else if (Main.netMode == 2)
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.LimeGreen);
        }
    }
}