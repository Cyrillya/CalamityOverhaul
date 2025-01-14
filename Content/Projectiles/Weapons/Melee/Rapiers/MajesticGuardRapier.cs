﻿using CalamityOverhaul.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Melee.Rapiers
{
    internal class MajesticGuardRapier : BaseRapiers
    {
        public override string Texture => CWRConstant.Cay_Wap_Melee + "MajesticGuard";
        public override string GlowPath => CWRConstant.Item_Melee + "MajesticGuardGlow";
        public override void SetRapiers() {
            overHitModeing = 73;
            SkialithVarSpeedMode = 3;
            drawOrig = new Vector2(0, 100);
        }

        public override void ExtraShoot() {
            if (HitNPCs.Count > 0) {
                return;
            }
            int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.UnitVector() * 13
                , ModContent.ProjectileType<MajesticGuardBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            Main.projectile[proj].rotation = Main.projectile[proj].velocity.ToRotation();
        }
    }
}
