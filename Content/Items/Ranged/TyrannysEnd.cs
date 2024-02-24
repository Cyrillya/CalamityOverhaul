﻿using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Sounds;
using CalamityMod;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using System.Collections.Generic;

namespace CalamityOverhaul.Content.Items.Ranged
{
    /// <summary>
    /// 暴政之终
    /// </summary>
    internal class TyrannysEnd : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "TyrannysEnd";
        public override void SetDefaults() {
            Item.damage = 2000;
            Item.knockBack = 9.5f;
            Item.DamageType = DamageClass.Ranged;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.shoot = ProjectileID.BulletHighVelocity;
            Item.shootSpeed = 12f;
            Item.useAmmo = AmmoID.Bullet;
            Item.autoReuse = true;
            Item.width = 150;
            Item.height = 48;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.UseSound = CommonCalamitySounds.LargeWeaponFireSound;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.rare = ModContent.RarityType<Violet>();
            Item.Calamity().donorItem = true;
            Item.Calamity().canFirePointBlankShots = true;
            Item.CWR().hasHeldNoCanUseBool = true;
            Item.CWR().heldProjType = ModContent.ProjectileType<TyrannysEndHeldProj>();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            tooltips.ReplaceTooltip("[KEY]", CWRKeySystem.ADS_Key.TooltipHotkeyString(), CWRMod.Instance.Name);
        }
    }
}
