﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class UniversalGenesisEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "UniversalGenesis";
        public override void SetDefaults() {
            Item.SetCalamityGunSD<UniversalGenesis>();
            Item.SetCartridgeGun<UniversalGenesisHeldProj>(60);
        }
    }
}
