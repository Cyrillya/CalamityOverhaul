﻿using CalamityMod.Items;
using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityOverhaul.Content.Items.Ranged;
using CalamityMod;
using Microsoft.Xna.Framework;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RCoralCannon : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.CoralCannon>();
        public override int ProtogenesisID => ModContent.ItemType<CoralCannonEcType>();
        public override string TargetToolTipItemName => "CoralCannonEcType";
        public override void SetDefaults(Item item) {
            item.damage = 124;
            item.DamageType = DamageClass.Ranged;
            item.width = 52;
            item.height = 40;
            item.useTime = 90;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.Shoot;
            item.noMelee = true;
            item.knockBack = 7.5f;
            item.value = CalamityGlobalItem.Rarity2BuyPrice;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<SmallCoral>();
            item.shootSpeed = 10f;
            item.Calamity().canFirePointBlankShots = true;
            item.SetHeldProj<CoralCannonHeldProj>();
        }
    }
}
