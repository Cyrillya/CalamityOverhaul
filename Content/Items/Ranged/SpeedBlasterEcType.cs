﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class SpeedBlasterEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "SpeedBlaster";
        public override void SetDefaults() {
            Item.SetCalamitySD<SpeedBlaster>();
            Item.SetCartridgeGun<SpeedBlasterHeldProj>(80);
        }
    }
}
