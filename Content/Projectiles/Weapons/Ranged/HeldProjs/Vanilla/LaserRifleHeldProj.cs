﻿using CalamityOverhaul.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla
{
    internal class LaserRifleHeldProj : BaseGun
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.LaserRifle].Value;
        public override int targetCayItem => ItemID.LaserRifle;
        public override int targetCWRItem => ItemID.LaserRifle;
        public override void SetRangedProperty() {
            ShootPosToMouLengValue = 0;
            ShootPosNorlLengValue = 0;
            HandDistance = 15;
            HandDistanceY = 0;
            GunPressure = 0.2f;
            ControlForce = 0.05f;
            Recoil = 1f;
            RangeOfStress = 48;
        }

        public override void FiringShoot() {
            Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, ProjectileID.PurpleLaser, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            _ = UpdateConsumeAmmo();
            _ = CreateRecoil();
        }
    }
}
