﻿using CalamityMod.Items;
using CalamityOverhaul.Content.Events;
using CalamityOverhaul.Content.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalamityOverhaul
{
    internal class TextItem : ModItem
    {
        public override string Texture => "CalamityOverhaul/icon";
        bool old;
        public override bool IsLoadingEnabled(Mod mod) {
            return true;
        }

        public override void SetDefaults() {
            Item.width = 80;
            Item.height = 80;
            Item.damage = 9999;
            Item.DamageType = DamageClass.Default;
            Item.useAnimation = Item.useTime = 13;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2.25f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.shootSpeed = 8f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = ItemRarityID.Yellow;
        }

        public override void UpdateInventory(Player player) {
            //player.velocity.Domp();
            bool news = player.PressKey(false);
            if (news && !old) {
                player.QuickSpawnItem(player.parent(), Main.HoverItem, Main.HoverItem.stack);
            }
            old = news;

        }

        public override void HoldItem(Player player) {
        }

        /// <summary>
        /// 向弹匣中装入子弹的函数
        /// </summary>
        public Item[] LoadBulletsIntoMagazine(Player player) {
            List<Item> loadedItems = new List<Item>();
            int magazineCapacity = 20000;
            int accumulatedAmount = 0;

            foreach (Item ammoItem in player.GetAmmoState(AmmoID.Bullet).InItemInds) {
                int stack = ammoItem.stack;

                if (stack > magazineCapacity - accumulatedAmount) {
                    stack = magazineCapacity - accumulatedAmount;
                }

                loadedItems.Add(new Item(ammoItem.type, stack));
                accumulatedAmount += stack;

                if (accumulatedAmount >= magazineCapacity) {
                    break;
                }
            }

            return loadedItems.ToArray();
        }

        public override bool? UseItem(Player player) {
            if (TungstenRiot.Instance.TungstenRiotIsOngoing) {
                TungstenRiot.Instance.CloseEvent();
            }
            else {
                TungstenRiot.Instance.TryStartEvent();
            }
            return true;
        }
    }
}
