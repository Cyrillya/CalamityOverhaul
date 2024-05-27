﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class P90HeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "P90";
        public override int targetCayItem => ModContent.ItemType<P90>();
        public override int targetCWRItem => ModContent.ItemType<P90EcType>();

        public override void SetRangedProperty() {
            kreloadMaxTime = 90;
            FireTime = 2;
            HandDistanceY = 0;
            HandDistance = HandFireDistance = 12;
            HandFireDistanceY = -6;
            ShootPosNorlLengValue = -3;
            ShootPosToMouLengValue = -10;
            RepeatedCartridgeChange = true;
            MustConsumeAmmunition = false;
            Recoil = GunPressure = ControlForce = 0;
        }

        public override void PreInOwnerUpdate() {
            LoadingAnimation(20, 3, 5);
        }

        public override void FiringShoot() {
            Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, Item.shoot, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
        }
    }
}
