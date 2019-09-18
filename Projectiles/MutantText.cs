using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX.Projectiles
{
    public class MutantText : ModProjectile
    {
        public bool[] p1 = new bool[8];
        public bool[] p2 = new bool[16];

        public string[] spam = {
            "THE ABYSS CONSUMES ALL!",
            "RAGNAROK WILL END YOUR CHILDISH RAMPAGE!",
            "YOU WILL BE BURNED AS THE SUNKEN SEA WAS!",
            "THE TRUE FURY OF THE TYRANT WILL ANNIHILATE YOUR TINY POWER!",
            "YOU ARE NOTHING COMPARED TO THE ERODED SPIRITS!",
            "I BRING FORTH THE END UPON THE FOOLISH, THE UNWORTHY!",
            "YOU WANT TO DEFEAT ME?",
            "MAYBE IN TWO ETERNITIES!",
            "DIE, FOOLISH TERRARIAN!",
            "THEY SAID THERE WAS 3 END BRINGERS...",
            "BUT I AM THE FOURTH, A BREAKER OF REALITY!",
            "HELL DOESN’T ACCEPT SCUM LIKE YOU, SO SUFFER FOREVER IN MY ENDLESS ONSLAUGHT!",
            "MY INFINITE POWER!!",
            "THE POTENTIAL OF ETERNITIES STRETCHED TO THE ABSOLUTE MAXIMUM APOTHEOSES!",
            "YOUR UNHOLY SOUL SHALL BE CONSUMED BY DEPTHS LOWER THAN THE DEEPEST REACHES OF HELL!",
            "I CONTROL THE POWER THAT HAS REACHED FROM THE FAR ENDS OF THE UNIVERSE!",
            "UNITING DIMENSIONS, MANIPULATING TERRARIA, SLAYING MASOCHIST, AND JUDGING HEAVENS!!!",
            "FOR CENTURIES I HAVE TRAINED FOR ONE GOAL ONLY:",
            "PURGE THE WORLD OF THE UNWORTHY, SLAY THE WEAK, AND BRING FORTH TRUE POWER.",
            "IN THE HIGHEST REACHES OF HEAVEN, MY BROTHER RULES OVER THE SKY!",
            "SOON, ALL OF TERRARIA WILL BE PURGED OF THE UNWORTHY AND A NEW AGE WILL START!",
            "A NEW AGE OF AWESOME!",
            "A GOLDEN AGE WHERE ONLY ABSOLUTE BEINGS EXIST!",
            "DEATH, INFERNO, TIDE; I AM THE OMEGA AND THE ALPHA, THE BEGINNING, AND THE END!",
            "ALMIGHTY POWER; REVELATIONS.",
            "ABSOLUTE BEING, ABSOLUTE PURITY.",
            "WITHIN THE FOOLISH BANTERINGS OF THE MORTAL WORLD I HAVE ACHIEVED POWER!",
            "POWER THAT WAS ONCE BANISHED TO THE EDGE OF THE GALAXY!",
            "I BRING FORTH CALAMITIES, CATASTROPHES, AND CATACLYSM;",
            "ELDRITCH POWERS DERIVED FROM THE ABSOLUTE WORD OF FATE.",
            "FEEL MY UBIQUITOUS WRATH DRIVE YOU INTO THE GROUND!",
            "JUST AS A WORLD SHAPER DRIVES HIS WORLD INTO REALITY!",
            "THE SHARPSHOOTER’S EYE PALES IN COMPARISON OF MY PERCEPTION OF REALITY!",
            "THE BERSERKER'S RAGE IS NAUGHT BUT A BUNNY'S BEFORE MINE!",
            "THE OLYMPIANS ARE MERE LESSER GODS COMPARED TO MY IMMEASURABLE MIGHT!",
            "THE ARCH WIZARD'S A POSER, A HACK, A PARLOUR TRICK TOTING JOKER!",
            "A MASTERY OF FLIGHT AND THE IRON WILL OF A COLOSSUS ARE BOTH ELEMENTARY CONCEPTS!",
            "A CONJURER IS BUT A PEDDLING MAGICIAN!",
            "A TRAWLER IS BUT A SLIVER COMPARED TO MY LIFE MASTERY!",
            "SUPERSONIC SPEED, LIGHTSPEED TIME!",
            "GLORIOUS LIGHT SHALL ELIMINATE YOU, YOU FOOLISH BUFFOON!",
            "WHAT ARE YOUR TRUE INTENTIONS?",
            "WHY DO YOU REALLY EVEN NEED THIS POWER?",
            "WHAT IS THE POINT IN ALL OF THIS!?",
            "TO THINK YOU WERE SATISFIED WITH THE PROSPERITY OF THIS LAND!",
            "SAFETY AMONGST THE TOWN, PROTECTION OF THE EVIL THREATS, BUT NO!",
            "YOU WANTED MORE!",
            "YOU JUST WANTED TO SPITE ME, DIDN’T YOU!?",
            "ENOUGH OF THIS!",
            "I CAN’T KEEP GOING MUCH LONGER!",
            "YOU CANNOT KILL ME, THIS IS JUST THE ACT OF AN INSIGNIFICANT LUNATIC!",
            "I WILL SOON RETURN FOR ANOTHER BATTLE!",
            "THIS IS ONLY THE BEGINNING!",
            "DO YOU HONESTLY THINK YOU CAN SURVIVE ANY LONGER?",
            "POWER IS IN THE EYE OF THE BEHOLDER!",
            "YOU ARE NOT DESERVING OF TRUE DIVINITY!",
            "I SHOULD KNOW FROM THE COUNTLESS YOU HAVE SLAUGHTERED!",
            "YOUR MOTIVATION IS UNFOUNDED!",
            "ALL YOU SEEK IS THE DESTRUCTION OF ANYONE WHO POSES A THREAT TO YOU!",
            "THAT’S WHY YOU LIMP ON THIS MOIST ROCK!",
            "SEARCHING FOR STICKS AND PEBBLES TO ELIMINATE THE FEARS YOU CANNOT TRULY OVERCOME!",
            "IT REALLY SURPRISES ME THAT IT TOOK YOU AWHILE TO REACH THIS POINT!",
            "ESPECIALLY GIVEN HOW WELL YOU LEECH OFF YOUR OPPONENTS AFTER BARELY SCRAPING BY!",
            "THAT’S HOW YOU WIN ALL YOUR BATTLES, AM I RIGHT!?",
            "IT’S HONESTLY IMPRESSIVE THAT YOU’VE MADE IT THIS FAR.",
            "I HOPE YOU WEREN’T USING GODMODE, PATHETIC COWARD!",
            "YOU MUST BE SO NERVOUS THAT YOU’RE SO CLOSE!",
            "I HOPE YOU CHOKE, AND I HOPE YOU CHOKE ON THE ASH AND BLOOD OF YOUR SINS TOO!",
            "GLORIOUS LIGHT SHALL ELIMINATE YOU, YOU FOOLISH BUFFOON!",
            "AAAAAAAAAAAAA!",
            "THIS IS IT!",
            "NOW LET'S GET TO THE GOOD PART!!!!!"
        };

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mutant Text");
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
            if (!(ai0 > -1 && ai0 < 200 && Main.npc[ai0].active && Main.npc[ai0].type == MasomodeEX.Souls.NPCType("MutantBoss")))
            {
                projectile.Kill();
                return;
            }
            NPC npc = Main.npc[ai0];
            projectile.Center = npc.Center;
            projectile.timeLeft = 2;
            if (projectile.localAI[0] == 0)
            {
                projectile.localAI[0] = 1;
                npc.lifeMax *= 2;
                npc.life = npc.lifeMax;

                if (MasomodeEXWorld.MutantSummons < 1)
                    EdgyBossText(npc, "Big mistake, kid. You'll pay for that.");
                else if (MasomodeEXWorld.MutantSummons < 3)
                    EdgyBossText(npc, "Back for more? Really now.");
                else if (MasomodeEXWorld.MutantSummons < 5)
                    EdgyBossText(npc, "You really like going through hell don't you? Have it your way.");
                else if (MasomodeEXWorld.MutantSummons < 7)
                    EdgyBossText(npc, "You brought this upon yourself. I'll just have to keep ripping you to bloody pieces.");
                else if (MasomodeEXWorld.MutantSummons < 10)
                    EdgyBossText(npc, "Luck won't help you here. I'm barely responsive to prayers.");
                else if (MasomodeEXWorld.MutantSummons < 15)
                    EdgyBossText(npc, "You have some real commitment... to being a wretched fool, that is.");
                else if (MasomodeEXWorld.MutantSummons < 20)
                    EdgyBossText(npc, "Nothing's sticking. It's no use, TAKE THIS!");
                else if (MasomodeEXWorld.MutantSummons < 50)
                    EdgyBossText(npc, "I bore of dealing with you. Focus, slow down.");
                else if (MasomodeEXWorld.MutantSummons < 75)
                    EdgyBossText(npc, "Give my brother a break, damn it.");
                else if (MasomodeEXWorld.MutantSummons < 100)
                    EdgyBossText(npc, "Honestly, the guide deserved this, but not us.");
                else if (MasomodeEXWorld.MutantSummons < 250)
                    EdgyBossText(npc, "...");
                else if (MasomodeEXWorld.MutantSummons < 500)
                    EdgyBossText(npc, "This is just getting ridiculous...");
                else if (MasomodeEXWorld.MutantSummons < 1000)
                    EdgyBossText(npc, "I’ll make you suffer for an eternity. You deserve as much. Die over and over again until you break in rage and insanity.");
                else
                    EdgyBossText(npc, "Ech.");
                
                MasomodeEXWorld.MutantSummons++;
                if (Main.netMode == 2)
                    NetMessage.SendData(7); //sync world
            }
            EdgyBossText(npc, ref p1[0], 0.9, "I could recite all the digits of Pi before you beat me.");
            EdgyBossText(npc, ref p1[1], 0.8, "Not even the Great Tyrant is a worthy opponent.");
            EdgyBossText(npc, ref p1[2], 0.7, "The eroded spirits have failed... Yet, you will still fail against me.");
            EdgyBossText(npc, ref p1[3], 0.65, "This is just the beginning of true hell.");
            EdgyBossText(npc, ref p1[4], 0.6, "No matter how well you use your weapons, the result, still, will not be in your favor.");
            EdgyBossText(npc, ref p1[5], 0.55, "This fight is already impossible... you think it'll be over that easily?");
            EdgyBossText(npc, ref p1[6], 0.5, "So this is the rampage that killed the Moon Lord...");
            if (p1[7])
            {
                EdgyBossText(npc, ref p2[0], 0.9, "This is barely the beginning!");
                EdgyBossText(npc, ref p2[1], 0.8, "Power is in the eye of the beholder. Absolute power is a fact.");
                EdgyBossText(npc, ref p2[2], 0.7, "Never has else anyone advanced as much as you... there can only be one.");
                EdgyBossText(npc, ref p2[3], 0.6, "This has dragged on too long... prepare for my unbound power.");
                EdgyBossText(npc, ref p2[4], 0.5, "The tyrant and the witch failed to stop you. No matter, you'll end here.");
                EdgyBossText(npc, ref p2[5], 0.4, "Fall beneath the emissary of justice, kid.");
                EdgyBossText(npc, ref p2[6], 0.3, "There’s so much blood on your hands...");
                EdgyBossText(npc, ref p2[7], 0.25, "I may be a mutant, but thank Terry I’m no pushover.");
                EdgyBossText(npc, ref p2[8], 0.2, "Still not worthy, but these are tough times... no longer on your side.");
                EdgyBossText(npc, ref p2[9], 0.15, "Savagery, barbarism, bloodthirst... you have too much.");
                EdgyBossText(npc, ref p2[10], 0.1, "Don’t tell me I’m not creative, the other guy's purple!");
                EdgyBossText(npc, ref p2[11], 0.05, "I shouldn’t have let you buy in bulk.");
                EdgyBossText(npc, ref p2[12], 0.04, "WHY AREN’T YOU DEAD?! FACE TRUE HELL!!!");
                EdgyBossText(npc, ref p2[13], 0.03, "YOU ARE POWERLESS, HERO.");
                EdgyBossText(npc, ref p2[14], 0.02, "ALL YOU FOUGHT FOR WILL BE LOST IN THE INFINITE ABYSS!!!!");
                EdgyBossText(npc, ref p2[15], 0.01, "FALL BENEATH MY FIERY WRATH, FOOLISH HERO! THE ABYSS CONSUMES ALL!");
            }
            if (npc.ai[0] < -1)
            {
                if (++projectile.localAI[1] > 20)
                {
                    projectile.localAI[1] = 0;
                    if (projectile.ai[1] < spam.Length)
                        EdgyBossText(npc, spam[(int)projectile.ai[1]++]);
                }
            }
            switch ((int)npc.ai[0])
            {
                case -7:
                    if (npc.alpha == 0)
                    {
                        if (MasomodeEXWorld.MutantDefeats < 3)
                            EdgyBossText(npc, "At least I'll get to see my brother again...");
                        else if (MasomodeEXWorld.MutantDefeats < 5)
                            EdgyBossText(npc, "With some luck, maybe my brother can catch a break...");
                        else if (MasomodeEXWorld.MutantDefeats < 10)
                            EdgyBossText(npc, "Cut me some slack...");
                        else if (MasomodeEXWorld.MutantDefeats < 15)
                            EdgyBossText(npc, "Now do it without getting hit.");
                        else if (MasomodeEXWorld.MutantDefeats < 50)
                            EdgyBossText(npc, "Showoff... now do it with a copper shortsword.");
                        else if (MasomodeEXWorld.MutantDefeats < 100)
                            EdgyBossText(npc, "How annoying.");
                        else
                            EdgyBossText(npc, "By the way, I'm out of death text now.");

                        MasomodeEXWorld.MutantDefeats++;
                        if (Main.netMode == 2)
                            NetMessage.SendData(7); //sync world
                    }
                    break;

                case 0:
                    if (npc.ai[1] == 1)
                        EdgyBossText(npc, "Now. Prepare to wish death as to your escape in suffering.");
                    break;

                case 1:
                    if (projectile.ai[1] == 0)
                    {
                        projectile.ai[1] = 1;
                        EdgyBossText(npc, "Just try to harm me. Your pathetic peashooters have proven nothing!");
                    }
                    break;

                case 2:
                    projectile.ai[1] = 0;
                    if (npc.ai[1] == 1)
                        EdgyBossText(npc, "I've watched your journey...");
                    break;

                case 4:
                    if (npc.ai[3] == 0)
                        EdgyBossText(npc, "I've seen you fail.");
                    break;

                case 6:
                    if (npc.ai[1] == 0 && projectile.ai[1] == 0)
                    {
                        projectile.ai[1] = 1;
                        EdgyBossText(npc, "Eviscerate under my hands!");
                    }
                    break;

                case 7:
                    projectile.ai[1] = 0;
                    if (npc.ai[2] == 0)
                        EdgyBossText(npc, "Die, you rat!");
                    break;

                case 9:
                    if (npc.ai[3] == 0)
                        EdgyBossText(npc, "How long can you survive?");
                    break;

                case 10:
                    if (npc.ai[1] == 1)
                        EdgyBossText(npc, "No... it can't... be... How could I fall... to a mere human...");
                    if (npc.ai[1] == 120)
                    {
                        EdgyBossText(npc, "Foolish Terrarian. Your powers aren't even 28.5714 percent as strong as mine. Witness a true cataclysm.");
                        EdgyBossText(npc, "THIS ISN'T EVEN MY FINAL FORM!");
                    }
                    break;

                case 11:
                    p1[7] = true;
                    if (npc.ai[2] == 1)
                        EdgyBossText(npc, "Still think this move is that bad?");
                    break;

                case 13:
                    if (npc.ai[3] == 0)
                        EdgyBossText(npc, "Fall beneath true power!");
                    break;

                case 15:
                    if (npc.ai[1] == 0 && projectile.ai[1] == 0)
                    {
                        projectile.ai[1] = 1;
                        EdgyBossText(npc, "Don't even try to dodge!");
                    }
                    break;

                case 16:
                    projectile.ai[1] = 0;
                    if (npc.ai[2] == 1)
                        EdgyBossText(npc, "He who watches only gains suffering.");
                    break;

                case 18:
                    EdgyBossText(npc, "Thought you had a hard enough time dodging one?");
                    break;

                case 19:
                    if (npc.ai[1] == 240)
                        EdgyBossText(npc, "Such a beautiful day outside, isn't it?");
                    break;

                case 20:
                    if (npc.ai[1] == 0 && npc.ai[2] == 1)
                        EdgyBossText(npc, "You know, I was that \"evil presence\" watching...");
                    break;

                case 21:
                    if (npc.ai[3] == 0)
                        EdgyBossText(npc, "Fall beneath true power!");
                    break;

                case 23:
                    if (npc.ai[1] == 0 && projectile.ai[1] == 0)
                    {
                        projectile.ai[1] = 1;
                        EdgyBossText(npc, "Don't even try to dodge!");
                    }
                    break;

                case 24:
                    projectile.ai[1] = 0;
                    if (npc.ai[1] == 1)
                        EdgyBossText(npc, "Worm bosses... worm everywhere.");
                    break;

                case 25:
                    if (npc.ai[1] == 1 && projectile.ai[1] == 0)
                    {
                        projectile.ai[1] = 1;
                        EdgyBossText(npc, "Exterminating the weak is a hobby of mine.");
                    }
                    break;

                case 26:
                    projectile.ai[1] = 0;
                    if (npc.ai[1] == 120)
                        EdgyBossText(npc, "Time to light it up!");
                    break;

                case 28:
                    if (npc.ai[3] == 30)
                        EdgyBossText(npc, "Now this is how we use our HEADS. Is this a bad enough time for you!?");
                    break;

                case 29:
                    EdgyBossText(npc, "ASSIST ME, FELLOW MUTANTS!");
                    break;

                case 31:
                    if (npc.ai[1] == 1)
                        EdgyBossText(npc, "I've watched your journey...");
                    break;

                case 33:
                    if (npc.ai[1] == 180)
                        EdgyBossText(npc, "And now it's time to light you up!");
                    break;

                case 35:
                    if (npc.ai[3] == 0)
                        EdgyBossText(npc, "Lovely weather, isn't it?");
                    break;

                case 36:
                    if (npc.ai[1] == 60)
                        EdgyBossText(npc, "ASSIST ME, FELLOW MUTANTS!");
                    break;

                case 38:
                    if (npc.ai[3] == 0)
                        EdgyBossText(npc, "Lovely weather, isn't it?");
                    break;

                case 39:
                    if (npc.ai[3] == 0)
                        EdgyBossText(npc, "Burn in hell!");
                    break;

                case 40:
                    if (npc.ai[1] == 179)
                        EdgyBossText(npc, "Exterminating the weak is a hobby of mine.");
                    break;

                case 41:
                    EdgyBossText(npc, "This'll leave a few Marx.");
                    break;

                default:
                    break;
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
            if (!(npc.HasValidTarget && npc.Distance(Main.player[npc.target].Center) < 5000))
                return;

            if (Main.netMode == 0)
                Main.NewText(text, Color.LimeGreen);
            else if (Main.netMode == 2)
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.LimeGreen);
        }
    }
}