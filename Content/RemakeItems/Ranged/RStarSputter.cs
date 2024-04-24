﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria.ModLoader;
using Terraria;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RStarSputter : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<StarSputter>();
        public override int ProtogenesisID => ModContent.ItemType<StarSputterEcType>();
        public override string TargetToolTipItemName => "StarSputterEcType";
        public override void SetDefaults(Item item) => item.SetCartridgeGun<StarSputterHeldProj>(42);
    }
}
