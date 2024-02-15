﻿using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Projectiles.Melee;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using CalamityOverhaul.Content.Particles.Core;
using CalamityOverhaul.Content.Particles;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee.HeldProjectiles;

namespace CalamityOverhaul.Content.Items.Melee
{
    /// <summary>
    /// 邪恶收割者
    /// </summary>
    internal class BalefulHarvester : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Melee + "BalefulHarvester";
        public new string LocalizationCategory => "Items.Weapons.Melee";
        public override void SetStaticDefaults() {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void SetDefaults() {
            Item.damage = 90;
            Item.width = 74;
            Item.height = 86;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 22;
            Item.useTurn = true;
            Item.knockBack = 8f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.value = CalamityGlobalItem.Rarity10BuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<BalefulHarvesterHeldProj>();
            Item.shootSpeed = 15;
            
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<BalefulHarvesterHeldProj>()] == 0;

        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
            Item.noMelee = false;
            Item.noUseGraphic = false;
            if (player.altFunctionUse == 2) {
                Item.noMelee = true;
                Item.noUseGraphic = true;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (player.altFunctionUse == 2) {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                return false;
            }
            SoundEngine.PlaySound(SoundID.Item71, player.position);
            for (int i = 0; i < 2; i++) {
                Projectile.NewProjectile(source, position, velocity.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f)), ModContent.ProjectileType<BalefulSickle>(), damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers) {
            modifiers.CritDamage *= 0.5f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
            int type = Main.rand.NextBool() ? ModContent.ProjectileType<BalefulHarvesterProjectile>() : ProjectileID.FlamingJack;
            CalamityPlayer.HorsemansBladeOnHit(player, target.whoAmI, (int)(Item.damage * 1.5f), Item.knockBack, 0, type);
            target.AddBuff(BuffID.OnFire3, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo) {
            int type = Main.rand.NextBool() ? ModContent.ProjectileType<BalefulHarvesterProjectile>() : ProjectileID.FlamingJack;
            CalamityPlayer.HorsemansBladeOnHit(player, -1, (int)(Item.damage * 1.5f), Item.knockBack, 0, type);
            target.AddBuff(BuffID.OnFire3, 300);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if (Main.rand.NextBool(3))
                SpanDust(hitbox.TopLeft() + new Vector2(Main.rand.Next(hitbox.Width), Main.rand.Next(hitbox.Height)), 6, 0.3f, 0.5f);
        }

        public static void SpanDust(Vector2 origPos, float maxSengNum, float minScale, float maxScale) {
            float randomRot = Main.rand.NextFloat(MathHelper.TwoPi);
            for (int i = 0; i < 4; i++) {
                float rot = MathHelper.PiOver2 * i + randomRot;
                Vector2 vr = rot.ToRotationVector2();
                for (int j = 0; j < maxSengNum; j++) {
                    HeavenfallStarParticle spark = new HeavenfallStarParticle(origPos, vr * (0.1f + i * 0.1f), false, 37, Main.rand.NextFloat(minScale, maxScale), Color.DarkGoldenrod);
                    CWRParticleHandler.AddParticle(spark);
                }
            }
        }
    }
}
