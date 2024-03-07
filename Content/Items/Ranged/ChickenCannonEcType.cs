﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class ChickenCannonEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "ChickenCannon";
        public override void SetDefaults() {
            Item.SetCalamityGunSD<ChickenCannon>();
            Item.SetCartridgeGun<ChickenCannonHeldProj>();
        }
    }
}
