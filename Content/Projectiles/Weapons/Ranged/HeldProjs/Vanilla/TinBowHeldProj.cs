﻿using CalamityOverhaul.Common;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla
{
    internal class TinBowHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.TinBow].Value;
        public override int targetCayItem => ItemID.TinBow;
        public override int targetCWRItem => ItemID.TinBow;
        public override void SetRangedProperty() {
            ShootSpanTypeValue = SpanTypesEnum.CopperBow;
        }

        public override void PostInOwner() {
            base.PostInOwner();
        }

        public override void BowShoot() {
            base.BowShoot();
        }
    }
}
