﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class SvantechnicalHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Svantechnical";
        public override int targetCayItem => ModContent.ItemType<Svantechnical>();
        public override int targetCWRItem => ModContent.ItemType<SvantechnicalEcType>();

        public override void SetRangedProperty() {
            kreloadMaxTime = 90;
            FireTime = 15;
            HandDistance = 25;
            HandDistanceY = 5;
            HandFireDistance = 25;
            HandFireDistanceY = -10;
            ShootPosNorlLengValue = 0;
            ShootPosToMouLengValue = 30;
            RepeatedCartridgeChange = true;
            GunPressure = 0.1f;
            ControlForce = 0.05f;
            Recoil = 1.2f;
            RangeOfStress = 25;
            EnableRecoilRetroEffect = true;
            RecoilRetroForceMagnitude = 6;
        }

        public override void PreInOwnerUpdate() {
            LoadingAnimation(50, 3, 25);
        }

        public override void HanderSpwanDust() {
            SpawnGunFireDust(GunShootPos, ShootVelocity, 1, dustID1: DustID.FireworkFountain_Blue
                , dustID2: DustID.FireworkFountain_Blue, dustID3: DustID.FireworkFountain_Blue);
        }

        public override void FiringShoot() {
            Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            Projectile.NewProjectile(Source, GunShootPos, ShootVelocity * 1.1f, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            Projectile.NewProjectile(Source, GunShootPos, ShootVelocity * 1.2f, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            Vector2 spanPos = GunShootPos + ShootVelocity.UnitVector() * GunShootPos.To(Main.MouseWorld).Length();
            for (int i = 0; i < 23; i++) {
                int proj = Projectile.NewProjectile(Source, spanPos, ShootVelocity.RotatedByRandom(0.12f), AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
                Main.projectile[proj].timeLeft = 90;
                Main.projectile[proj].maxPenetrate *= 2;
                Main.projectile[proj].usesLocalNPCImmunity = true;
                Main.projectile[proj].localNPCHitCooldown = 5;
            }
        }
    }
}
