﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class ConferenceCallEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "ConferenceCall";
        public override void SetDefaults() {
            Item.SetCalamitySD<ConferenceCall>();
            Item.SetCartridgeGun<ConferenceCallHeldProj>(85);
        }
    }
}
