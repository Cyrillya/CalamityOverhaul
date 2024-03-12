﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria.ModLoader;
using Terraria;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RSepticSkewer : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<SepticSkewer>();
        public override int ProtogenesisID => ModContent.ItemType<SepticSkewerEcType>();
        public override string TargetToolTipItemName => "SepticSkewerEcType";
        public override void SetDefaults(Item item) => item.SetCartridgeGun<SepticSkewerHeldProj>(8);
    }
}
