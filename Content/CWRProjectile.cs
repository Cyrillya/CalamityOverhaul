﻿using CalamityMod;
using CalamityMod.Dusts;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Melee;
using CalamityOverhaul.Content.Projectiles.Weapons.Ranged;
using CalamityOverhaul.Content.Projectiles.Weapons.Summon;
using CalamityOverhaul.Content.RemakeItems.Vanilla;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;
using CosmicFire = CalamityOverhaul.Content.Projectiles.Weapons.Summon.CosmicFire;

namespace CalamityOverhaul.Content
{
    /// <summary>
    /// 用于分组弹幕的发射源，这决定了一些弹幕的特殊行为
    /// </summary>
    public enum SpanTypesEnum : byte
    {
        DeadWing = 1,
        ClaretCannon,
        Phantom,
        Alluvion,
        Marksman,
        NettlevineGreat
    }

    public class CWRProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public byte SpanTypes;

        public override void SetDefaults(Projectile projectile) {
            if (projectile.type == ProjectileID.Meowmere) {
                projectile.timeLeft = 160;
                projectile.penetrate = 2;
            }
        }

        public override void OnSpawn(Projectile projectile, IEntitySource source) {
            base.OnSpawn(projectile, source);
        }

        public override void AI(Projectile projectile) {
        }

        public override bool PreAI(Projectile projectile) {
            return base.PreAI(projectile);
        }

        public override void PostAI(Projectile projectile) {
            if (projectile.type == ProjectileID.Meowmere) {
                projectile.velocity.X *= 0.98f;
                projectile.velocity.Y += 0.01f;
            }
            if (SpanTypes == (byte)SpanTypesEnum.NettlevineGreat) {
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height
                        , (int)CalamityDusts.SulfurousSeaAcid, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
            }
        }

        public override void OnKill(Projectile projectile, int timeLeft) {
            RMeowmere.SpanDust(projectile, 0.2f);
            if (projectile.IsOwnedByLocalPlayer()) {
                if (SpanTypes == (byte)SpanTypesEnum.Marksman) {
                    int proj = Projectile.NewProjectile(projectile.parent(), projectile.Center, projectile.velocity
                        , ProjectileID.LostSoulFriendly, projectile.damage / 2, projectile.knockBack / 2, projectile.owner, 0);
                    Main.projectile[proj].DamageType = DamageClass.Ranged;
                    Main.projectile[proj].timeLeft = 60;
                    NetMessage.SendData(MessageID.SyncProjectile, -1, projectile.owner, null, proj);
                }
            }
        }

        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info) {
            RMeowmere.SpanDust(projectile);
            if (SpanTypes == (byte)SpanTypesEnum.NettlevineGreat) {
                target.AddBuff(70, 60);
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
            RMeowmere.SpanDust(projectile);

            Player player = Main.player[projectile.owner];
            if (SpanTypes == (byte)SpanTypesEnum.DeadWing) {
                int types = ModContent.ProjectileType<DeadWave>();

                if (player.Center.To(target.Center).LengthSquared() < 600 * 600
                    && projectile.type != types
                    && projectile.numHits == 0) {
                    Vector2 vr = player.Center.To(Main.MouseWorld)
                        .RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-15, 15))).UnitVector() * Main.rand.Next(7, 9);
                    Vector2 pos = player.Center + vr * 10;
                    Projectile.NewProjectileDirect(
                        CWRUtils.parent(player), pos, vr,
                        ModContent.ProjectileType<DeadWave>(),
                        projectile.damage,
                        projectile.knockBack,
                        projectile.owner
                        ).rotation = vr.ToRotation();
                }
            }

            if (SpanTypes == (byte)SpanTypesEnum.ClaretCannon) {
                Projectile projectile1 = Projectile.NewProjectileDirect(
                        CWRUtils.parent(player),
                        target.position,
                        Vector2.Zero,
                        ModContent.ProjectileType<BloodVerdict>(),
                        projectile.damage,
                        projectile.knockBack,
                        projectile.owner
                        );
                BloodVerdict bloodVerdict = projectile1.ModProjectile as BloodVerdict;
                if (bloodVerdict != null) {
                    bloodVerdict.offsetVr = new Vector2(Main.rand.Next(target.width), Main.rand.Next(target.height));
                    bloodVerdict.Projectile.ai[1] = target.whoAmI;
                    Vector2[] vrs = new Vector2[3];
                    for (int i = 0; i < 3; i++)
                        vrs[i] = Main.rand.NextVector2Unit() * Main.rand.Next(16, 19);
                    bloodVerdict.effusionDirection = vrs;
                }
            }

            if (SpanTypes == (byte)SpanTypesEnum.Phantom && projectile.numHits == 0) {
                Projectile.NewProjectile(projectile.parent(), player.Center + projectile.velocity.GetNormalVector() * Main.rand.Next(-130, 130)
                    , projectile.velocity, ModContent.ProjectileType<PhantasmArrow>()
                    , projectile.damage, projectile.knockBack / 2, player.whoAmI, 0, target.whoAmI);
            }

            if (SpanTypes == (byte)SpanTypesEnum.Alluvion && projectile.numHits == 0) {
                Projectile.NewProjectile(projectile.parent(), player.Center
                    , projectile.velocity, ModContent.ProjectileType<DeepSeaSharks>()
                    , projectile.damage, projectile.knockBack / 2, player.whoAmI, 0, target.whoAmI);
            }

            if (projectile.DamageType == DamageClass.Summon && target.CWR().WhipHitNum > 0) {
                CWRNpc npc = target.CWR();
                WhipHitTypeEnum wTypes = (WhipHitTypeEnum)npc.WhipHitType;
                switch (wTypes) {
                    case WhipHitTypeEnum.WhiplashGalactica:
                        if ((
                            (projectile.numHits % 3 == 0 && projectile.minion == true)
                            || (projectile.numHits == 0 && projectile.minion == false)
                            )
                            && projectile.type != ModContent.ProjectileType<CosmicFire>()
                            ) {
                            float randRot = Main.rand.NextFloat(MathHelper.TwoPi);

                            for (int i = 0; i < 3; i++) {
                                Vector2 vr = (MathHelper.TwoPi / 3 * i + randRot).ToRotationVector2() * 10;
                                int proj = Projectile.NewProjectile(CWRUtils.parent(projectile), target.Center, vr
                                    , ModContent.ProjectileType<GodKillers>(), projectile.damage / 2, 0, projectile.owner);
                                Main.projectile[proj].timeLeft = 65;
                                Main.projectile[proj].penetrate = -1;
                            }
                        }
                        break;
                    case WhipHitTypeEnum.BleedingScourge:
                        Projectile.NewProjectile(
                                    CWRUtils.parent(projectile),
                                    target.Center,
                                    Vector2.Zero,
                                    ModContent.ProjectileType<Content.Projectiles.Weapons.Summon.BloodBlast>(),
                                    projectile.damage / 2, 0, projectile.owner);
                        break;
                    case WhipHitTypeEnum.AzureDragonRage:
                        break;
                    case WhipHitTypeEnum.GhostFireWhip:
                        break;
                    case WhipHitTypeEnum.AllhallowsGoldWhip:
                        break;
                    case WhipHitTypeEnum.ElementWhip:
                        break;
                }

                if (npc.WhipHitNum > 0)
                    npc.WhipHitNum--;
            }

            if (SpanTypes == (byte)SpanTypesEnum.NettlevineGreat) {
                target.AddBuff(70, 60);
            }

            if (projectile.type == ModContent.ProjectileType<ExoVortex>()) {
                ExoVortexOnHitDeBug(target);
            }
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor) {
            if (projectile.type == ModContent.ProjectileType<ThanatosLaser>()) {
                ThanatosLaserDrawDeBug(projectile, ref lightColor);
            }
            return base.PreDraw(projectile, ref lightColor);
        }

        private void ExoVortexOnHitDeBug(NPC npc) {
            if (npc.type == ModContent.NPCType<BrimstoneHeart>()) {
                return;
            }
        }

        private bool ThanatosLaserDrawDeBug(Projectile projectile, ref Color lightColor) {
            ThanatosLaser thanatosLaser = projectile.ModProjectile as ThanatosLaser;
            if (thanatosLaser.TelegraphDelay >= ThanatosLaser.TelegraphTotalTime) {
                lightColor.R = (byte)(255 * projectile.Opacity);
                lightColor.G = (byte)(255 * projectile.Opacity);
                lightColor.B = (byte)(255 * projectile.Opacity);
                Vector2 drawOffset = projectile.velocity.SafeNormalize(Vector2.Zero) * -30f;
                projectile.Center += drawOffset;
                if (projectile.type.ValidateIndex(Main.maxProjectiles))
                    CalamityUtils.DrawAfterimagesCentered(projectile, ProjectileID.Sets.TrailingMode[projectile.type], lightColor, 1);
                projectile.Center -= drawOffset;
                return false;
            }

            Texture2D laserTelegraph = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/LaserWallTelegraphBeam").Value;

            float yScale = 2f;
            if (thanatosLaser.TelegraphDelay < ThanatosLaser.TelegraphFadeTime)
                yScale = MathHelper.Lerp(0f, 2f, thanatosLaser.TelegraphDelay / 15f);
            if (thanatosLaser.TelegraphDelay > ThanatosLaser.TelegraphTotalTime - ThanatosLaser.TelegraphFadeTime)
                yScale = MathHelper.Lerp(2f, 0f, (thanatosLaser.TelegraphDelay - (ThanatosLaser.TelegraphTotalTime - ThanatosLaser.TelegraphFadeTime)) / 15f);

            Vector2 scaleInner = new Vector2(ThanatosLaser.TelegraphWidth / laserTelegraph.Width, yScale);
            Vector2 origin = laserTelegraph.Size() * new Vector2(0f, 0.5f);
            Vector2 scaleOuter = scaleInner * new Vector2(1f, 2.2f);

            Color colorOuter = Color.Lerp(Color.Red, Color.Crimson, thanatosLaser.TelegraphDelay / ThanatosLaser.TelegraphTotalTime * 2f % 1f);
            Color colorInner = Color.Lerp(colorOuter, Color.White, 0.75f);

            colorOuter *= 0.6f;
            colorInner *= 0.6f;

            Main.EntitySpriteDraw(laserTelegraph, projectile.Center - Main.screenPosition, null, colorInner, thanatosLaser.Velocity.ToRotation(), origin, scaleInner, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(laserTelegraph, projectile.Center - Main.screenPosition, null, colorOuter, thanatosLaser.Velocity.ToRotation(), origin, scaleOuter, SpriteEffects.None, 0);
            return false;
        }
    }
}
