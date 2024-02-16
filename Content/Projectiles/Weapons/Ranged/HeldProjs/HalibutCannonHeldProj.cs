﻿using CalamityOverhaul.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using CalamityOverhaul.Content.Items.Ranged;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class HalibutCannonHeldProj : BaseHeldGun
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "HalibutCannon";
        public override int targetCayItem => ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.HalibutCannon>();
        public override int targetCWRItem => ModContent.ItemType<HalibutCannon>();
        public override float ControlForce => 0.1f;
        public override float GunPressure => 0.3f;
        public override float Recoil => 1.7f;
        public override void InOwner() {
            float armRotSengsFront = 60 * CWRUtils.atoR;
            float armRotSengsBack = 110 * CWRUtils.atoR;

            Projectile.Center = Owner.Center + new Vector2(DirSign * 40, 10);
            Projectile.rotation = DirSign > 0 ? MathHelper.ToRadians(10) : MathHelper.ToRadians(170);
            Projectile.timeLeft = 2;
            SetHeld();

            if (!Owner.mouseInterface) {
                if (Owner.PressKey()) {
                    Owner.direction = ToMouse.X > 0 ? 1 : -1;
                    Projectile.rotation = GunOnFireRot;
                    Projectile.Center = Owner.Center + Projectile.rotation.ToRotationVector2() * 40 + new Vector2(0, -3) + offsetPos;
                    armRotSengsBack = armRotSengsFront = (MathHelper.PiOver2 - Projectile.rotation) * DirSign;
                    if (HaveAmmo) {
                        onFire = true;
                        Projectile.ai[1]++;
                    }
                }
                else {
                    onFire = false;
                }
            }

            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, armRotSengsFront * -DirSign);
            Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, armRotSengsBack * -DirSign);
        }

        public override void SpanProj() {
            if (onFire && Projectile.ai[1] > heldItem.useTime) {
                SoundEngine.PlaySound(heldItem.UseSound, Projectile.Center);
                Vector2 gundir = Projectile.rotation.ToRotationVector2();

                DragonsBreathRifleHeldProj.SpawnGunDust(Projectile, Projectile.Center, gundir);
                if (Main.rand.NextBool()) {
                    Vector2 vr = (Projectile.rotation - Main.rand.NextFloat(-0.1f, 0.1f) * DirSign).ToRotationVector2() * -Main.rand.NextFloat(3, 7) + Owner.velocity;
                    Projectile.NewProjectile(Projectile.parent(), Projectile.Center, vr, ModContent.ProjectileType<GunCasing>(), 10, Projectile.knockBack, Owner.whoAmI);
                }

                if (AmmoTypes == ProjectileID.Bullet) {
                    AmmoTypes = ModContent.ProjectileType<TorrentialBullet>();
                }

                for (int i = 0; i < 33; i++) {
                    int proj = Projectile.NewProjectile(Owner.parent(), Projectile.Center + gundir * 3
                        , gundir.RotatedBy(Main.rand.NextFloat(-0.05f, 0.05f)) * Main.rand.NextFloat(0.9f, 1.32f) * 13
                        , AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
                    Main.projectile[proj].timeLeft = 90;
                }
                _ = UpdateConsumeAmmo();
                _ = CreateRecoil();
                Projectile.ai[1] = 0;
                onFire = false;
            }
        }
    }
}
