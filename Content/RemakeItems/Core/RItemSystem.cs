﻿using CalamityOverhaul.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.RemakeItems.Core
{
    public delegate void On_SetDefaults_Dalegate(Item item, bool createModItem = true);
    public delegate bool On_Shoot_Dalegate(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, bool defaultResult = true);
    public delegate void On_HitNPC_Delegate(Item item, Player player, NPC target, in NPC.HitInfo hit, int damageDone);
    public delegate void On_HitPvp_Delegate(Item item, Player player, Player target, Player.HurtInfo hurtInfo);
    public delegate void On_ModifyHitNPC_Delegate(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers);
    public delegate bool On_CanUseItem_Delegate(Item item, Player player);
    public delegate bool On_PreDrawInInventory_Delegate(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale);

    internal class RItemSystem : ModSystem
    {
        public static Type itemLoaderType;

        public static MethodBase onSetDefaultsMethod;
        public static MethodBase onShootMethod;
        public static MethodBase onHitNPCMethod;
        public static MethodBase onHitPvpMethod;
        public static MethodBase onModifyHitNPCMethod;
        public static MethodBase onCanUseItemMethod;
        public static MethodBase onPreDrawInInventoryMethod;

        public override void Load() {
            itemLoaderType = typeof(ItemLoader);
            onSetDefaultsMethod = itemLoaderType.GetMethod("SetDefaults", BindingFlags.NonPublic | BindingFlags.Static);
            onShootMethod = itemLoaderType.GetMethod("Shoot", BindingFlags.Public | BindingFlags.Static);
            onHitNPCMethod = itemLoaderType.GetMethod("OnHitNPC", BindingFlags.Public | BindingFlags.Static);
            onHitPvpMethod = itemLoaderType.GetMethod("OnHitPvp", BindingFlags.Public | BindingFlags.Static);
            onModifyHitNPCMethod = itemLoaderType.GetMethod("ModifyHitNPC", BindingFlags.Public | BindingFlags.Static);
            onCanUseItemMethod = itemLoaderType.GetMethod("CanUseItem", BindingFlags.Public | BindingFlags.Static);
            onPreDrawInInventoryMethod = itemLoaderType.GetMethod("PreDrawInInventory", BindingFlags.Public | BindingFlags.Static);

            if (onSetDefaultsMethod != null) {
                MonoModHooks.Add(onSetDefaultsMethod, OnSetDefaultsHook);
            }
            if (onShootMethod != null) {
                MonoModHooks.Add(onShootMethod, OnShootHook);
            }
            if (onHitNPCMethod != null ) {
                MonoModHooks.Add(onHitNPCMethod, OnHitNPCHook);
            }
            if (onHitPvpMethod != null) {
                MonoModHooks.Add(onHitPvpMethod, OnHitPvpHook);
            }
            if (onModifyHitNPCMethod != null) {
                MonoModHooks.Add(onModifyHitNPCMethod, OnModifyHitNPCHook);
            }
            if (onCanUseItemMethod != null) {
                MonoModHooks.Add(onCanUseItemMethod, OnCanUseItemHook);
            }
            if (onPreDrawInInventoryMethod != null) {
                MonoModHooks.Add(onPreDrawInInventoryMethod, OnPreDrawInInventoryHook);
            }
        }
        
        /// <summary>
        /// 这个钩子用于挂载一个提前于TML方法的SetDefaults，以此来进行一些高级的修改
        /// </summary>
        public void OnSetDefaultsHook(On_SetDefaults_Dalegate orig, Item item, bool createModItem) {//
            orig.Invoke(item);
            if (CWRConstant.ForceReplaceResetContent) {
                foreach (BaseRItem ritem in CWRMod.RItemInstances) {
                    if (ritem.SetReadonlyTargetID == item.type) {
                        ritem.On_PostSetDefaults(item);
                    }
                }
            }
        }
        /// <summary>
        /// 提前于TML的方法执行，这样继承重写<br/><see cref="BaseRItem.On_Shoot"/><br/>便拥有可以阻断TML后续方法运行的能力，用于进行一些高级修改
        /// </summary>
        public bool OnShootHook(On_Shoot_Dalegate orig, Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, bool defaultResult) {//
            if (CWRConstant.ForceReplaceResetContent) {
                foreach (BaseRItem ritem in CWRMod.RItemInstances) {
                    if (ritem.SetReadonlyTargetID == item.type) {
                        bool? rasg = ritem.On_Shoot(item, player, source, position, velocity, type, damage, knockback);
                        if (rasg.HasValue) {
                            return rasg.Value;
                        }
                    }
                }
            }

            return orig.Invoke(item, player, source, position, velocity, type, damage, knockback);
        }
        /// <summary>
        /// 提前于TML的方法执行，这个钩子可以用来做到<see cref="GlobalItem.CanUseItem"/>无法做到的修改效果，比如让一些原本不可使用的物品可以使用，
        /// <br/>继承重写<see cref="BaseRItem.On_CanUseItem(Item, Player)"/>来达到这些目的，用于进行一些高级修改
        /// </summary>
        public bool OnCanUseItemHook(On_CanUseItem_Delegate orig, Item item, Player player) {
            if (CWRConstant.ForceReplaceResetContent) {
                foreach (BaseRItem ritem in CWRMod.RItemInstances) {
                    //这个钩子的运作原理有些不同，因为这个目标函数的返回值应该直接起到作用，而不是简单的返回Void类型
                    if (ritem.SetReadonlyTargetID == item.type) {
                        //运行OnUseItem获得钩子函数的返回值，这应该起到传递的作用
                        bool? rasg = ritem.On_CanUseItem(item, player);
                        //如果rasg不为空，那么直接返回这个值让钩子的传递起效
                        if (rasg.HasValue) {
                            return rasg.Value;
                        }
                        //如果rasg不包含实际值，那么在这次枚举中就什么都不做
                    }
                }
            }
            
            return orig.Invoke(item, player);
        }

        public void OnHitNPCHook(On_HitNPC_Delegate orig, Item item, Player player, NPC target, in NPC.HitInfo hit, int damageDone) {
            if (CWRConstant.ForceReplaceResetContent) {
                foreach (BaseRItem ritem in CWRMod.RItemInstances) {
                    if (ritem.SetReadonlyTargetID == item.type) {
                        bool? rasg = ritem.On_OnHitNPC(item, player, target, hit, damageDone);
                        if (rasg.HasValue) {
                            if (!rasg.Value) {
                                return;
                            }
                        }
                    }
                }
            }
            
            orig.Invoke(item, player, target, hit, damageDone);
        }

        public void OnHitPvpHook(On_HitPvp_Delegate orig, Item item, Player player, Player target, Player.HurtInfo hurtInfo) {
            if (CWRConstant.ForceReplaceResetContent) {
                foreach (BaseRItem ritem in CWRMod.RItemInstances) {
                    if (ritem.SetReadonlyTargetID == item.type) {
                        bool? rasg = ritem.On_OnHitPvp(item, player, target, hurtInfo);
                        if (rasg.HasValue) {
                            if (!rasg.Value) {
                                return;
                            }
                        }
                    }
                }
            }
            
            orig.Invoke(item, player, target, hurtInfo);
        }

        public void OnModifyHitNPCHook(On_ModifyHitNPC_Delegate orig, Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
            if (CWRConstant.ForceReplaceResetContent) {
                foreach (BaseRItem ritem in CWRMod.RItemInstances) {
                    if (ritem.SetReadonlyTargetID == item.type) {
                        bool? rasg = ritem.On_ModifyHitNPC(item, player, target, ref modifiers);
                        if (rasg.HasValue) {
                            if (rasg.Value) {//如果返回了true，那么执行原物品的该方法
                                item.ModItem?.ModifyHitNPC(player, target, ref modifiers);
                                return;
                            }
                            else {//否则返回false，那么后续的就什么都不执行
                                return;
                            }
                        }
                    }
                }
            }

            orig.Invoke(item, player, target, ref modifiers);
        }

        public bool OnPreDrawInInventoryHook(On_PreDrawInInventory_Delegate orig, Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            if (CWRConstant.ForceReplaceResetContent) {
                foreach (BaseRItem ritem in CWRMod.RItemInstances) {
                    if (ritem.SetReadonlyTargetID == item.type) {
                        bool rasg = ritem.On_PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
                        if (!rasg) {
                            return false;
                        }
                    }
                }
            }

            return orig.Invoke(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
}
