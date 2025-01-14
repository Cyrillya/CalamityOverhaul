﻿using CalamityMod;
using CalamityMod.Events;
using CalamityMod.NPCs;
using CalamityMod.World;
using CalamityOverhaul.Common;
using CalamityOverhaul.Content.NPCs.Core;
using CalamityOverhaul.Content.Particles;
using CalamityOverhaul.Content.Particles.Core;
using CalamityOverhaul.Content.Projectiles.Boss.SkeletronPrime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.NPCs.BrutalNPCs.BrutalSkeletronPrime
{
    internal class BrutalSkeletronPrimeAI : NPCCoverage, ISetupData
    {
        public override int TargetID => NPCID.SkeletronPrime;
        internal static int BSP_Main_NPC_Index = -1;
        private const int maxfindModes = 6000;
        private int frame = 0;
        private int primeCannon;
        private int primeSaw;
        private int primeVice;
        private int primeLaser;
        private int fireIndex;
        private bool cannonAlive;
        private bool viceAlive;
        private bool sawAlive;
        private bool laserAlive;
        private bool bossRush;
        private bool death;
        private Player player;
        internal static Asset<Texture2D> HandAsset;
        internal static Asset<Texture2D> BSPCannon;
        internal static Asset<Texture2D> BSPlaser;
        internal static Asset<Texture2D> BSPPliers;
        internal static Asset<Texture2D> BSPSAW;
        internal static Asset<Texture2D> BSPRAM;
        internal static Asset<Texture2D> HandAssetGlow;
        internal static Asset<Texture2D> BSPCannonGlow;
        internal static Asset<Texture2D> BSPlaserGlow;
        internal static Asset<Texture2D> BSPPliersGlow;
        internal static Asset<Texture2D> BSPSAWGlow;
        internal static Asset<Texture2D> BSPRAMGlow;

        internal static void DrawArm(SpriteBatch spriteBatch, NPC rCurrentNPC, Vector2 screenPos) {
            Vector2 vector7 = new Vector2(rCurrentNPC.position.X + rCurrentNPC.width * 0.5f - 5f * rCurrentNPC.ai[0], rCurrentNPC.position.Y + 20f);
            for (int k = 0; k < 2; k++) {
                float num21 = Main.npc[(int)rCurrentNPC.ai[1]].position.X + Main.npc[(int)rCurrentNPC.ai[1]].width / 2 - vector7.X;
                float num22 = Main.npc[(int)rCurrentNPC.ai[1]].position.Y + Main.npc[(int)rCurrentNPC.ai[1]].height / 2 - vector7.Y;
                float num23;
                if (k == 0) {
                    num21 -= 200f * rCurrentNPC.ai[0];
                    num22 += 130f;
                    num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                    num23 = 92f / num23;
                    vector7.X += num21 * num23;
                    vector7.Y += num22 * num23;
                }
                else {
                    num21 -= 50f * rCurrentNPC.ai[0];
                    num22 += 80f;
                    num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                    num23 = 60f / num23;
                    vector7.X += num21 * num23;
                    vector7.Y += num22 * num23;
                }
                float rotation7 = (float)Math.Atan2(num22, num21) - 1.57f;
                Color color7 = Lighting.GetColor((int)vector7.X / 16, (int)(vector7.Y / 16f));

                Vector2 drawPos = new Vector2(vector7.X - screenPos.X, vector7.Y - screenPos.Y);
                Vector2 drawOrig = new Vector2(TextureAssets.BoneArm.Width() * 0.5f, TextureAssets.BoneArm.Height() * 0.5f);
                Rectangle drawRec = new Rectangle(0, 0, TextureAssets.BoneArm.Width(), TextureAssets.BoneArm.Height());
                SpriteEffects spriteEffects = k == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
                spriteBatch.Draw(BSPRAM.Value, drawPos, drawRec, color7, rotation7, drawOrig, 1f, spriteEffects, 0f);
                spriteBatch.Draw(BSPRAMGlow.Value, drawPos, drawRec, Color.White, rotation7, drawOrig, 1f, spriteEffects, 0f);

                if (k == 0) {
                    vector7.X += num21 * num23 / 2f;
                    vector7.Y += num22 * num23 / 2f;
                }
                else if (Main.instance.IsActive) {
                    vector7.X += num21 * num23 - 16f;
                    vector7.Y += num22 * num23 - 6f;
                    int num24 = Dust.NewDust(new Vector2(vector7.X, vector7.Y), 30, 10
                        , DustID.FireworkFountain_Red, num21 * 0.02f, num22 * 0.02f, 0, Color.Gold, 0.5f);
                    Main.dust[num24].noGravity = true;
                }
            }
        }

        void ISetupData.LoadData() {
            if (Main.dedServ) {
                return;
            }
            string path = CWRConstant.NPC + "BSP/";
            HandAsset = CWRUtils.GetT2DAsset(path + "BrutalSkeletron");
            BSPCannon = CWRUtils.GetT2DAsset(path + "BSPCannon");
            BSPlaser = CWRUtils.GetT2DAsset(path + "BSPlaser");
            BSPPliers = CWRUtils.GetT2DAsset(path + "BSPPliers");
            BSPSAW = CWRUtils.GetT2DAsset(path + "BSPSAW");
            BSPRAM = CWRUtils.GetT2DAsset(path + "BSPRAM");
            HandAssetGlow = CWRUtils.GetT2DAsset(path + "BrutalSkeletronGlow");
            BSPCannonGlow = CWRUtils.GetT2DAsset(path + "BSPCannonGlow");
            BSPlaserGlow = CWRUtils.GetT2DAsset(path + "BSPlaserGlow");
            BSPPliersGlow = CWRUtils.GetT2DAsset(path + "BSPPliersGlow");
            BSPSAWGlow = CWRUtils.GetT2DAsset(path + "BSPSAWGlow");
            BSPRAMGlow = CWRUtils.GetT2DAsset(path + "BSPRAMGlow");
        }

        void ISetupData.UnLoadData() {
            HandAsset = null;
            BSPCannon = null;
            BSPlaser = null;
            BSPPliers = null;
            BSPSAW = null;
            BSPRAM = null;
            HandAssetGlow = null;
            BSPCannonGlow = null;
            BSPlaserGlow = null;
            BSPPliersGlow = null;
            BSPSAWGlow = null;
            BSPRAMGlow = null;
        }

        public override bool CanLoad() {
            return true;
        }

        internal static void FindPlayer(NPC npc) {
            if (npc.target < 0 || npc.target == Main.maxPlayers || Main.player[npc.target].dead || !Main.player[npc.target].active) {
                npc.TargetClosest();
            }
            if (Vector2.Distance(Main.player[npc.target].Center, npc.Center) > CalamityGlobalNPC.CatchUpDistance200Tiles) {
                npc.TargetClosest();
            }
        }

        internal static int SetMultiplier(int num) {
            if (CalamityConfig.Instance.EarlyHardmodeProgressionRework && !BossRushEvent.BossRushActive) {
                double firstMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkFirstMechStatMultiplier_Expert;
                double secondMechMultiplier = CalamityGlobalNPC.EarlyHardmodeProgressionReworkSecondMechStatMultiplier_Expert;
                if (!NPC.downedMechBossAny)
                    num = (int)(num * firstMechMultiplier);
                else if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2) || (!NPC.downedMechBoss2 && !NPC.downedMechBoss3) || (!NPC.downedMechBoss3 && !NPC.downedMechBoss1))
                    num = (int)(num * secondMechMultiplier);
            }
            return num;
        }

        internal static void CheakDead(NPC npc, NPC head) {
            // 所以，如果头部死亡，那么手臂也立马死亡
            if (!head.active) {
                npc.ai[2] += 10f;
                if (npc.ai[2] > 50f || Main.netMode != NetmodeID.Server) {
                    npc.life = -1;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                }
            }
        }

        internal static void CheakRam(out bool cannonAlive, out bool viceAlive, out bool sawAlive, out bool laserAlive) {
            cannonAlive = viceAlive = sawAlive = laserAlive = false;
            if (CalamityGlobalNPC.primeCannon != -1) {
                if (Main.npc[CalamityGlobalNPC.primeCannon].active)
                    cannonAlive = true;
            }
            if (CalamityGlobalNPC.primeVice != -1) {
                if (Main.npc[CalamityGlobalNPC.primeVice].active)
                    viceAlive = true;
            }
            if (CalamityGlobalNPC.primeSaw != -1) {
                if (Main.npc[CalamityGlobalNPC.primeSaw].active)
                    sawAlive = true;
            }
            if (CalamityGlobalNPC.primeLaser != -1) {
                if (Main.npc[CalamityGlobalNPC.primeLaser].active)
                    sawAlive = true;
            }
        }

        internal static void SpanFireLerterDustEffect(NPC npc, int modes) {
            Vector2 pos = npc.Center + (npc.rotation + MathHelper.PiOver2).ToRotationVector2() * 30;
            for (int i = 0; i < 4; i++) {
                float rot1 = MathHelper.PiOver2 * i;
                Vector2 vr = rot1.ToRotationVector2();
                for (int j = 0; j < modes; j++) {
                    CWRParticle spark = new HeavenfallStarParticle(pos, vr * (0.1f + j * 0.34f), false, 13, Main.rand.NextFloat(1.2f, 1.3f), Color.Red);
                    CWRParticleHandler.AddParticle(spark);
                }
            }
        }

        private void FindePlayer(NPC npc) {
            if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > maxfindModes
                || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > maxfindModes) {
                npc.TargetClosest();
                if (Main.player[npc.target].dead || Math.Abs(npc.position.X - Main.player[npc.target].position.X) > maxfindModes
                    || Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > maxfindModes) {
                    npc.ai[1] = 3f;
                }
            }
        }

        public override bool? AI(NPC npc, Mod mod) {
            bossRush = BossRushEvent.BossRushActive;
            death = CalamityWorld.death || bossRush;
            player = Main.player[npc.target];
            npc.defense = npc.defDefense;
            if (npc.ai[3] != 0f) {
                NPC.mechQueen = npc.whoAmI;
            }

            npc.reflectsProjectiles = false;
            if (npc.ai[0] == 0f) {
                if (Main.netMode != NetmodeID.MultiplayerClient) {
                    npc.TargetClosest();
                    spanArm(npc);
                    fireIndex = 0;
                }
                if (!Main.dedServ) {
                    CWRUtils.ActivateSky("CWRMod:BrutalSkeletronSky");
                }
                npc.ai[0] = 1f;
            }

            FindePlayer(npc);
            CheakRam(out cannonAlive, out viceAlive, out sawAlive, out laserAlive);

            if (Main.IsItDay() && npc.ai[1] != 3f && npc.ai[1] != 2f) {
                npc.ai[1] = 2f;
                SoundEngine.PlaySound(SoundID.ForceRoar, npc.Center);
            }

            if (npc.ai[1] == 0f) {
                npc.damage = 0;

                npc.ai[2] += 1f;
                float aiThreshold = Main.masterMode ? 300f : 600f;
                if (npc.ai[2] >= aiThreshold) {
                    npc.ai[2] = 0f;
                    npc.ai[1] = 1f;
                    npc.Calamity().newAI[0]++;
                    if (!CWRUtils.isClient && npc.Calamity().newAI[0] >= 2) {
                        Projectile.NewProjectile(npc.GetSource_FromAI(), player.Center, new Vector2(0, 0)
                            , ModContent.ProjectileType<SetPosingStarm>(), npc.damage, 2, -1, 0, npc.whoAmI);
                        npc.Calamity().newAI[0] = 0;
                        npc.SyncExtraAI();
                        fireIndex++;
                    }
                    npc.TargetClosest();
                    npc.netUpdate = true;
                }

                if (NPC.IsMechQueenUp) {
                    npc.rotation = npc.rotation.AngleLerp(npc.velocity.X / 15f * 0.5f, 0.75f);
                }
                else {
                    npc.rotation = npc.velocity.X / 15f;
                }

                float verticalAcceleration = 0.1f;
                float maxVerticalSpeed = 2f;
                float horizontalAcceleration = 0.1f;
                float maxHorizontalSpeed = 8f;
                float deceleration = Main.masterMode ? 0.94f : Main.expertMode ? 0.96f : 0.98f;
                int verticalOffset = 200;
                int verticalThreshold = 500;
                float horizontalOffset = 0f;
                int directionMultiplier = (Main.player[npc.target].Center.X < npc.Center.X) ? -1 : 1;

                if (NPC.IsMechQueenUp) {
                    horizontalOffset = -450f * directionMultiplier;
                    verticalOffset = 300;
                    verticalThreshold = 350;
                }

                if (Main.expertMode) {
                    verticalAcceleration = Main.masterMode ? 0.05f : 0.03f;
                    maxVerticalSpeed = Main.masterMode ? 5f : 4f;
                    horizontalAcceleration = Main.masterMode ? 0.15f : 0.12f;
                    maxHorizontalSpeed = Main.masterMode ? 11f : 9.5f;
                    if (death) {
                        verticalAcceleration += 0.01f;
                        maxVerticalSpeed += 1f;
                        horizontalAcceleration += 0.2f;
                        maxHorizontalSpeed += 3f;
                    }
                }

                AdjustVerticalMovement(npc, verticalAcceleration, maxVerticalSpeed, deceleration, verticalOffset, verticalThreshold);
                AdjustHorizontalMovement(npc, horizontalAcceleration, maxHorizontalSpeed, deceleration, horizontalOffset);
            }
            else if (npc.ai[1] == 1f) {
                npc.defense *= 2;
                npc.damage = npc.defDamage * 2;
                npc.Calamity().CurrentlyIncreasingDefenseOrDR = true;

                npc.ai[2]++;
                if (npc.ai[2] == 2f) {
                    SoundEngine.PlaySound(SoundID.ForceRoar, npc.Center);
                }

                float aiThreshold = Main.masterMode ? 300f : 400f;
                if (npc.ai[2] >= aiThreshold) {
                    npc.ai[2] = 0f;
                    npc.ai[1] = 0f;
                }

                UpdateRotation(npc);

                Vector2 targetVector = Main.player[npc.target].Center - npc.Center;
                float distanceToTarget = targetVector.Length();
                float initialSpeed = 5f;
                float speedMultiplier = CalculateSpeedMultiplier(distanceToTarget, initialSpeed);
                if (NPC.IsMechQueenUp) {
                    float mechQueenSpeedFactor = NPC.npcsFoundForCheckActive[NPCID.TheDestroyerBody] ? 0.6f : 0.75f;
                    speedMultiplier *= mechQueenSpeedFactor;
                }

                UpdateVelocity(npc, targetVector, speedMultiplier, distanceToTarget);
            }
            else if (npc.ai[1] == 2f) {
                EnrageNPC(npc);
                UpdateRotation(npc);
                MoveTowardsPlayer(npc, 10f, 8f, 32f, 100f);
            }
            else {
                if (npc.ai[1] != 3f)
                    return false;
                HandleDespawn(npc);
            }

            return false;
        }

        #region SetFromeAIFunc
        private void AdjustVerticalMovement(NPC npc, float acceleration, float maxSpeed, float deceleration, int offset, int threshold) {
            if (npc.position.Y > Main.player[npc.target].position.Y - offset) {
                if (npc.velocity.Y > 0f) {
                    npc.velocity.Y *= deceleration;
                }
                npc.velocity.Y -= acceleration;
                if (npc.velocity.Y > maxSpeed) {
                    npc.velocity.Y = maxSpeed;
                }
            }
            else if (npc.position.Y < Main.player[npc.target].position.Y - threshold) {
                if (npc.velocity.Y < 0f) {
                    npc.velocity.Y *= deceleration;
                }
                npc.velocity.Y += acceleration;
                if (npc.velocity.Y < -maxSpeed) {
                    npc.velocity.Y = -maxSpeed;
                }
            }
        }

        private void AdjustHorizontalMovement(NPC npc, float acceleration, float maxSpeed, float deceleration, float offset) {
            if (npc.Center.X > Main.player[npc.target].Center.X + 100f + offset) {
                if (npc.velocity.X > 0f) {
                    npc.velocity.X *= deceleration;
                }
                npc.velocity.X -= acceleration;
                if (npc.velocity.X > maxSpeed) {
                    npc.velocity.X = maxSpeed;
                }
            }

            if (npc.Center.X < Main.player[npc.target].Center.X - 100f + offset) {
                if (npc.velocity.X < 0f) {
                    npc.velocity.X *= deceleration;
                }
                npc.velocity.X += acceleration;
                if (npc.velocity.X < -maxSpeed) {
                    npc.velocity.X = -maxSpeed;
                }
            }
        }

        private void UpdateRotation(NPC npc) {
            if (NPC.IsMechQueenUp)
                npc.rotation = npc.rotation.AngleLerp(npc.velocity.X / 15f * 0.5f, 0.75f);
            else
                npc.rotation += npc.direction * 0.3f;
        }

        private float CalculateSpeedMultiplier(float distance, float initialSpeed) {
            if (Main.expertMode) {
                float speed = Main.masterMode ? 7f : 6f;
                float speedFactor = Main.masterMode ? 1.15f : 1.1f;
                if (distance > 150f) speed *= Main.masterMode ? 1.075f : 1.05f;
                for (int threshold = 200; threshold <= 600; threshold += 50) {
                    if (distance > threshold) speed *= speedFactor;
                }
                return speed;
            }
            return initialSpeed;
        }

        private void UpdateVelocity(NPC npc, Vector2 targetVector, float speedMultiplier, float distance) {
            float adjustedSpeed = speedMultiplier / distance;
            if (death && fireIndex >= 2) {
                if (--npc.Calamity().newAI[2] <= 0) {
                    npc.velocity.X = targetVector.X * adjustedSpeed;
                    npc.velocity.Y = targetVector.Y * adjustedSpeed;
                }
                else {
                    npc.velocity *= 0.99f;
                }

                if (++npc.Calamity().newAI[1] > 90) {
                    Vector2 toD = npc.Center.To(player.Center) + player.velocity;
                    toD = toD.UnitVector();
                    npc.velocity += toD * 23;
                    npc.Calamity().newAI[2] = 60;
                    npc.Calamity().newAI[1] = 0;
                    npc.SyncExtraAI();
                    if (Main.npc[primeCannon].active)
                        Main.npc[primeCannon].velocity += toD * 33;
                    if (Main.npc[primeSaw].active)
                        Main.npc[primeSaw].velocity += toD * 53;
                    if (Main.npc[primeLaser].active)
                        Main.npc[primeLaser].velocity += toD * 33;
                    if (Main.npc[primeVice].active)
                        Main.npc[primeVice].velocity += toD * 53;
                }
            }
            else {
                npc.velocity.X = targetVector.X * adjustedSpeed;
                npc.velocity.Y = targetVector.Y * adjustedSpeed;
            }
            if (NPC.IsMechQueenUp) {
                float distanceToPlayer = Vector2.Distance(npc.Center, Main.player[npc.target].Center);
                if (distanceToPlayer < 0.1f) distanceToPlayer = 0f;
                if (distanceToPlayer < speedMultiplier)
                    npc.velocity = npc.velocity.SafeNormalize(Vector2.Zero) * distanceToPlayer;
            }
        }

        private void EnrageNPC(NPC npc) {
            // 增加 NPC 的伤害和防御
            npc.damage = 1000;
            npc.defense = 9999;
            // 标记当前正在愤怒状态和增加防御力或伤害减免
            npc.Calamity().CurrentlyEnraged = true;
            npc.Calamity().CurrentlyIncreasingDefenseOrDR = true;
        }

        private void MoveTowardsPlayer(NPC npc, float baseSpeed, float minSpeed, float maxSpeed, float speedDivisor) {
            // 计算玩家与 NPC 之间的向量和距离
            Vector2 npcCenter = npc.Center;
            Vector2 playerCenter = Main.player[npc.target].Center;
            Vector2 directionToPlayer = playerCenter - npcCenter;
            float distanceToPlayer = directionToPlayer.Length();
            // 计算速度
            float adjustedSpeed = baseSpeed + distanceToPlayer / speedDivisor;
            adjustedSpeed = Math.Clamp(adjustedSpeed, minSpeed, maxSpeed);
            // 根据计算出的向量调整速度
            directionToPlayer.Normalize();
            npc.velocity = directionToPlayer * adjustedSpeed;
        }

        private void HandleDespawn(NPC npc) {
            if (NPC.IsMechQueenUp) {
                DespawnNPC(NPCID.Retinazer);
                DespawnNPC(NPCID.Spazmatism);
                // 如果 Retinazer 和 Spazmatism 都不在，则变形并消失
                if (!NPC.AnyNPCs(NPCID.Retinazer) && !NPC.AnyNPCs(NPCID.Spazmatism)) {
                    TransformOrDespawnNPC(NPCID.TheDestroyer, NPCID.TheDestroyerTail, npc);
                }
                AdjustVelocity(npc, 0.1f, 0.95f, 13f);
            }
            else {
                npc.EncourageDespawn(500);
                AdjustVelocity(npc, 0.1f, 0.95f, float.MaxValue);
            }
        }

        private void DespawnNPC(int npcID) {
            int npcIndex = NPC.FindFirstNPC(npcID);
            if (npcIndex >= 0) {
                Main.npc[npcIndex].EncourageDespawn(5);
            }
        }

        private void TransformOrDespawnNPC(int findNpcID, int transformNpcID, NPC npc) {
            int npcIndex = NPC.FindFirstNPC(findNpcID);
            if (npcIndex >= 0) {
                Main.npc[npcIndex].Transform(transformNpcID);
            }
            npc.EncourageDespawn(5);
        }

        private void AdjustVelocity(NPC npc, float verticalAcceleration, float horizontalDeceleration, float maxVerticalSpeed) {
            npc.velocity.Y += verticalAcceleration;
            if (npc.velocity.Y < 0f) {
                npc.velocity.Y *= horizontalDeceleration;
            }

            npc.velocity.X *= horizontalDeceleration;
            if (npc.velocity.Y > maxVerticalSpeed) {
                npc.velocity.Y = maxVerticalSpeed;
            }
        }

        private void spanArm(NPC npc, int limit = 0) {
            if (limit == 1 || limit == 0) {
                primeCannon = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeCannon, npc.whoAmI);
                Main.npc[primeCannon].ai[0] = -1f;
                Main.npc[primeCannon].ai[1] = npc.whoAmI;
                Main.npc[primeCannon].target = npc.target;
                Main.npc[primeCannon].netUpdate = true;
            }
            if (limit == 2 || limit == 0) {
                primeSaw = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeSaw, npc.whoAmI);
                Main.npc[primeSaw].ai[0] = 1f;
                Main.npc[primeSaw].ai[1] = npc.whoAmI;
                Main.npc[primeSaw].target = npc.target;
                Main.npc[primeSaw].netUpdate = true;
            }
            if (limit == 3 || limit == 0) {
                primeVice = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeVice, npc.whoAmI);
                Main.npc[primeVice].ai[0] = -1f;
                Main.npc[primeVice].ai[1] = npc.whoAmI;
                Main.npc[primeVice].target = npc.target;
                Main.npc[primeVice].ai[3] = 150f;
                Main.npc[primeVice].netUpdate = true;
            }
            if (limit == 4 || limit == 0) {
                primeLaser = NPC.NewNPC(npc.GetSource_FromAI(), (int)npc.Center.X, (int)npc.Center.Y, NPCID.PrimeLaser, npc.whoAmI);
                Main.npc[primeLaser].ai[0] = 1f;
                Main.npc[primeLaser].ai[1] = npc.whoAmI;
                Main.npc[primeLaser].target = npc.target;
                Main.npc[primeLaser].ai[3] = 150f;
                Main.npc[primeLaser].netUpdate = true;
            }
        }
        #endregion

        public override bool? Draw(Mod mod, NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => false;

        public override bool PostDraw(Mod mod, NPC NPC, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            Texture2D mainValue = HandAsset.Value;
            Texture2D mainValue2 = HandAssetGlow.Value;
            Main.EntitySpriteDraw(mainValue, NPC.Center - Main.screenPosition, CWRUtils.GetRec(mainValue, frame, 6)
                , drawColor, NPC.rotation, CWRUtils.GetOrig(mainValue, 6), NPC.scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(mainValue2, NPC.Center - Main.screenPosition, CWRUtils.GetRec(mainValue, frame, 6)
                , Color.White, NPC.rotation, CWRUtils.GetOrig(mainValue, 6), NPC.scale, SpriteEffects.None, 0);
            if (!cannonAlive && !laserAlive && !sawAlive && !viceAlive) {
                Vector2 toD = player.Center.To(NPC.Center);
                Vector2 origpos = player.Center - Main.screenPosition;
                float alp = toD.Length() / 400f;
                if (alp > 1) {
                    alp = 1;
                }
                Vector2 drawPos1 = new Vector2(-toD.X, toD.Y) + origpos;
                Main.EntitySpriteDraw(mainValue, drawPos1, CWRUtils.GetRec(mainValue, frame, 6)
                , drawColor * alp, NPC.rotation, CWRUtils.GetOrig(mainValue, 6), NPC.scale, SpriteEffects.None, 0);
                Vector2 drawPos2 = new Vector2(-toD.X, -toD.Y) + origpos;
                Main.EntitySpriteDraw(mainValue, drawPos2, CWRUtils.GetRec(mainValue, frame, 6)
                , drawColor * alp, NPC.rotation, CWRUtils.GetOrig(mainValue, 6), NPC.scale, SpriteEffects.None, 0);
                Vector2 drawPos3 = new Vector2(toD.X, -toD.Y) + origpos;
                Main.EntitySpriteDraw(mainValue, drawPos3, CWRUtils.GetRec(mainValue, frame, 6)
                , drawColor * alp, NPC.rotation, CWRUtils.GetOrig(mainValue, 6), NPC.scale, SpriteEffects.None, 0);
            }
            return false;
        }
    }
}
