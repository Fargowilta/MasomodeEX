using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX.NPCs
{
    public class PhantomPortal : ModNPC
    {
        public virtual int spawn => NPCID.CultistDragonHead;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Portal");
        }

        public override void SetDefaults()
        {
            npc.width = 140;
            npc.height = 190;
            npc.lifeMax = 25000;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.aiStyle = -1;
            npc.alpha = 0;
            for (int i = 0; i < npc.buffImmune.Length; i++)
                npc.buffImmune[i] = true;
        }

        public override void AI()
        {
            if (npc.ai[0] < 0f || npc.ai[0] >= 200f)
            {
                npc.StrikeNPCNoInteraction(99999, 0f, 0);
                npc.active = false;
                return;
            }
            NPC hand = Main.npc[(int)npc.ai[0]];
            if (!hand.active || hand.type != NPCID.MoonLordHand)
            {
                npc.StrikeNPCNoInteraction(99999, 0f, 0);
                npc.active = false;
                return;
            }

            npc.Center = hand.Center;
            npc.position.Y -= 250;

            if (++npc.ai[1] > 120)
            {
                npc.ai[1] = 0;
                if (!NPC.AnyNPCs(NPCID.CultistDragonHead) && Main.netMode != 1)
                {
                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, spawn);
                    if (n != Main.maxNPCs && Main.netMode == 2)
                        NetMessage.SendData(23, -1, -1, null, n);
                }
            }

            if (npc.ai[3] > 0 && ++npc.ai[2] > 1200)
            {
                npc.ai[3] = 0;
                npc.ai[2] = 0;
                npc.netUpdate = true;
            }

            npc.dontTakeDamage = npc.ai[3] > 0;
        }

        public override bool CheckDead()
        {
            npc.ai[3] = 1;
            npc.active = true;
            npc.life = 1;
            npc.netUpdate = true;
            return false;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool PreNPCLoot()
        {
            return false;
        }
    }
}