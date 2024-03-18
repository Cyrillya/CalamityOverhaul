﻿using CalamityOverhaul.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria;
using CalamityOverhaul.Content.Items.Ranged;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs
{
    internal class MarksmanBowHeldProj : BaseHeldRanged
    {
        public override string Texture => CWRConstant.Cay_Wap_Ranged + "MarksmanBow";
        public override int targetCayItem => ModContent.ItemType<CalamityMod.Items.Weapons.Ranged.MarksmanBow>();
        public override int targetCWRItem => ModContent.ItemType<MarksmanBowEcType>();
        public override void InOwner() {
            float armRotSengsFront = 60 * CWRUtils.atoR;
            float armRotSengsBack = 110 * CWRUtils.atoR;

            Projectile.Center = Owner.Center + new Vector2(DirSign * 12, 0);
            Projectile.rotation = DirSign > 0 ? MathHelper.ToRadians(20) : MathHelper.ToRadians(160);
            Projectile.timeLeft = 2;
            SetHeld();

            if (!Owner.mouseInterface) {
                if (Owner.PressKey()) {
                    Owner.direction = ToMouse.X > 0 ? 1 : -1;
                    Projectile.rotation = ToMouseA;
                    Projectile.Center = Owner.Center + Projectile.rotation.ToRotationVector2() * 12;
                    armRotSengsBack = armRotSengsFront = (MathHelper.PiOver2 - (ToMouseA + 0.5f * DirSign)) * DirSign;
                    if (HaveAmmo) {
                        onFire = true;
                        Projectile.ai[1]++;
                        armRotSengsFront += MathF.Sin(Time * 0.4f) * 0.7f;
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
            if (onFire && Projectile.ai[1] > Item.useTime) {
                SoundEngine.PlaySound(Item.UseSound, Projectile.Center);
                for (int i = 0; i < 3; i++) {
                    int proj = Projectile.NewProjectile(Owner.parent(), Projectile.Center, ShootVelocity * (0.7f + i * 0.1f), AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
                    Main.projectile[proj].MaxUpdates = 6;
                    Main.projectile[proj].CWR().SpanTypes = (byte)SpanTypesEnum.Marksman;
                    NetMessage.SendData(MessageID.SyncProjectile, -1, Owner.whoAmI, null, proj);
                }
                Projectile.ai[1] = 0;
                onFire = false;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D value = CWRUtils.GetT2DValue(Texture);
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, null, onFire ? Color.White : lightColor
                , Projectile.rotation, value.Size() / 2, Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}
