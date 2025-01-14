﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RHalleysInferno : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<HalleysInferno>();
        public override int ProtogenesisID => ModContent.ItemType<HalleysInfernoEcType>();
        public override string TargetToolTipItemName => "HalleysInfernoEcType";
        public override void SetDefaults(Item item) {
            item.SetCartridgeGun<HalleysInfernoHeldProj>(26);
            item.CWR().CartridgeEnum = CartridgeUIEnum.JAR;
        }
    }
}
