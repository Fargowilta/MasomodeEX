using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MasomodeEX.NPCs
{
    public class PurityPortal : PhantomPortal
    {
        public override int spawn => NPCID.AncientCultistSquidhead;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purity Portal");
        }
    }
}