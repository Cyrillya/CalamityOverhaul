﻿using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityOverhaul.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Accessories
{
    internal class ElementMuzzleBrake : ModItem
    {
        public override string Texture => CWRConstant.Item + "MuzzleBrakeIII";
        public override void SetDefaults() {
            Item.width = Item.height = 32;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice * 4;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            CWRPlayer modplayer = player.CWR();
            modplayer.LoadMuzzleBrake = true;
            modplayer.LoadMuzzleBrakeLevel = 3;
            modplayer.PressureIncrease -= 0.65f;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player) {
            return incomingItem.type != ModContent.ItemType<NeutronStarMuzzleBrake>() 
                && incomingItem.type != ModContent.ItemType<PrecisionMuzzleBrake>() 
                && incomingItem.type != ModContent.ItemType<SimpleMuzzleBrake>();
        }

        public override void AddRecipes() {
            _ = CreateRecipe()
                .AddIngredient<PrecisionMuzzleBrake>()
                .AddIngredient<LifeAlloy>(5)
                .AddIngredient(ItemID.LunarBar, 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
