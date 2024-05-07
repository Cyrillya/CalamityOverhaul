﻿using CalamityMod;
using CalamityOverhaul.Common;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;
using ReLogic.Content;
using System.Runtime.InteropServices;
using Terraria.Audio;
using Terraria.ID;
using CalamityOverhaul.Content.Particles.Core;
using CalamityOverhaul.Content.Particles;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Melee.Neutrons
{
    internal class NeutronGlaiveHeld : BaseHeldProj, ISetupData
    {
        public override string Texture => CWRConstant.Item_Melee + "NeutronGlaive";
        static Asset<Texture2D> bar1;
        static Asset<Texture2D> bar2;
        static Asset<Texture2D> bar3;
        static Asset<Texture2D> bar4;
        bool canatcck;
        bool canatcck2 = true;
        bool canatcck3 = true;
        int uiframe;
        const int maxatcck = 80;
        public void SetupData() {
            bar1 = CWRUtils.GetT2DAsset(CWRConstant.UI + "NeutronsBar");
            bar2 = CWRUtils.GetT2DAsset(CWRConstant.UI + "NeutronsBar2");
            bar3 = CWRUtils.GetT2DAsset(CWRConstant.UI + "NeutronsBarTop");
            bar4 = CWRUtils.GetT2DAsset(CWRConstant.UI + "NeutronsBarTop2");
        }

        public override void SetDefaults() {
            Projectile.width = Projectile.height = 112;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.hide = true;
        }

        public override void AI() {
            if (Owner.dead || !Owner.active || !Owner.Calamity().mouseRight || canatcck) {
                canatcck = true;
                if (Projectile.ai[0] >= maxatcck) {
                    Projectile.Kill();
                }
                else {
                    canatcck2 = false;
                    Projectile.scale = 1.25f;

                    if (++Projectile.ai[1] > 5) {
                        SoundEngine.PlaySound(SoundID.Item4, Projectile.Center);
                        Vector2 pos = Projectile.Center + Projectile.velocity.UnitVector() * Main.rand.Next(-52, 112);
                        int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos
                        , Projectile.velocity.RotatedByRandom(0.2f), ModContent.ProjectileType<NeutronsOrb>(), Projectile.damage, 0);
                        Main.projectile[proj].Calamity().allProjectilesHome = true;
                        for (int i = 0; i < 4; i++) {
                            float rot1 = MathHelper.PiOver2 * i;
                            Vector2 vr = rot1.ToRotationVector2();
                            for (int j = 0; j < 13; j++) {
                                CWRParticle spark = new HeavenfallStarParticle(pos, vr * (0.1f + j * 0.14f)
                                    , false, 17, Main.rand.NextFloat(0.5f, 0.7f), Color.BlueViolet);
                                CWRParticleHandler.AddParticle(spark);
                            }
                        }
                        Projectile.ai[1] = 0;
                    }

                    Projectile.ai[0]--;
                    if (Projectile.ai[0] <= 0) {
                        Projectile.Kill();
                    }
                }
            }
            if (canatcck2) {
                Projectile.velocity = ToMouse.UnitVector() * 18;
            }
            Projectile.Center = Owner.GetPlayerStabilityCenter() + Projectile.velocity.UnitVector() * 53 * Projectile.scale;
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (!canatcck && Projectile.ai[0] <= maxatcck) {
                Projectile.ai[0]++;
            }
            if (Projectile.ai[0] >= maxatcck) {
                if (canatcck3) {
                    SoundEngine.PlaySound(SoundID.Item4 with { Pitch = -0.2f }, Projectile.Center);
                    canatcck3 = false;
                }
                Projectile.scale = 1.5f;
            }
            SetHeld();
            CWRUtils.ClockFrame(ref Projectile.frame, 5, 5);
            if (canatcck2) {
                CWRUtils.ClockFrame(ref uiframe, 5, 6);
            }
            float rot = (MathHelper.PiOver2 * SafeGravDir - Projectile.rotation) * DirSign * SafeGravDir;
            Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, rot * -DirSign);
            Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, rot * -DirSign);
            Owner.direction = Math.Sign(Projectile.velocity.X);
        }

        public override void OnKill(int timeLeft) {
            if (Projectile.IsOwnedByLocalPlayer() && canatcck2) {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + Projectile.velocity.UnitVector() * 255
                    , Vector2.Zero, ModContent.ProjectileType<EXNeutronExplode>(), Projectile.damage, 0);
            }
        }

        public static void DrawBar(Player Owner, float sengs, int uiframe) {
            if (!(sengs <= 0f)) {
                Texture2D barBG = bar3.Value;
                Texture2D barFG = bar1.Value;
                if (sengs >= maxatcck) {
                    barBG = bar4.Value;
                    barFG = bar2.Value;
                }
                float barScale = 1.2f;
                Vector2 drawPos = Owner.GetPlayerStabilityCenter() + new Vector2(0, 75) - Main.screenPosition;
                Rectangle frameCrop = new Rectangle(0, 0, (int)(sengs / maxatcck * barFG.Width), barFG.Height);
                Color color = Color.White;
                Main.spriteBatch.Draw(barBG, drawPos, CWRUtils.GetRec(barBG, uiframe, 7), color, 0f, CWRUtils.GetOrig(barBG, 7), barScale, 0, 0f);
                Main.spriteBatch.Draw(barFG, drawPos, frameCrop, color, 0f, CWRUtils.GetOrig(barFG, 1), barScale, 0, 0f);
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            DrawBar(Owner, Projectile.ai[0], uiframe);
            Texture2D value = TextureAssets.Projectile[Type].Value;
            Main.EntitySpriteDraw(value, Projectile.Center - Main.screenPosition, CWRUtils.GetRec(value, Projectile.frame, 6)
                , Color.White, Projectile.rotation + MathHelper.PiOver4 * Owner.direction, CWRUtils.GetOrig(value, 6) + new Vector2(0, 5 * Owner.direction)
                , Projectile.scale, Owner.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0);
            return false;
        }
    }
}
