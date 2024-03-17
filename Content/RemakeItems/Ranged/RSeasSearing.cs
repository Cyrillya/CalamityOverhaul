﻿using CalamityMod.Items;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Rarities;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using CalamityOverhaul.Content.Items.Ranged;
using Microsoft.Xna.Framework;

namespace CalamityOverhaul.Content.RemakeItems.Ranged
{
    internal class RSeasSearing : BaseRItem
    {
        public override int TargetID => ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.SeasSearing>();
        public override int ProtogenesisID => ModContent.ItemType<SeasSearingEcType>();
        public override string TargetToolTipItemName => "SeasSearingEcType";
        public override void SetDefaults(Item item) {
            item.damage = 40;
            item.DamageType = DamageClass.Ranged;
            item.width = 88;
            item.height = 44;
            item.useTime = 5;
            item.useAnimation = 10;
            item.reuseDelay = 16;
            item.useLimitPerAnimation = 3;
            item.useStyle = ItemUseStyleID.Shoot;
            item.noMelee = true;
            item.knockBack = 5f;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.useAmmo = AmmoID.Bullet;
            item.shoot = ModContent.ProjectileType<SeasSearingBubble>();
            item.shootSpeed = 13f;
            item.value = CalamityGlobalItem.Rarity5BuyPrice;
            item.rare = ItemRarityID.Pink;
            item.SetHeldProj<SeasSearingHeldProj>();
        }
    }
}
