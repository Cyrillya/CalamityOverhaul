﻿using CalamityOverhaul.Common;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla
{
    internal class StarCannonHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.StarCannon].Value;
        public override int targetCayItem => ItemID.StarCannon;
        public override int targetCWRItem => ItemID.StarCannon;

        public override void SetRangedProperty() {
            kreloadMaxTime = 60;
            FireTime = 30;
            ShootPosToMouLengValue = 30;
            ShootPosNorlLengValue = 0;
            HandDistance = 15;
            HandDistanceY = 5;
            GunPressure = 0.2f;
            ControlForce = 0.05f;
            Recoil = 0.6f;
            RangeOfStress = 8;
            EnableRecoilRetroEffect = true;
            RecoilRetroForceMagnitude = 6;
            RepeatedCartridgeChange = true;
            SpwanGunDustMngsData.dustID1 = 15;
            SpwanGunDustMngsData.dustID2 = 57;
            SpwanGunDustMngsData.dustID3 = 58;
        }

        public override bool KreLoadFulfill() {
            FireTime = 30;
            return true;
        }

        public override void HanderPlaySound() {
            SoundEngine.PlaySound(CWRSound.Gun_50CAL_Shoot 
                with { Volume = 0.2f, Pitch = -0.3f }, Projectile.Center);
        }

        public override void SetShootAttribute() {
            if (FireTime > 10) {
                FireTime--;
            }
        }

        public override void FiringShoot() {
            int proj = Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, Item.shoot, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = -1;
        }
    }
}
