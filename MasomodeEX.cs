using System.Collections.Generic;
using Terraria.ModLoader;

namespace MasomodeEX
{
    internal partial class MasomodeEX : Mod
    {
        internal static MasomodeEX Instance
        {
            get; private set;
        }

        internal static Mod Souls;
        internal static Mod Fargo;

        internal bool HyperLoaded;
        internal bool VeinMinerLoaded;
        internal bool LuiLoaded;

        internal static List<int> DebuffIDs;

        public override void Load()
        {
            Instance = this;
        }
        public override void Unload()
        {
            Instance = null;
            if (DebuffIDs != null)
                DebuffIDs.Clear();
        }

        public override void PostSetupContent()
        {
            Souls = ModLoader.GetMod("FargowiltasSouls");
            Fargo = ModLoader.GetMod("Fargowiltas");
            HyperLoaded = ModLoader.GetMod("HyperMode") != null;
            VeinMinerLoaded = ModLoader.GetMod("VeinMiner") != null;
            LuiLoaded = ModLoader.GetMod("Luiafk") != null;
            DebuffIDs = new List<int> { 20, 22, 23, 24, 36, 39, 44, 46, 47, 67, 68, 69, 70, 80,
                    88, 94, 103, 137, 144, 145, 148, 149, 156, 160, 163, 164, 195, 196, 197, 199 };
            DebuffIDs.Add(BuffType("Antisocial"));
            DebuffIDs.Add(BuffType("Atrophied"));
            DebuffIDs.Add(BuffType("Berserked"));
            DebuffIDs.Add(BuffType("Bloodthirsty"));
            DebuffIDs.Add(BuffType("ClippedWings"));
            DebuffIDs.Add(BuffType("Crippled"));
            DebuffIDs.Add(BuffType("CurseoftheMoon"));
            DebuffIDs.Add(BuffType("Defenseless"));
            DebuffIDs.Add(BuffType("FlamesoftheUniverse"));
            DebuffIDs.Add(BuffType("Flipped"));
            DebuffIDs.Add(BuffType("FlippedHallow"));
            DebuffIDs.Add(BuffType("Fused"));
            DebuffIDs.Add(BuffType("GodEater"));
            DebuffIDs.Add(BuffType("Guilty"));
            DebuffIDs.Add(BuffType("Hexed"));
            DebuffIDs.Add(BuffType("Infested"));
            DebuffIDs.Add(BuffType("IvyVenom"));
            DebuffIDs.Add(BuffType("Jammed"));
            DebuffIDs.Add(BuffType("Lethargic"));
            DebuffIDs.Add(BuffType("LightningRod"));
            DebuffIDs.Add(BuffType("LivingWasteland"));
            DebuffIDs.Add(BuffType("Lovestruck"));
            DebuffIDs.Add(BuffType("MarkedforDeath"));
            DebuffIDs.Add(BuffType("Midas"));
            DebuffIDs.Add(BuffType("MutantNibble"));
            DebuffIDs.Add(BuffType("NullificationCurse"));
            DebuffIDs.Add(BuffType("Oiled"));
            DebuffIDs.Add(BuffType("OceanicMaul"));
            DebuffIDs.Add(BuffType("Purified"));
            DebuffIDs.Add(BuffType("Recovering"));
            DebuffIDs.Add(BuffType("ReverseManaFlow"));
            DebuffIDs.Add(BuffType("Rotting"));
            DebuffIDs.Add(BuffType("Shadowflame"));
            DebuffIDs.Add(BuffType("SqueakyToy"));
            DebuffIDs.Add(BuffType("Stunned"));
            DebuffIDs.Add(BuffType("Swarming"));
            DebuffIDs.Add(BuffType("Unstable"));

            DebuffIDs.Add(BuffType("MutantFang"));
            DebuffIDs.Add(BuffType("MutantPresence"));

            DebuffIDs.Add(BuffType("TimeFrozen"));
        }
    }
}