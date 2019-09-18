using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MasomodeEX
{
    internal partial class MasomodeEXWorld : ModWorld
    {
        public static int MutantSummons;
        public static int MutantDefeats;
        public static int MutantPlayerKills;

        public override void Initialize()
        {
            MutantSummons = 0;
            MutantDefeats = 0;
            MutantPlayerKills = 0;
        }

        public override TagCompound Save()
        {
            List<int> mutant = new List<int>
            {
                MutantSummons,
                MutantDefeats,
                MutantPlayerKills
            };

            return new TagCompound
            {
                {"mutant", mutant}
            };
        }

        public override void Load(TagCompound tag)
        {
            if (tag.ContainsKey("mutant"))
            {
                IList<int> count = tag.GetList<int>("mutant");
                MutantSummons = count[0];
                MutantDefeats = count[1];
                MutantPlayerKills = count[2];
            }
        }

        public override void NetReceive(BinaryReader reader)
        {
            MutantSummons = reader.ReadInt32();
            MutantDefeats = reader.ReadInt32();
            MutantPlayerKills = reader.ReadInt32();
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(MutantSummons);
            writer.Write(MutantDefeats);
            writer.Write(MutantPlayerKills);
        }

        public override void PreUpdate()
        {
            Main.expertMode = true;
            FargowiltasSouls.FargoSoulsWorld.MasochistMode = true;

            if (Main.time == 1 && Main.netMode != 1)
            {
                if (Main.dayTime) //all bosses become DG at daybreak
                    for (int i = 0; i < Main.maxNPCs; i++)
                        if (Main.npc[i].active && (Main.npc[i].boss || Main.npc[i].type == NPCID.EaterofWorldsHead))
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