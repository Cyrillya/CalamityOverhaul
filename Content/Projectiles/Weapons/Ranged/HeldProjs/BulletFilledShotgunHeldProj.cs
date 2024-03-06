﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Microsoft.Xna.Framework;
using Mono.Cecil;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class BulletFilledShotgunHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "BulletFilledShotgun";
        public override int targetCayItem => ModContent.ItemType<BulletFilledShotgun>();
        public override int targetCWRItem => ModContent.ItemType<BulletFilledShotgunEcType>();
        public override void SetRangedProperty() {
            FireTime = 20;
            ShootPosToMouLengValue = 0;
            ShootPosNorlLengValue = 0;
            HandDistance = 17;
            HandDistanceY = 4;
            ShootPosNorlLengValue = -20;
            ShootPosToMouLengValue = 15;
            GunPressure = 0.1f;
            ControlForce = 0.05f;
            Recoil = 0.8f;
            RangeOfStress = 28;
            RepeatedCartridgeChange = true;
            kreloadMaxTime = 30;
        }

        public override void PreInOwnerUpdate() {
            if (kreloadTimeValue > 0) {//设置一个特殊的装弹动作，调整转动角度和中心点，让枪身看起来上抬
                Owner.direction = ToMouse.X > 0 ? 1 : -1;//为了防止抽搐，这里额外设置一次玩家朝向
                FeederOffsetRot = -MathHelper.ToRadians(30) * DirSign;
                FeederOffsetPos = new Vector2(0, -13);
            }
        }

        public override bool PreReloadEffects(int time, int maxItem) {
            if (kreloadTimeValue == kreloadMaxTime - 1) {
                SoundEngine.PlaySound(CWRSound.CaseEjection with { Volume = 0.6f }, Projectile.Center);
                UpdateConsumeAmmo();
            }
            return false;
        }

        public override void OnKreLoad() {
            if (BulletNum < Item.CWR().AmmoCapacity) {
                if (!onFire) {
                    OnKreload = true;
                    kreloadTimeValue = kreloadMaxTime;
                }
                BulletNum++;
            }
            if (Item.CWR().AmmoCapacityInFire) {
                Item.CWR().AmmoCapacityInFire = false;
            }
        }

        public override void FiringShoot() {
            SpawnGunFireDust();
            int bulletAmt = Main.rand.Next(25, 35);
            for (int i = 0; i < bulletAmt; i++) {
                float newSpeedX = ShootVelocity.X + Main.rand.NextFloat(-15f, 15f);
                float newSpeedY = ShootVelocity.Y + Main.rand.NextFloat(-15f, 15f);
                Projectile.NewProjectile(Source, GunShootPos, new Vector2(newSpeedX, newSpeedY), Item.shoot, WeaponDamage, WeaponKnockback, Owner.whoAmI);
            }
            _ = CreateRecoil();
        }
    }
}
