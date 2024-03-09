﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria.ModLoader;
using Terraria;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RChickenCannon : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<ChickenCannon>();
        public override int ProtogenesisID => ModContent.ItemType<ChickenCannonEcType>();
        public override string TargetToolTipItemName => "ChickenCannonEcType";
        public override void SetDefaults(Item item) => item.SetCartridgeGun<ChickenCannonHeldProj>(25);
    }
}
