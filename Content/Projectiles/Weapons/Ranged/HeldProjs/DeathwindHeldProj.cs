﻿using CalamityMod;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.Items.Ranged;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class DeathwindHeldProj : BaseBow
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "Deathwind";
        public override int targetCayItem => ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.Deathwind>();
        public override int targetCWRItem => ModContent.ItemType<DeathwindEcType>();
        public override void SetRangedProperty() {
            base.SetRangedProperty();
        }

        public override void HandEvent() {
            base.HandEvent();
        }

        public override void BowShoot() {
            if (CalamityUtils.CheckWoodenAmmo(AmmoTypes, Owner)) {
                SoundEngine.PlaySound(SoundID.Item12, Projectile.Center);
                for (int i = 0; i < 3; i++) {
                    int ammo = Projectile.NewProjectile(Owner.parent(), Projectile.Center, Vector2.Zero
                        , ModContent.ProjectileType<DeathLaser>(), WeaponDamage + 500, WeaponKnockback, Projectile.owner);
                    Main.projectile[ammo].ai[1] = Projectile.whoAmI;
                    Main.projectile[ammo].rotation = Projectile.rotation + MathHelper.ToRadians(5 - 5 * i);
                }
            }
            else {
                SoundEngine.PlaySound(SoundID.Item5, Projectile.Center);
                for (int i = 0; i < 3; i++) {
                    int ammo = Projectile.NewProjectile(Owner.parent(), Projectile.Center
                        , (Projectile.rotation + MathHelper.ToRadians(5 - 5 * i)).ToRotationVector2() * 17
                        , AmmoTypes, WeaponDamage, WeaponKnockback, Projectile.owner);
                    Main.projectile[ammo].MaxUpdates = 2;
                    Main.projectile[ammo].CWR().SpanTypes = (byte)SpanTypesEnum.DeadWing;
                }
                Projectile.ai[2]++;
                if (Projectile.ai[2] > 3) {
                    for (int i = 0; i < 3; i++) {
                        Vector2 vr = (Projectile.rotation + MathHelper.ToRadians(5 - 5 * i)).ToRotationVector2();
                        int ammo = Projectile.NewProjectile(Owner.parent(), Projectile.Center + vr * 150, vr * 15,
                                ModContent.ProjectileType<DeadArrow>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                        Main.projectile[ammo].scale = 1.5f;
                    }
                    Projectile.ai[2] = 0;
                }
            }
        }
    }
}
