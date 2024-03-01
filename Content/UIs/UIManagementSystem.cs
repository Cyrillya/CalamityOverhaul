﻿using CalamityOverhaul.Content.RemakeItems.Core;
using CalamityOverhaul.Content.UIs.SupertableUIs;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace CalamityOverhaul.Content.UIs
{
    internal class UIManagementSystem : ModSystem
    {
        public override void PostSetupContent() {
            //将自定义的UI放到最后加载，在这之前是确保物品、ID、生物等其他内容都加载完成后
            new CompressorUI().Load();
            new SupertableUI().Load();
            new RecipeUI().Load();
            new DragButton().Load();
            new InItemDrawRecipe().Load();
            new MouseTextContactPanel().Load();
            new ResetItemReminderUI().Load();
            new OverhaulTheBibleUI().Load();
            new CartridgeHolderUI().Load();
            new MagazinesUI().Load();

            OverhaulTheBibleUI.Instance.ecTypeItemList = new List<Item>();
            foreach (BaseRItem baseRItem in CWRMod.RItemInstances) {
                Item item = new Item(baseRItem.TargetID);
                if (item != null) {
                    if (item.type != ItemID.None) {
                        OverhaulTheBibleUI.Instance.ecTypeItemList.Add(item);
                    }
                }
            }
        }

        public override void SaveWorldData(TagCompound tag) {
            if (SupertableUI.Instance != null && SupertableUI.Instance?.items != null) {
                for (int i = 0; i < SupertableUI.Instance.items.Length; i++) {
                    if (SupertableUI.Instance.items[i] == null) {
                        SupertableUI.Instance.items[i] = new Item(0);
                    }
                }
                tag.Add("SupertableUI_ItemDate", SupertableUI.Instance.items);
            }
        }

        public override void LoadWorldData(TagCompound tag) {
            if (SupertableUI.Instance != null && tag.ContainsKey("SupertableUI_ItemDate")) {
                Item[] loadSupUIItems = tag.Get<Item[]>("SupertableUI_ItemDate");
                for (int i = 0; i < loadSupUIItems.Length; i++) {
                    if (loadSupUIItems[i] == null) {
                        loadSupUIItems[i] = new Item(0);
                    }
                }
                SupertableUI.Instance.items = tag.Get<Item[]>("SupertableUI_ItemDate");
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
            int mouseIndex = layers.FindIndex((GameInterfaceLayer layer) => layer.Name == "Vanilla: Mouse Text");
            if (mouseIndex != -1) {
                layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("DFs UI", delegate {
                    if (Main.LocalPlayer.CWR().CompressorPanelID != -1)
                        CompressorUI.Instance.Draw(Main.spriteBatch);
                    return true;
                }, InterfaceScaleType.UI));
                layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("Sp UI", delegate {
                    if (SupertableUI.Instance.Active) {
                        SupertableUI.Instance.Draw(Main.spriteBatch);
                        RecipeUI.Instance.Draw(Main.spriteBatch);
                        DragButton.instance.Draw(Main.spriteBatch);
                    }  
                    return true;
                }, InterfaceScaleType.UI));
                layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("Ov UI", delegate {
                    if (OverhaulTheBibleUI.Instance.Active) {
                        OverhaulTheBibleUI.Instance.Draw(Main.spriteBatch);
                    }
                    return true;
                }, InterfaceScaleType.UI));
                layers.Insert(mouseIndex, new LegacyGameInterfaceLayer("CH UI", delegate {
                    if (CartridgeHolderUI.Instance.Active) {
                        CartridgeHolderUI.Instance.Draw(Main.spriteBatch);
                    }
                    if (MagazinesUI.Instance.Active) {
                        MagazinesUI.Instance.Draw(Main.spriteBatch);
                    }
                    return true;
                }, InterfaceScaleType.UI));
            }
        }

        public override void UpdateUI(GameTime gameTime) {
            base.UpdateUI(gameTime);
            if (Main.LocalPlayer.CWR().CompressorPanelID != -1) {
                CompressorUI.Instance.Update();
            }  
            if (SupertableUI.Instance.Active) {
                SupertableUI.Instance.Update(gameTime);
                RecipeUI.Instance.Update(gameTime);
                DragButton.instance.Update(gameTime);
            }  
            if (OverhaulTheBibleUI.Instance.Active) {
                OverhaulTheBibleUI.Instance.Update(gameTime);
            }
        }
    }
}
