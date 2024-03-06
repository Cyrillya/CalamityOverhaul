﻿using CalamityOverhaul.Common;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class StarCannonHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.StarCannon].Value;
        public override int targetCayItem => ItemID.StarCannon;
        public override int targetCWRItem => ItemID.StarCannon;

        public override void SetRangedProperty() {
            kreloadMaxTime = 120;
            FireTime = 60;
            ShootPosToMouLengValue = 0;
            ShootPosNorlLengValue = 0;
            HandDistance = 15;
            HandDistanceY = 5;
            RepeatedCartridgeChange = true;
            GunPressure = 0.05f;
            ControlForce = 0.05f;
            Recoil = -2f;
            RangeOfStress = 8;
        }

        public override void KreloadSoundCaseEjection() {
            base.KreloadSoundCaseEjection();
        }

        public override void KreloadSoundloadTheRounds() {
            base.KreloadSoundloadTheRounds();
        }

        public override void FiringShoot() {
            SpawnGunFireDust(GunShootPos, ShootVelocity, dustID1: 15, dustID2: 57, dustID3: 58);
            Projectile.NewProjectile(Owner.parent(), GunShootPos, ShootVelocity, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            UpdateConsumeAmmo();
            FireTime -= 10;
            if (FireTime < 6) {
                FireTime = 6;
            }
        }

        public override void OnKreLoad() {
            base.OnKreLoad();//装弹
            FireTime = 60;
        }
    }
}
