﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class HandheldTankHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "HandheldTank";
        public override int targetCayItem => ModContent.ItemType<HandheldTank>();
        public override int targetCWRItem => ModContent.ItemType<HandheldTankEcType>();
        public override void SetRangedProperty() {
            FireTime = 30;
            kreloadMaxTime = 60;
            ShootPosToMouLengValue = 0;
            ShootPosNorlLengValue = 0;
            HandDistance = 60;
            HandDistanceY = 4;
            HandFireDistance = 60;
            ShootPosNorlLengValue = -10;
            ShootPosToMouLengValue = 25;
            GunPressure = 0.1f;
            ControlForce = 0.03f;
            Recoil = 3.5f;
            RangeOfStress = 28;
            RepeatedCartridgeChange = true;
        }

        public override void PreInOwnerUpdate() {
            LoadingAnimation(30, 0, 13);
        }

        public override void PostInOwnerUpdate() {
            if (!Owner.PressKey() && kreloadTimeValue == 0) {
                ArmRotSengsFront = 70 * CWRUtils.atoR;
                ArmRotSengsBack = 110 * CWRUtils.atoR;
            }
        }

        public override void FiringShoot() {
            SpawnGunFireDust();
            Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, Item.shoot, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            _ = CreateRecoil();
        }
    }
}
