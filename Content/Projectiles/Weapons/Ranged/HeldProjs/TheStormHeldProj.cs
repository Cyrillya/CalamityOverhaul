﻿using CalamityMod;
using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class TheStormHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "TheStorm";
        public override int targetCayItem => ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.TheStorm>();
        public override int targetCWRItem => ModContent.ItemType<TheStormEcType>();

        public override void PostInOwner() {
            if (onFire) {
                LimitingAngle(20, 160);
            }
            CWRUtils.ClockFrame(ref Projectile.frame, 5, 8);
        }

        public override void BowShoot() {
            for (int i = 0; i < 5; i++) {
                if (CalamityUtils.CheckWoodenAmmo(AmmoTypes, Owner)) {
                    AmmoTypes = ModContent.ProjectileType<Bolt>();
                }
                Vector2 spanPos = Projectile.Center + new Vector2(Main.rand.Next(-320, 320), Main.rand.Next(-732, -623));
                Vector2 vr = spanPos.To(Main.MouseWorld).UnitVector().RotatedBy(Main.rand.NextFloat(-0.12f, 0.12f)) * Main.rand.NextFloat(0.6f, 1.52f) * 27;
                int proj = Projectile.NewProjectile(Owner.parent(), spanPos, vr, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
                Main.projectile[proj].noDropItem = true;
                Main.projectile[proj].CWR().SpanTypes = (byte)SpanTypesEnum.TheStorm;
            }
        }

        public override void BowDraw(ref Color lightColor) {
            Main.EntitySpriteDraw(TextureValue, Projectile.Center - Main.screenPosition, CWRUtils.GetRec(TextureValue, Projectile.frame, 9), onFire ? Color.White : lightColor
                , Projectile.rotation, CWRUtils.GetOrig(TextureValue, 9), Projectile.scale, DirSign > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically);
        }
    }
}
