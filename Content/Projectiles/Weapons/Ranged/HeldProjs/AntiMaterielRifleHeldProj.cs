﻿using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class AntiMaterielRifleHeldProj : TyrannysEndHeldProj
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "AntiMaterielRifle";
        public override int targetCayItem => ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.AntiMaterielRifle>();
        public override int targetCWRItem => ModContent.ItemType<AntiMaterielRifleEcType>();
        public override void SetRangedProperty() {
            kreloadMaxTime = 120;
            FireTime = 40;
            ControlForce = 0.04f;
            GunPressure = 0.25f;
            Recoil = 4.5f;
            HandDistance = 35;
            HandFireDistance = 30;
            HandFireDistanceY = -8;
            ShootPosToMouLengValue = 30;
            ShootPosNorlLengValue = -5;
            RangeOfStress = 25;
            RepeatedCartridgeChange = true;
        }
        public override void FiringShoot() {
            SoundEngine.PlaySound(Item.UseSound.Value with { Pitch = 0.3f }, Projectile.Center);
            DragonsBreathRifleHeldProj.SpawnGunDust(Projectile, GunShootPos, ShootVelocity);
            Projectile.NewProjectile(Source, GunShootPos, ShootVelocity
                    , ModContent.ProjectileType<BMGBullet>(), WeaponDamage, WeaponKnockback, Owner.whoAmI, 0, 1);
        }
    }
}
