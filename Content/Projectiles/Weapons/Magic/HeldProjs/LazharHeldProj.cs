﻿using CalamityMod.Items.Weapons.Magic;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Magic;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Magic.HeldProjs
{
    internal class LazharHeldProj : BaseMagicGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Magic + "Lazhar";
        public override int targetCayItem => ModContent.ItemType<Lazhar>();
        public override int targetCWRItem => ModContent.ItemType<LazharEcType>();
        public override void SetMagicProperty() {
            ShootPosToMouLengValue = -30;
            ShootPosNorlLengValue = 0;
            HandDistance = 15;
            HandDistanceY = 3;
            HandFireDistance = 15;
            HandFireDistanceY = -5;
            GunPressure = 0;
            ControlForce = 0;
            Recoil = 0;
        }

        public override void FiringIncident() {
            base.FiringIncident();
        }

        public override int Shoot() {
            OffsetPos += ShootVelocity.UnitVector() * -5;
            int type = base.Shoot();
            if (Main.rand.NextBool(6)) {
                Main.projectile[type].penetrate = -1;
            }
            return type;
        }
    }
}
