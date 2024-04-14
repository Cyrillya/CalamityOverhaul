﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class CosmicBolterEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "CosmicBolter";
        public override void SetDefaults() {
            Item.SetCalamitySD<CosmicBolter>();
            Item.SetHeldProj<CosmicBolterHeldProj>();
        }
    }
}
