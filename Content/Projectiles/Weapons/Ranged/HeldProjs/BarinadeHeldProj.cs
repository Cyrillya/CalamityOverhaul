﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class BarinadeHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Barinade";
        public override int targetCayItem => ModContent.ItemType<Barinade>();
        public override int targetCWRItem => ModContent.ItemType<BarinadeEcType>();
        public override void SetRangedProperty() {
            BowArrowDrawNum = 2;
            HandDistanceY = 5;
        }

        public override void BowShoot() {
            Projectile.NewProjectile(Source, Projectile.Center + ShootVelocity.RotatedBy(-0.55f), ShootVelocity.RotatedBy(0.025f)
                , ModContent.ProjectileType<BarinadeArrow>(), WeaponDamage, WeaponKnockback, Owner.whoAmI);
            Projectile.NewProjectile(Source, Projectile.Center + ShootVelocity.RotatedBy(0.55f), ShootVelocity.RotatedBy(-0.025f)
                , ModContent.ProjectileType<BarinadeArrow>(), WeaponDamage, WeaponKnockback, Owner.whoAmI);
        }
    }
}
