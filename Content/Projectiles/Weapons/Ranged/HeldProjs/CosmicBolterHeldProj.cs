﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class CosmicBolterHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "CosmicBolter";
        public override int targetCayItem => ModContent.ItemType<CosmicBolter>();
        public override int targetCWRItem => ModContent.ItemType<CosmicBolterEcType>();
        public override void SetRangedProperty() {
            BowArrowDrawNum = 3;
        }
        public override void BowShoot() {
            if (AmmoTypes == ProjectileID.WoodenArrowFriendly) {
                AmmoTypes = ModContent.ProjectileType<LunarBolt2>();
            }
            for (int i = 0; i < 3; i++) {
                FireOffsetPos = ShootVelocity.GetNormalVector() * ((-1 + i) * 8);
                base.BowShoot();
            }
        }
    }
}