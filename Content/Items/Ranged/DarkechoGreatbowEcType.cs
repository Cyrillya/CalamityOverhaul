﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class DarkechoGreatbowEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "DarkechoGreatbow";
        public override void SetDefaults() {
            Item.SetCalamitySD<DarkechoGreatbow>();
            Item.SetHeldProj<DarkechoGreatbowHeldProj>();
        }
    }
}
