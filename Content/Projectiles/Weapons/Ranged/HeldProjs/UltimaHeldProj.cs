﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class UltimaHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Ultima";
        public override int targetCayItem => ModContent.ItemType<Ultima>();
        public override int targetCWRItem => ModContent.ItemType<UltimaEcType>();

        private int fireIndex;
        public override void SetRangedProperty() {
            HandFireDistance = 26;
            BowArrowDrawBool = false;
            CanFireMotion = false;
        }
        public override void PostInOwner() {
            if (!CanFire) {
                fireIndex = 0;
            }
        }
        public override void BowShoot() {
            AmmoTypes = ModContent.ProjectileType<UltimaBolt>();
            if (fireIndex > 33) {
                AmmoTypes = ModContent.ProjectileType<UltimaRay>();
                if (Main.rand.NextBool(3)) {
                    AmmoTypes = ModContent.ProjectileType<UltimaSpark>();
                    base.BowShoot();
                    AmmoTypes = ModContent.ProjectileType<UltimaRay>();
                }
            }
            FireOffsetPos = ShootVelocity.GetNormalVector() * Main.rand.Next(-13, 13);
            base.BowShoot();
            fireIndex++;
        }
    }
}
