﻿using CalamityMod;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Particles;
using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class SpeedBlasterHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "SpeedBlaster";
        public override int targetCayItem => ModContent.ItemType<SpeedBlaster>();
        public override int targetCWRItem => ModContent.ItemType<SpeedBlasterEcType>();
        public float DashShotDamageMult = 4.5f;
        public int DashCooldown = 300;
        public float FireRatePowerup = 1.15f;
        public float ColorValue = 0f;
        public override void SetRangedProperty() {
            kreloadMaxTime = 90;
            FireTime = 15;
            HandDistance = 25;
            HandDistanceY = 5;
            HandFireDistance = 25;
            HandFireDistanceY = -10;
            ShootPosNorlLengValue = -3;
            ShootPosToMouLengValue = 10;
            RepeatedCartridgeChange = true;
            GunPressure = 0.1f;
            ControlForce = 0.05f;
            Recoil = 1.2f;
            RangeOfStress = 25;
            AmmoTypeAffectedByMagazine = false;
            EnableRecoilRetroEffect = true;
            RecoilRetroForceMagnitude = 6;
            CanRightClick = true;
        }

        public override void PreInOwnerUpdate() {
            if (kreloadTimeValue > 0) {//设置一个特殊的装弹动作，调整转动角度和中心点，让枪身看起来上抬
                Owner.direction = ToMouse.X > 0 ? 1 : -1;//为了防止抽搐，这里额外设置一次玩家朝向
                FeederOffsetRot = -MathHelper.ToRadians(50) * DirSign;
                FeederOffsetPos = new Vector2(DirSign * -3, -25);
            }

        }

        public override void PostInOwnerUpdate() {
            base.PostInOwnerUpdate();
        }

        public override void FiringShoot() {
            GunPressure = 0.1f;
            ControlForce = 0.05f;
            Vector2 newVel = ShootVelocity.RotatedByRandom(MathHelper.ToRadians(Owner.Calamity().SpeedBlasterDashDelayCooldown > 0 ? 3f : 15f));
            float ShotMode = Owner.Calamity().SpeedBlasterDashDelayCooldown > 0 ? 2f : 0f;
            Projectile.NewProjectile(Source, GunShootPos, newVel, Item.shoot, WeaponDamage, WeaponKnockback, Owner.whoAmI, ColorValue, ShotMode);
        }

        public override void FiringShootR() {
            GunPressure = 0.1f;
            ControlForce = 0.05f;
            if (Owner.Calamity().SpeedBlasterDashDelayCooldown == 0) {
                if (ColorValue >= 4f)
                    ColorValue = 0f;
                else
                    ColorValue++;

                Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, Item.shoot, (int)(WeaponDamage * DashShotDamageMult), WeaponKnockback, Owner.whoAmI, ColorValue, 3f);

                Owner.Calamity().SpeedBlasterDashDelayCooldown = DashCooldown;
                Owner.Calamity().sBlasterDashActivated = true;
                if (Owner.velocity != Vector2.Zero) {
                    Color ColorUsed = SpeedBlasterShot.GetColor(ColorValue);
                    for (int i = 0; i <= 8; i++) {
                        CritSpark spark = new CritSpark(Owner.Center, Owner.velocity.RotatedByRandom(MathHelper.ToRadians(13f))
                            * Main.rand.NextFloat(-2.1f, -4.5f), Color.White, ColorUsed, 2f, 45, 2f, 2.5f);
                        GeneralParticleHandler.SpawnParticle(spark);
                    }
                }
            }
            else {
                GunPressure = 0;
                ControlForce = 0;
                SoundEngine.PlaySound(SpeedBlaster.Empty, Owner.Center);
            }
        }

        public override void PostFiringShoot() {
            base.PostFiringShoot();
        }
    }
}
