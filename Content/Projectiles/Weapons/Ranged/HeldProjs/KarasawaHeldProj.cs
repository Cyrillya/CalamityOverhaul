﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class KarasawaHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Karasawa";
        public override int targetCayItem => ModContent.ItemType<Karasawa>();
        public override int targetCWRItem => ModContent.ItemType<KarasawaEcType>();

        public override void SetRangedProperty() {
            kreloadMaxTime = 90;
            FireTime = 30;
            HandDistance = 30;
            HandDistanceY = -5;
            HandFireDistance = 30;
            HandFireDistanceY = -10;
            ShootPosNorlLengValue = -8;
            ShootPosToMouLengValue = 30;
            RepeatedCartridgeChange = true;
            MustConsumeAmmunition = true;
            GunPressure = 0;
            ControlForce = 0;
            Recoil = 2.2f;
            RangeOfStress = 25;
        }

        public override void PreInOwnerUpdate() {
            LoadingAnimation(50, 3, 25);
        }

        public override void FiringShoot() {
            SpawnGunFireDust(dustID1: 187, dustID2: 229);
            OffsetPos -= ShootVelocity.UnitVector() * 18;
            Projectile.NewProjectile(Owner.parent(), Projectile.Center, ShootVelocity, Item.shoot, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
        }

        public override void PostFiringShoot() {
            base.PostFiringShoot();
            CaseEjection();
        }
    }
}
