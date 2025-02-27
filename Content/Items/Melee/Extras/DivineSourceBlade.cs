﻿using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Melee.Extras
{
    internal class DivineSourceBlade : ModItem
    {
        public override string Texture => CWRConstant.Item_Melee + "DivineSourceBlade";

        public Texture2D Value => CWRUtils.GetT2DValue(Texture);

        public override void SetDefaults() {
            Item.height = 154;
            Item.width = 154;
            Item.damage = 480;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = Item.useTime = 15;
            Item.scale = 1;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.UseSound = SoundID.Item60;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(0, 33, 15, 0);
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<DivineSourceBladeProjectile>();
            Item.shootSpeed = 18f;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit) => crit += 10;

        public override void UseAnimation(Player player) {
            int types = ModContent.ProjectileType<DivineSourceBeam>();
            Vector2 vector2 = player.Center.To(Main.MouseWorld).UnitVector() * 3;
            Vector2 position = player.Center;
            Projectile.NewProjectile(
                player.parent(), position, vector2, types
                , (int)(Item.damage * 1.25f)
                , Item.knockBack
                , player.whoAmI);
        }

        public override void AddRecipes() {
            CreateRecipe().
                AddIngredient<AuricBar>(5).
                AddIngredient<CalamityMod.Items.Weapons.Melee.Terratomere>().
                AddIngredient<CalamityMod.Items.Weapons.Melee.Excelsus>().
                AddTile(ModContent.TileType<CosmicAnvil>()).
                Register();
        }
    }
}
