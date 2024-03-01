﻿using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class TyrannysEndHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "TyrannysEnd";
        public override int targetCayItem => ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.TyrannysEnd>();
        public override int targetCWRItem => ModContent.ItemType<TyrannysEnd>();
        private int bulletNum {
            get => heldItem.CWR().NumberBullets;
            set => heldItem.CWR().NumberBullets = value;
        }

        public override void SetRangedProperty() {
            kreloadMaxTime = 120;
            fireTime = 20;
            HandDistance = 45;
            HandDistanceY = 5;
            HandFireDistance = 45;
            HandFireDistanceY = -10;
            ShootPosNorlLengValue = -8;
            ShootPosToMouLengValue = 30;
            RepeatedCartridgeChange = true;
            GunPressure = 0.5f;
            ControlForce = 0.05f;
            Recoil = 13;
            RangeOfStress = 50;
        }

        public override bool WhetherStartChangingAmmunition() {
            return base.WhetherStartChangingAmmunition() && bulletNum < heldItem.CWR().AmmoCapacity && !onFire;
        }

        public override void KreloadSoundCaseEjection() {
            base.KreloadSoundCaseEjection();
        }

        public override void KreloadSoundloadTheRounds() {
            base.KreloadSoundloadTheRounds();
        }

        public override bool PreFireReloadKreLoad() {
            if (bulletNum <= 0) {

                loadingReminder = false;//在发射后设置一下装弹提醒开关，防止进行一次有效射击后仍旧弹出提示
                isKreload = false;
                if (heldItem.type != ItemID.None) {
                    heldItem.CWR().IsKreload = false;
                }

                bulletNum = 0;
            }
            return false;
        }

        public override void OnKreLoad() {
            bulletNum = heldItem.CWR().AmmoCapacity;
        }

        public override void OnSpanProjFunc() {
            SoundEngine.PlaySound(heldItem.UseSound, Projectile.Center);
            DragonsBreathRifleHeldProj.SpawnGunDust(Projectile, Projectile.Center, ShootVelocity);
            Projectile.NewProjectile(Owner.parent(), Projectile.Center, ShootVelocity
                    , ModContent.ProjectileType<BMGBullet>(), WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
        }

        public override void PostSpanProjFunc() {
            bulletNum--;
        }
    }
}
