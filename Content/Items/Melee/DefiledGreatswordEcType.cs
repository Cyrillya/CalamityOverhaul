﻿using CalamityMod;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using CalamityMod.Rarities;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Melee
{
    /// <summary>
    /// 暴君巨刃
    /// </summary>
    internal class DefiledGreatswordEcType : EctypeItem, ISetupData
    {
        public override string Texture => CWRConstant.Cay_Wap_Melee + "DefiledGreatsword";

        public const float DefiledGreatswordMaxRageEnergy = 15000;

        private float rageEnergy {
            get => Item.CWR().MeleeCharge;
            set => Item.CWR().MeleeCharge = value;
        }

        private static Asset<Texture2D> rageEnergyTopAsset;
        private static Asset<Texture2D> rageEnergyBarAsset;
        private static Asset<Texture2D> rageEnergyBackAsset;
        void ISetupData.SetupData() {
            if (!Main.dedServ) {
                rageEnergyTopAsset = CWRUtils.GetT2DAsset(CWRConstant.UI + "RageEnergyTop");
                rageEnergyBarAsset = CWRUtils.GetT2DAsset(CWRConstant.UI + "RageEnergyBar");
                rageEnergyBackAsset = CWRUtils.GetT2DAsset(CWRConstant.UI + "RageEnergyBack");
            }
        }
        void ISetupData.UnLoadData() {
            rageEnergyTopAsset = null;
            rageEnergyBarAsset = null;
            rageEnergyBackAsset = null;
        }

        public override void SetDefaults() {
            Item.width = 102;
            Item.damage = 112;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 18;
            Item.useTurn = true;
            Item.knockBack = 7.5f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.height = 102;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.shoot = ModContent.ProjectileType<BlazingPhantomBlade>();
            Item.shootSpeed = 12f;
            Item.CWR().heldProjType = ModContent.ProjectileType<DefiledGreatswordHeld>();
        }

        internal static void UpdateBar(Item item) {
            if (item.CWR().MeleeCharge > DefiledGreatswordMaxRageEnergy)
                item.CWR().MeleeCharge = DefiledGreatswordMaxRageEnergy;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (Item.CWR().MeleeCharge > DefiledGreatswordMaxRageEnergy) {
                Item.CWR().MeleeCharge = DefiledGreatswordMaxRageEnergy;
            }
            if (!Item.CWR().closeCombat) {
                rageEnergy -= damage * 1.25f;
                if (rageEnergy < 0) {
                    rageEnergy = 0;
                }

                if (rageEnergy == 0) {
                    float adjustedItemScale = player.GetAdjustedItemScale(Item);
                    float ai1 = 40;
                    float velocityMultiplier = 2;
                    Projectile.NewProjectile(source, player.MountedCenter, velocity * velocityMultiplier, ModContent.ProjectileType<BlazingPhantomBlade>(), (int)(damage * 0.75)
                        , knockback * 0.5f, player.whoAmI, player.direction * player.gravDir, ai1, adjustedItemScale);
                }
                else {
                    float adjustedItemScale = player.GetAdjustedItemScale(Item);
                    for (int i = 0; i < 3; i++) {
                        float ai1 = 40 + i * 8;
                        float velocityMultiplier = 1f - i / (float)3;
                        Projectile.NewProjectile(source, player.MountedCenter, velocity * velocityMultiplier, ModContent.ProjectileType<BlazingPhantomBlade>(), (int)(damage * 0.75)
                            , knockback * 0.5f, player.whoAmI, player.direction * player.gravDir, ai1, adjustedItemScale);
                    }
                }
            }
            Item.CWR().closeCombat = false;
            return false;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.NextBool(5)) {
                _ = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.RuneWizard);
            }
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            Item.CWR().closeCombat = true;
            float addnum = hit.Damage;
            if (addnum > target.lifeMax) {
                addnum = 0;
            }
            else {
                addnum *= 2;
            }

            rageEnergy += addnum;

            player.AddBuff(ModContent.BuffType<BrutalCarnage>(), 300);
            target.AddBuff(70, 150);

            if (CWRIDs.WormBodys.Contains(target.type) && !Main.rand.NextBool(3)) {
                return;
            }
            int type = ModContent.ProjectileType<SunlightBlades>();
            int randomLengs = Main.rand.Next(150);
            for (int i = 0; i < 3; i++) {
                Vector2 offsetvr = CWRUtils.GetRandomVevtor(-15, 15, 900 + randomLengs);
                Vector2 spanPos = target.Center + offsetvr;
                int proj1 = Projectile.NewProjectile(
                    CWRUtils.parent(player), spanPos,
                    CWRUtils.UnitVector(offsetvr) * -13,
                    type, Item.damage - 50, 0, player.whoAmI);
                Vector2 offsetvr2 = CWRUtils.GetRandomVevtor(165, 195, 900 + randomLengs);
                Vector2 spanPos2 = target.Center + offsetvr2;
                int proj2 = Projectile.NewProjectile(
                    CWRUtils.parent(player), spanPos2,
                    CWRUtils.UnitVector(offsetvr2) * -13, type,
                    Item.damage - 50, 0, player.whoAmI);
                Main.projectile[proj1].extraUpdates += 1;
                Main.projectile[proj2].extraUpdates += 1;
            }
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo) {
            Item.CWR().closeCombat = true;
            int type = ModContent.ProjectileType<SunlightBlades>();
            int offsety = 180;
            for (int i = 0; i < 16; i++) {
                Vector2 offsetvr = new(-600, offsety);
                Vector2 spanPos = offsetvr + player.Center;
                _ = Projectile.NewProjectile(
                    CWRUtils.parent(player),
                    spanPos,
                    CWRUtils.UnitVector(offsetvr) * -13,
                    type,
                    Item.damage / 3,
                    0,
                    player.whoAmI
                    );
                Vector2 offsetvr2 = new(600, offsety);
                Vector2 spanPos2 = offsetvr + player.Center;
                _ = Projectile.NewProjectile(
                    CWRUtils.parent(player),
                    spanPos2,
                    CWRUtils.UnitVector(offsetvr2) * -13,
                    type,
                    Item.damage / 3,
                    0,
                    player.whoAmI
                    );

                offsety -= 20;
            }
            player.AddBuff(ModContent.BuffType<BrutalCarnage>(), 300);
            target.AddBuff(70, 150);
        }

        public static void DrawRageEnergyChargeBar(Player player, float alp) {
            Item item = player.ActiveItem();
            if (item.IsAir) {
                return;
            }
            Texture2D rageEnergyTop = rageEnergyTopAsset.Value;
            Texture2D rageEnergyBar = rageEnergyBarAsset.Value;
            Texture2D rageEnergyBack = rageEnergyBackAsset.Value;
            float slp = 3;
            int offsetwid = 4;
            float max = DefiledGreatswordMaxRageEnergy;
            if (item.type == ModContent.ItemType<BlightedCleaverEcType>() || item.type == ModContent.ItemType<BlightedCleaver>()) {
                max = BlightedCleaverEcType.BlightedCleaverMaxRageEnergy;
            }
            Vector2 drawPos = CWRUtils.WDEpos(player.GetPlayerStabilityCenter() + new Vector2(rageEnergyBar.Width / -2 * slp, 135));
            Rectangle backRec = new(offsetwid, 0, (int)((rageEnergyBar.Width - (offsetwid * 2)) * (item.CWR().MeleeCharge / max)), rageEnergyBar.Height);

            Main.EntitySpriteDraw(rageEnergyBack, drawPos, null, Color.White * alp, 0, Vector2.Zero, slp, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(rageEnergyBar, drawPos + (new Vector2(offsetwid, 0) * slp)
                , backRec, Color.White * alp, 0, Vector2.Zero, slp, SpriteEffects.None, 0);

            Main.EntitySpriteDraw(rageEnergyTop, drawPos, null, Color.White * alp, 0, Vector2.Zero, slp, SpriteEffects.None, 0);
        }
    }
}
