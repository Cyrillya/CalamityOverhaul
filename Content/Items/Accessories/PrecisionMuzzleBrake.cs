﻿using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityOverhaul.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Accessories
{
    internal class PrecisionMuzzleBrake : ModItem
    {
        public override string Texture => CWRConstant.Item + "MuzzleBrakeII";
        public override void SetDefaults() {
            Item.width = Item.height = 32;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice * 2;
            Item.rare = ModContent.RarityType<Turquoise>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual) {
            CWRPlayer modplayer = player.CWR();
            modplayer.LoadMuzzleBrake = true;
            modplayer.LoadMuzzleBrakeLevel = 2;
            modplayer.PressureIncrease -= 0.55f;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player) {
            return incomingItem.type != ModContent.ItemType<NeutronStarMuzzleBrake>() 
                && incomingItem.type != ModContent.ItemType<ElementMuzzleBrake>() 
                && incomingItem.type != ModContent.ItemType<SimpleMuzzleBrake>();
        }

        public override void AddRecipes() {
            _ = CreateRecipe()
                .AddIngredient<SimpleMuzzleBrake>()
                .AddIngredient<PlasmaDriveCore>()
                .AddIngredient(ItemID.HallowedBar, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
