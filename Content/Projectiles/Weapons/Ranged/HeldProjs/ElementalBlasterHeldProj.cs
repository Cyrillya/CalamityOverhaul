﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class ElementalBlasterHeldProj : BaseGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "ElementalBlaster";
        public override int targetCayItem => ModContent.ItemType<ElementalBlaster>();
        public override int targetCWRItem => ModContent.ItemType<ElementalBlasterEcType>();
        public override void SetRangedProperty() {
            ControlForce = 0f;
            GunPressure = 0f;
            Recoil = 0f;
            HandDistance = 20;
            HandFireDistance = 25;
            HandFireDistanceY = -5;
        }

        public override void FiringShoot() {
            int proj = Projectile.NewProjectile(Owner.parent(), Projectile.Center, ShootVelocity
                       , ModContent.ProjectileType<EnergyBlast>(), WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            Projectile.NewProjectile(Owner.parent(), Projectile.Center, ShootVelocity
                    , ModContent.ProjectileType<EnergyBlast2>(), WeaponDamage / 2, WeaponKnockback, Owner.whoAmI, 1, proj, -60);
            Projectile.NewProjectile(Owner.parent(), Projectile.Center, ShootVelocity
                    , ModContent.ProjectileType<EnergyBlast2>(), WeaponDamage / 2, 0, Owner.whoAmI, -1, proj, 60);
        }
    }
}
