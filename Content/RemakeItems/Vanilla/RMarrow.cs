﻿using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria.ID;
using Terraria;

namespace CalamityOverhaul.Content.RemakeItems.Vanilla
{
    /// <summary>
    /// 骸骨弓
    /// </summary>
    internal class RMarrow : BaseRItem
    {
        public override int TargetID => ItemID.Marrow;
        public override bool IsVanilla => true;
        public override string TargetToolTipItemName => "Wap_Marrow_Text";
        public override void SetDefaults(Item item) => item.SetHeldProj<MarrowHeldProj>();
    }
}
