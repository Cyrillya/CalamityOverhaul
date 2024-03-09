﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria.ModLoader;
using Terraria;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RMidasPrime : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<MidasPrime>();
        public override int ProtogenesisID => ModContent.ItemType<MidasPrimeEcType>();
        public override string TargetToolTipItemName => "MidasPrimeEcType";
        public override void SetDefaults(Item item) => item.SetCartridgeGun<MidasPrimeHeldProj>(30);
    }
}
