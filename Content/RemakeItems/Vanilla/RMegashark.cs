﻿using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using CalamityOverhaul.Content.RemakeItems.Core;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;

namespace CalamityOverhaul.Content.RemakeItems.Vanilla
{
    internal class RMegashark : BaseRItem
    {
        public override int TargetID => ItemID.Megashark;
        public override bool FormulaSubstitution => false;
        public override void On_PostSetDefaults(Item item) {
            item.useTime = 5;
            item.CWR().hasHeldNoCanUseBool = true;
            item.CWR().heldProjType = ModContent.ProjectileType<MegasharkHeldProj>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) 
            => CWRUtils.OnModifyTooltips(CWRMod.Instance, tooltips, CWRLocText.GetText("Wap_Megashark_Text"));
    }
}
