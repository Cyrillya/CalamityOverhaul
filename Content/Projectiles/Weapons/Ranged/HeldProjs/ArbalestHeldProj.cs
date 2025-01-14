﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class ArbalestHeldProj : BaseGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Arbalest";
        public override int targetCayItem => ModContent.ItemType<Arbalest>();
        public override int targetCWRItem => ModContent.ItemType<ArbalestEcType>();
        private int fireIndex;
        public override void SetRangedProperty() {
            HandDistance = 25;
            HandDistanceY = 5;
            HandFireDistance = 25;
            HandFireDistanceY = -10;
            ShootPosNorlLengValue = -0;
            ShootPosToMouLengValue = 30;
            GunPressure = 0;
            ControlForce = 0;
            Recoil = 0;
            RangeOfStress = 0;
            fireIndex = 1;
            DrawCrossArrowSize = 1;
            DrawCrossArrowNorlMode = 3;
            CanRightClick = true;
            IsCrossbow = true;
        }

        public override void SetShootAttribute() {
            Item.useTime = onFire ? 6 : 7;
        }

        public override void HanderPlaySound() {
            SoundEngine.PlaySound(SoundID.Item5, Owner.Center);
        }

        public override void FiringShoot() {
            for (int i = 0; i < fireIndex; i++) {
                int proj = Projectile.NewProjectile(Source, GunShootPos, ShootVelocity.RotatedByRandom(0.12f)
                    , AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
                Main.projectile[proj].scale += fireIndex * 0.08f;
                Main.projectile[proj].extraUpdates += 1;
                Main.projectile[proj].velocity *= 1 + fireIndex * 0.1f;
                Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver2;
            }

            _ = UpdateConsumeAmmo();
        }

        public override void PostFiringShoot() {
            if (onFire) {
                fireIndex++;
                if (fireIndex >= 6) {
                    Item.useTime = 45;
                    fireIndex = 0;
                }
            }
        }

        public override void FiringShootR() {
            int proj = Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            Main.projectile[proj].extraUpdates += 2;
            Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation() + MathHelper.PiOver2;
            _ = UpdateConsumeAmmo();
        }
    }
}
