﻿using CalamityOverhaul.Common;
using Terraria.ModLoader;
using Terraria;
using CalamityOverhaul.Content.Items.Ranged;
using Terraria.ID;
using CalamityMod.Projectiles.Ranged;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class LeviatitanHeldProj : BaseGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Leviatitan";
        public override int targetCayItem => ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.Leviatitan>();
        public override int targetCWRItem => ModContent.ItemType<Leviatitan>();

        public override void SetRangedProperty() {
            ControlForce = 0.1f;
            GunPressure = 0.2f;
            Recoil = 1;
            HandDistance = 27;
            HandDistanceY = 5;
            HandFireDistance = 27;
            HandFireDistanceY = -8;
        }

        public override void FiringShoot() {
            if (AmmoTypes == ProjectileID.Bullet) {
                AmmoTypes = ModContent.ProjectileType<AquaBlastToxic>();
                if (Main.rand.NextBool(2)) {
                    AmmoTypes = ModContent.ProjectileType<AquaBlast>();
                }
            }
            Projectile.NewProjectile(Owner.parent(), Projectile.Center, ShootVelocity, AmmoTypes
                , WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            CaseEjection();
            _ = UpdateConsumeAmmo();
            _ = CreateRecoil();
        }
    }
}
