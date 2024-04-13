﻿using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria.ID;
using Terraria;

namespace CalamityOverhaul.Content.RemakeItems.Vanilla
{
    /// <summary>
    /// 针叶木弓
    /// </summary>
    internal class RBorealWoodBow : BaseRItem
    {
        public override int TargetID => ItemID.BorealWoodBow;
        public override bool IsVanilla => true;
        public override void SetDefaults(Item item) => item.SetHeldProj<BorealWoodBowHeldProj>();
    }
}
