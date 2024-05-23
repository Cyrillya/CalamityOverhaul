﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class BarinauticalEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Barinautical";
        public override void SetDefaults() {
            Item.SetCalamitySD<Barinautical>();
            Item.SetHeldProj<BarinauticalHeldProj>();
        }
    }
}
