﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class TelluricGlareHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "TelluricGlare";
        public override int targetCayItem => ModContent.ItemType<TelluricGlare>();
        public override int targetCWRItem => ModContent.ItemType<TelluricGlareEcType>();
        private int fireIndex;
        public override void SetRangedProperty() => HandFireDistance = 20;
        public override void SetShootAttribute() {
            Item.useTime = 5;
            fireIndex++;
            if (fireIndex > 8) {
                Item.useTime = 30;
                fireIndex = 0;
            }
            AmmoTypes = ModContent.ProjectileType<TelluricGlareArrow>();
        }
        public override void BowShoot() {
            Vector2 norlShoot = ShootVelocity.UnitVector();
            FireOffsetPos = norlShoot * -53 + 
                norlShoot.GetNormalVector() * Main.rand.Next(-16, 16);
            base.BowShoot();
        }
    }
}
