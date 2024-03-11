﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class ShredderEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Shredder";
        public override void SetDefaults() {
            Item.SetCalamitySD<Shredder>();
            Item.SetCartridgeGun<ShredderHeldProj>(300);
        }
    }
}
