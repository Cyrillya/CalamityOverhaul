﻿using CalamityMod;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.UIs.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.UIs
{
    internal class CartridgeHolderUI : CWRUIPanel
    {
        public static class Date
        {
            public static float JARSengs;
        }

        public static CartridgeHolderUI Instance;
        public override Texture2D Texture => CWRUtils.GetT2DValue("CalamityOverhaul/Assets/UIs/BulletCard" + (handItem.CWR().AmmoCapacityInFire ? "_Fire" : ""));
        private Item handItem => player.ActiveItem();
        private int bulletNum => player.ActiveItem().CWR().NumberBullets;
        private bool onMainP;
        public bool Active {
            get {
                if (handItem.type == ItemID.None) {
                    return false;
                }
                return handItem.CWR().HasCartridgeHolder;
            }
        }

        public override void Load() => Instance = this;

        public override void Initialize() {
            if (handItem.CWR().CartridgeEnum == CartridgeUIEnum.CartridgeHolder)
                DrawPos = new Vector2(20, Main.screenHeight - 100);
            if (handItem.CWR().CartridgeEnum == CartridgeUIEnum.Magazines)
                DrawPos = new Vector2(60, Main.screenHeight - 100);
            if (handItem.CWR().CartridgeEnum == CartridgeUIEnum.JAR)
                DrawPos = new Vector2(60, Main.screenHeight - 100);

            DrawPos += new Vector2(CWRServerConfig.Instance.CartridgeUI_Offset_X_Value, -CWRServerConfig.Instance.CartridgeUI_Offset_Y_Value);

            onMainP = new Rectangle((int)DrawPos.X, (int)DrawPos.Y, Texture.Width, Texture.Height).Intersects(new Rectangle((int)MouPos.X, (int)MouPos.Y, 1, 1));
        }

        public override void Draw(SpriteBatch spriteBatch) {
            Initialize();
            CWRItems cwrItem = handItem.CWR();
            if (cwrItem.CartridgeEnum == CartridgeUIEnum.CartridgeHolder) {
                spriteBatch.Draw(Texture, DrawPos, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, bulletNum.ToString()
                    , DrawPos.X + 50, DrawPos.Y + 0, Color.AliceBlue, Color.Black, Vector2.Zero, 1.3f);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, "Max"
                    , DrawPos.X + 50, DrawPos.Y + 22, Color.Gold, Color.Black, Vector2.Zero, 1f);
                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, cwrItem.AmmoCapacity.ToString()
                    , DrawPos.X + 85, DrawPos.Y + 22, Color.Gold, Color.Black, Vector2.Zero, 1.05f);
            }
            if (cwrItem.CartridgeEnum == CartridgeUIEnum.Magazines) {
                Texture2D texture2 = CWRUtils.GetT2DValue("CalamityOverhaul/Assets/UIs/Magazines");
                spriteBatch.Draw(texture2, DrawPos, CWRUtils.GetRec(texture2, 6 - bulletNum, 7), Color.White
                    , 0f, CWRUtils.GetOrig(texture2, 7), 2, SpriteEffects.None, 0);
            }
            if (cwrItem.CartridgeEnum == CartridgeUIEnum.JAR) {
                Texture2D jar = CWRUtils.GetT2DValue("CalamityOverhaul/Assets/UIs/JAR");
                Texture2D jar2 = CWRUtils.GetT2DValue("CalamityOverhaul/Assets/UIs/JAR_Full");
                Date.JARSengs = MathHelper.Lerp(Date.JARSengs, bulletNum / (float)cwrItem.AmmoCapacity, 0.05f);
                float sengs = jar2.Height * (1 - Date.JARSengs);
                Rectangle rectangle = new(0, (int)sengs, jar2.Width, (int)(jar2.Height - sengs));
                spriteBatch.Draw(jar2, DrawPos + new Vector2(0, sengs + 4), rectangle, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.Draw(jar, DrawPos, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            if (onMainP) {
                string text = "空\n";
                int value = 0;
                if (cwrItem.MagazineContents != null && cwrItem.MagazineContents.Length > 0) {
                    foreach (Item i in cwrItem.MagazineContents) {
                        if (i.type != ItemID.None && i.ammo != AmmoID.None) {
                            text += $"{i.Name} 剩余: {i.stack}\n";
                            value++;
                        }
                    }
                }

                Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.ItemStack.Value, text
                    , MouPos.X + 0, MouPos.Y - 30 - value * 30, Color.AliceBlue, Color.Black, Vector2.Zero, 1f);
            }
        }
    }
}
