﻿using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Summon.Whips;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Items.Summon.Extras
{
    internal class GhostFireWhip : ModItem
    {
        public override string Texture => CWRConstant.Item_Summon + "GhostFireWhip";

        public override void SetDefaults() {
            Item.DefaultToWhip(ModContent.ProjectileType<GhostFireWhipProjectile>(), 220, 1, 12, 30);
            Item.rare = ItemRarityID.Purple;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.value = Terraria.Item.buyPrice(0, 16, 5, 75);
            Item.rare = 9;
        }

        public override bool MeleePrefix() {
            return true;
        }
    }
}
