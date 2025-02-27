﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RNeedler : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<Needler>();
        public override int ProtogenesisID => ModContent.ItemType<NeedlerEcType>();
        public override string TargetToolTipItemName => "NeedlerEcType";
        public override void SetDefaults(Item item) => item.SetCartridgeGun<NeedlerHeldProj>(50);
    }
}
