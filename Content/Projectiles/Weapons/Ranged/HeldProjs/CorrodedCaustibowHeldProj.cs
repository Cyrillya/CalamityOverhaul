﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class CorrodedCaustibowHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "CorrodedCaustibow";
        public override int targetCayItem => ModContent.ItemType<CorrodedCaustibow>();
        public override int targetCWRItem => ModContent.ItemType<CorrodedCaustibowEcType>();
        public override void SetRangedProperty() {
            base.SetRangedProperty();
        }
        public override void BowShoot() {
            if (AmmoTypes == ProjectileID.WoodenArrowFriendly) {
                AmmoTypes = ModContent.ProjectileType<CorrodedShell>();
            }
            base.BowShoot();
        }
    }
}
