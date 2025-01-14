﻿using CalamityMod.Items.Materials;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Magic;
using CalamityOverhaul.Content.Projectiles.Weapons.Magic.HeldProjs;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Magic.Extras
{
    internal class RainbowMarshmallows : ModItem
    {
        public override string Texture => CWRConstant.Item_Magic + "Marshmallow";
        public override bool IsLoadingEnabled(Mod mod) {
            return false;
        }
        public override void SetDefaults() {
            Item.damage = 65;
            Item.knockBack = 1f;
            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.DamageType = DamageClass.Magic;
            Item.width = 46;
            Item.height = 46;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Terraria.Item.buyPrice(0, 15, 3, 15);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item117;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shootSpeed = 7;
            Item.mana = 22;
            Item.shoot = ModContent.ProjectileType<MarshmallowProj>();
            Item.SetHeldProj<RainbowMarshmallowsHeldProj>();
        }

        public override void AddRecipes() {
            CreateRecipe().
                AddIngredient<Marshmallows>().
                //AddIngredient<Polterplasm>(16).
                AddIngredient<GalacticaSingularity>(6).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}
