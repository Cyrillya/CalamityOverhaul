﻿using CalamityMod.Items.Weapons.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs;
using Terraria;

namespace CalamityOverhaul.Content.Items.Ranged
{
    internal class AstralRepeaterEcType : EctypeItem
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "AstralRepeater";
        public override void SetDefaults() {
            Item.SetCalamitySD<AstralRepeater>();
            Item.SetHeldProj<AstralRepeaterHeldProj>();
        }
    }
}
