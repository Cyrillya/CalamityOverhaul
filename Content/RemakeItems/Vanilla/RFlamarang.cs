﻿using CalamityMod;
using CalamityOverhaul.Content.Projectiles.Weapons.Rogue.HeldProjs.Vanilla;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityOverhaul.Content.RemakeItems.Core;
using Microsoft.Xna.Framework;

namespace CalamityOverhaul.Content.RemakeItems.Vanilla
{
    internal class RFlamarang : BaseRItem
    {
        public override int TargetID => ItemID.Flamarang;
        public override bool IsVanilla => true;
        public override string TargetToolTipItemName => "Wap_Flamarang_Text";
        public override void SetDefaults(Item item) {
            item.DamageType = ModContent.GetInstance<RogueDamageClass>();
            item.shoot = ModContent.ProjectileType<FlamarangHeld>();
        }
        public override bool? On_CanUseItem(Item item, Player player) => player.ownedProjectileCounts[item.shoot] <= 16;
        public override bool? On_Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source
            , Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
