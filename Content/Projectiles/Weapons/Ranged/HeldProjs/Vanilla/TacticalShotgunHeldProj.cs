﻿using CalamityOverhaul.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla
{
    internal class TacticalShotgunHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.TacticalShotgun].Value;
        public override int targetCayItem => ItemID.TacticalShotgun;
        public override int targetCWRItem => ItemID.TacticalShotgun;
        public override void SetRangedProperty() {
            FireTime = 35;
            ShootPosToMouLengValue = 0;
            ShootPosNorlLengValue = -4;
            HandDistance = 17;
            HandDistanceY = 4;
            GunPressure = 0.5f;
            ControlForce = 0.05f;
            Recoil = 1.4f;
            RangeOfStress = 12;
            RepeatedCartridgeChange = true;
            kreloadMaxTime = 145;
            EnableRecoilRetroEffect = true;
            RecoilRetroForceMagnitude = 7;
        }

        public override void PreInOwnerUpdate() {
            LoadingAnimation(30, 0, 13);
        }

        public override bool KreLoadFulfill() {
            return true;
        }

        public override void PostFiringShoot() {
        }

        public override void FiringShoot() {
            SpawnGunFireDust();
            _ = Projectile.NewProjectile(Owner.parent(), GunShootPos, ShootVelocity, AmmoTypes, WeaponDamage, WeaponKnockback * 1.5f, Owner.whoAmI, 0);
            for (int i = 0; i < 11; i++) {
                UpdateMagazineContents();
                _ = Projectile.NewProjectile(Owner.parent(), GunShootPos, ShootVelocity.RotatedBy(Main.rand.NextFloat(-0.12f, 0.12f)) * Main.rand.NextFloat(0.7f, 1.5f), AmmoTypes, WeaponDamage, WeaponKnockback * 1.5f, Owner.whoAmI, 0);
            }
        }
    }
}
