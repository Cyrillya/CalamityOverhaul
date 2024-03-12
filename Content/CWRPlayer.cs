﻿using CalamityMod;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items;
using CalamityOverhaul.Content.Items.Magic.Extras;
using CalamityOverhaul.Content.Items.Materials;
using CalamityOverhaul.Content.Items.Summon.Extras;
using CalamityOverhaul.Content.Items.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CalamityOverhaul.Content
{
    public class CWRPlayer : ModPlayer
    {
        public bool InitialCreation;
        /// <summary>
        /// 圣物的装备等级，这个字段决定了玩家会拥有什么样的弹幕效果
        /// </summary>
        public int TheRelicLuxor = 0;
        /// <summary>
        /// 是否装备制动器
        /// </summary>
        public bool LoadMuzzleBrake;
        /// <summary>
        /// 装备的制动器等级
        /// </summary>
        public int LoadMuzzleBrakeLevel;
        /// <summary>
        /// 应力缩放
        /// </summary>
        public float PressureIncrease;
        /// <summary>
        /// 摄像头位置额外矫正值
        /// </summary>
        public Vector2 OffsetScreenPos;
        //未使用的，这个属性属于一个未完成的UI
        public int CompressorPanelID = -1;
        /// <summary>
        /// 是否开启超级合成台
        /// </summary>
        public bool SupertableUIStartBool;
        /// <summary>
        /// 玩家是否坐在大排档塑料椅子之上
        /// </summary>
        public bool InFoodStallChair;
        /// <summary>
        /// 玩家是否装备休谟稳定器
        /// </summary>
        public bool EndlessStabilizerBool;
        /// <summary>
        /// 玩家是否手持鬼妖
        /// </summary>
        public bool HeldMurasamaBool;
        /// <summary>
        /// 玩家是否正在进行终结技
        /// </summary>
        public bool EndSkillEffectStartBool;
        /// <summary>
        /// 升龙技冷却时间
        /// </summary>
        public int RisingDragonCoolDownTime;
        /// <summary>
        /// 是否受伤
        /// </summary>
        public bool OnHit;

        #region 网络同步
        public bool DompBool;
        public bool RecoilAccelerationAddBool;
        public Vector2 RecoilAccelerationValue;
        #endregion

        public override void Initialize() {
            OnHit = false;
            TheRelicLuxor = 0;
            PressureIncrease = 1;
            LoadMuzzleBrake = false;
            InitialCreation = true;
        }

        public override void ResetEffects() {
            TheRelicLuxor = 0;
            LoadMuzzleBrakeLevel = 0;
            PressureIncrease = 1;
            OnHit = false;
            InFoodStallChair = false;
            EndlessStabilizerBool = false;
            HeldMurasamaBool = false;
            EndSkillEffectStartBool = false;
            LoadMuzzleBrake = false;
            OffsetScreenPos = Vector2.Zero;
        }

        public override void SaveData(TagCompound tag) {
            tag.Add("_InitialCreation", InitialCreation);
        }

        public override void LoadData(TagCompound tag) {
            InitialCreation = tag.GetBool("_InitialCreation");
        }

        public override void OnEnterWorld() {
            if (CWRServerConfig.Instance.ForceReplaceResetContent) {
                CWRUtils.Text(CWRMod.RItemIndsDict.Count + CWRLocText.GetTextValue("OnEnterWorld_TextContent"), Color.GreenYellow);
            }
            if (InitialCreation) {
                for (int i = 0; i < Player.inventory.Length; i++) {
                    if (Player.inventory[i].type == ItemID.CopperAxe) {
                        Player.inventory[i] = new Item(ModContent.ItemType<PebbleAxe>());
                    }
                    if (Player.inventory[i].type == ItemID.CopperPickaxe) {
                        Player.inventory[i] = new Item(ModContent.ItemType<PebblePick>());
                    }
                    if (Player.inventory[i].type == ItemID.CopperBroadsword 
                        || Player.inventory[i].type == ItemID.CopperShortsword) {
                        Player.inventory[i] = new Item(ModContent.ItemType<PebbleSpear>());
                    }
                }
                InitialCreation = false;
            }
        }

        public override void OnHurt(Player.HurtInfo info) {
            OnHit = true;
        }

        public override void PostUpdate() {
            if (Player.sitting.TryGetSittingBlock(Player, out Tile t)) {
                if (t.TileType == CWRIDs.FoodStallChairTile) {
                    InFoodStallChair = true;
                    Main.raining = true;
                    Main.maxRaining = 0.99f;
                    Main.cloudAlpha = 0.99f;
                    Main.windSpeedTarget = 0.8f;
                    float sengs = Math.Abs(MathF.Sin(Main.GameUpdateCount * 0.05f));
                    Lighting.AddLight(Player.Center, new Color(Main.DiscoB, Main.DiscoG, 220 + sengs * 30).ToVector3() * sengs * 113);
                    PunchCameraModifier modifier2 = new PunchCameraModifier(Player.Center, new Vector2(0, Main.rand.NextFloat(-2, 2)), 2f, 3f, 2, 1000f, FullName);
                    Main.instance.CameraModifiers.Add(modifier2);
                }
            }
            if (DompBool) {
                $"{Player.name}成功进行网络同步".Domp();
                DompBool = false;
            }
            if (RecoilAccelerationAddBool) {
                Player.velocity += RecoilAccelerationValue;
                RecoilAccelerationAddBool = false;
            }
        }

        internal void HandleRecoilAcceleration(BinaryReader reader) {
            RecoilAccelerationAddBool = reader.ReadBoolean();
            RecoilAccelerationValue.X = reader.ReadSingle();
            RecoilAccelerationValue.Y = reader.ReadSingle();
            if (Main.netMode == NetmodeID.Server)
                SyncRecoilAcceleration(true);
        }

        public void SyncRecoilAcceleration(bool server) {
            ModPacket packet = Mod.GetPacket(256);
            packet.Write((byte)CWRMessageType.RecoilAcceleration);
            packet.Write(Player.whoAmI);
            packet.Write(RecoilAccelerationAddBool);
            packet.Write(RecoilAccelerationValue.X);
            packet.Write(RecoilAccelerationValue.Y);
            Player.SendPacket(packet, server);
        }

        internal void HandleDomp(BinaryReader reader) {
            DompBool = reader.ReadBoolean();
            if (Main.netMode == NetmodeID.Server)
                SyncDomp(true);
        }

        public void SyncDomp(bool server) {
            ModPacket packet = Mod.GetPacket(256);
            packet.Write((byte)CWRMessageType.DompBool);
            packet.Write(Player.whoAmI);
            packet.Write(DompBool);
            Player.SendPacket(packet, server);
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage) {
            if (LoadMuzzleBrake) {
                if (item.DamageType == DamageClass.Ranged) {
                    if (LoadMuzzleBrakeLevel == 1) {
                        damage *= 0.75f;
                    }
                    else if (LoadMuzzleBrakeLevel == 2) {
                        damage *= 0.8f;
                    }
                    else if (LoadMuzzleBrakeLevel == 3) {
                        damage *= 0.85f;
                    }
                    else if (LoadMuzzleBrakeLevel == 4) {
                        damage *= 2;
                    }
                }
            }
        }

        public override void ModifyScreenPosition() {
            Main.screenPosition += OffsetScreenPos;
        }

        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath) {
            yield return new Item(ModContent.ItemType<PebbleSpear>());
            yield return new Item(ModContent.ItemType<PebblePick>());
            yield return new Item(ModContent.ItemType<PebbleAxe>());
            yield return new Item(ModContent.ItemType<TheSpiritFlint>());
            yield return new Item(ModContent.ItemType<TheUpiStele>());
            yield return new Item(ModContent.ItemType<Pebble>(), 999);
            yield return new Item(ModContent.ItemType<OverhaulTheBibleBook>());
        }

        public override void ModifyStartingInventory(IReadOnlyDictionary<string, List<Item>> itemsByMod, bool mediumCoreDeath) {
            if (!mediumCoreDeath) {
                itemsByMod["Terraria"].RemoveAll(item => item.type == ItemID.CopperAxe);
                itemsByMod["Terraria"].RemoveAll(item => item.type == ItemID.CopperShortsword);
                itemsByMod["Terraria"].RemoveAll(item => item.type == ItemID.CopperPickaxe);
            }
        }
    }
}
