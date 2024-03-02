﻿using CalamityMod;
using CalamityMod.Projectiles.Ranged;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged
{
    /// <summary>
    /// 一个改进版的枪基类，这个基类的基础实现会更加快捷和易于模板化
    /// </summary>
    internal abstract class BaseGun : BaseHeldRanged
    {
        /// <summary>
        /// 枪械旋转角矫正
        /// </summary>
        public float OffsetRot;
        /// <summary>
        /// 枪械位置矫正
        /// </summary>
        public Vector2 OffsetPos;
        /// <summary>
        /// 右手角度值
        /// </summary>
        public float ArmRotSengsFront;
        /// <summary>
        /// 右手角度值
        /// </summary>
        public float ArmRotSengsBack;
        /// <summary>
        /// 右手角度值矫正
        /// </summary>
        public float ArmRotSengsFrontNoFireOffset;
        /// <summary>
        /// 左手角度值矫正
        /// </summary>
        public float ArmRotSengsBackNoFireOffset;
        /// <summary>
        /// 是否可以右键，默认为<see langword="false"/>
        /// </summary>
        public bool CanRightClick;
        /// <summary>
        /// 是否正在右键开火
        /// </summary>
        protected bool onFireR;
        /// <summary>
        /// 是否在<see cref="InOwner"/>执行后自动更新手臂参数，默认为<see langword="true"/>
        /// </summary>
        public bool SetArmRotBool = true;
        /// <summary>
        /// 枪械是否受到应力缩放，默认为<see langword="true"/>
        /// </summary>
        public bool PressureWhetherIncrease = true;
        /// <summary>
        /// 开火时是否默认播放手持物品的使用音效<see cref="Item.UseSound"/>，但如果准备重写<see cref="SpanProj"/>，这个属性将失去作用，默认为<see langword="true"/>
        /// </summary>
        public bool FiringDefaultSound = true;
        /// <summary>
        /// 这个角度用于设置枪体在玩家非开火阶段的仰角，这个角度是周角而非弧度角，默认为20f
        /// </summary>
        public float AngleFirearmRest = 20f;
        /// <summary>
        /// 枪压，决定开火时的上抬力度，默认为0
        /// </summary>
        public float GunPressure = 0;
        /// <summary>
        /// 控制力度，决定压枪的力度，默认为0.01f
        /// </summary>
        public float ControlForce = 0.01f;
        /// <summary>
        /// 手持距离，生效于非开火状态下，默认为15
        /// </summary>
        public float HandDistance = 15;
        /// <summary>
        /// 手持距离，生效于非开火状态下，默认为0
        /// </summary>
        public float HandDistanceY = 0;
        /// <summary>
        /// 手持距离，生效于开火状态下，默认为20
        /// </summary>
        public float HandFireDistance = 20;
        /// <summary>
        /// 手持距离，生效于开火状态下，默认为-3
        /// </summary>
        public float HandFireDistanceY = -3;
        /// <summary>
        /// 应力范围，默认为5
        /// </summary>
        public float RangeOfStress = 8;
        /// <summary>
        /// 应力缩放系数
        /// </summary>
        public float OwnerPressureIncrease => PressureWhetherIncrease ? Owner.CWR().PressureIncrease : 1;
        /// <summary>
        /// 开火时会制造的后坐力模长，默认为5
        /// </summary>
        public float Recoil = 5;
        /// <summary>
        /// 该枪械在开火时的一个转动角，用于快捷获取
        /// </summary>
        public virtual float GunOnFireRot => ToMouseA - OffsetRot * DirSign;
        /// <summary>
        /// 发射口的长度矫正值，默认为0
        /// </summary>
        public float ShootPosToMouLengValue = 0;
        /// <summary>
        /// 发射口的竖直方向长度矫正值，默认为0
        /// </summary>
        public float ShootPosNorlLengValue = 0;
        /// <summary>
        /// 快捷获取该枪械的发射口位置
        /// </summary>
        public Vector2 GunShootPos => GetShootPos(ShootPosToMouLengValue, ShootPosNorlLengValue);
        /// <summary>
        /// 玩家是否正在行走
        /// </summary>
        public virtual bool WalkDetection => Owner.velocity.Y == 0 && Math.Abs(Owner.velocity.X) > 0;
        /// <summary>
        /// 该枪体使用的实际纹理
        /// </summary>
        public virtual Texture2D TextureValue => CWRUtils.GetT2DValue(Texture);
        /// <summary>
        /// 更新枪压的作用状态，这个函数只应该由弹幕主人调用
        /// </summary>
        public virtual void UpdateRecoil() {
            OffsetRot -= ControlForce;
            if (OffsetRot <= 0) {
                OffsetRot = 0;
            }
        }
        /// <summary>
        /// 制造后坐力，这个函数只应该由弹幕主人调用，它不会自动调用，需要重写时在合适的代码片段中调用这个函数
        /// ，以确保制造后坐力的时机正确，一般在<see cref="BaseHeldRanged.SpanProj"/>中调用
        /// </summary>
        /// <returns>返回制造出的后坐力向量</returns>
        public virtual Vector2 CreateRecoil() {
            OffsetRot += GunPressure * OwnerPressureIncrease;
            Vector2 recoilVr = ShootVelocity.UnitVector() * (Recoil * -OwnerPressureIncrease);
            if (Math.Abs(Owner.velocity.X) < RangeOfStress && Math.Abs(Owner.velocity.Y) < RangeOfStress) {
                Owner.velocity += recoilVr;
            }
            return recoilVr;
        }
        /// <summary>
        /// 在枪械的更新周期中的最后被调用，用于复原一些数据
        /// </summary>
        public virtual void Recover() {
        }
        /// <summary>
        /// 一个快捷创建手持事件的方法，在<see cref="InOwner"/>中被调用，值得注意的是，如果需要更强的自定义效果，一般是需要直接重写<see cref="InOwner"/>的
        /// </summary>
        public virtual void FiringIncident() {
            if (Owner.PressKey()) {
                Owner.direction = ToMouse.X > 0 ? 1 : -1;
                Projectile.rotation = GunOnFireRot;
                Projectile.Center = Owner.Center + Projectile.rotation.ToRotationVector2() * HandFireDistance + new Vector2(0, HandFireDistanceY) + OffsetPos;
                ArmRotSengsBack = ArmRotSengsFront = (MathHelper.PiOver2 - Projectile.rotation) * DirSign;
                if (HaveAmmo && Projectile.IsOwnedByLocalPlayer()) {
                    onFire = true;
                    Projectile.ai[1]++;
                }
            }
            else {
                onFire = false;
            }

            if (Owner.PressKey(false) && !onFire && CanRightClick) {
                Owner.direction = ToMouse.X > 0 ? 1 : -1;
                Projectile.rotation = GunOnFireRot;
                Projectile.Center = Owner.Center + Projectile.rotation.ToRotationVector2() * HandFireDistance + new Vector2(0, HandFireDistanceY) + OffsetPos;
                ArmRotSengsBack = ArmRotSengsFront = (MathHelper.PiOver2 - Projectile.rotation) * DirSign;
                if (HaveAmmo && Projectile.IsOwnedByLocalPlayer()) {
                    onFireR = true;
                    Projectile.ai[1]++;
                }
            }
            else {
                onFireR = false;
            }
        }

        public override void InOwner() {
            ArmRotSengsFront = (60 + ArmRotSengsFrontNoFireOffset) * CWRUtils.atoR;
            ArmRotSengsBack = (110 + ArmRotSengsBackNoFireOffset) * CWRUtils.atoR;
            Projectile.Center = Owner.Center + new Vector2(DirSign * HandDistance, HandDistanceY);
            Projectile.rotation = DirSign > 0 ? MathHelper.ToRadians(AngleFirearmRest) : MathHelper.ToRadians(180 - AngleFirearmRest);
            Projectile.timeLeft = 2;
            SetHeld();
            if (!Owner.mouseInterface) {
                FiringIncident();
            }
        }
        /// <summary>
        /// 一个快捷创建发射事件的方法，在<see cref="SpanProj"/>中被调用，<see cref="BaseHeldRanged.onFire"/>为<see cref="true"/>才可能调用。
        /// 值得注意的是，如果需要更强的自定义效果，一般是需要直接重写<see cref="SpanProj"/>的
        /// </summary>
        public virtual void FiringShoot() {
            Projectile.NewProjectile(Owner.parent(), GunShootPos, ShootVelocity, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            _ = UpdateConsumeAmmo();
            _ = CreateRecoil();
        }
        /// <summary>
        /// 一个快捷创建发射事件的方法，在<see cref="SpanProj"/>中被调用，<see cref="onFireR"/>为<see cref="true"/>才可能调用。
        /// 值得注意的是，如果需要更强的自定义效果，一般是需要直接重写<see cref="SpanProj"/>的
        /// </summary>
        public virtual void FiringShootR() {
            Projectile.NewProjectile(Owner.parent(), GunShootPos, ShootVelocity, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
            _ = UpdateConsumeAmmo();
            _ = CreateRecoil();
        }
        /// <summary>
        /// 一个快捷创建属于卢克索饰品的发射事件，如果luxorsGift为<see langword="true"/>,
        /// 或者<see cref="CWRPlayer.theRelicLuxor"/>大于0，便会调用该方法，在Firing方法之后调用
        /// </summary>
        public virtual void LuxirEvent() {
            float damageMult = 1f;
            if (heldItem.useTime < 10) {
                damageMult -= (10 - heldItem.useTime) / 10f;
            }   
            int luxirDamage = Owner.ApplyArmorAccDamageBonusesTo(WeaponDamage * damageMult * 0.15f);
            if (luxirDamage > 1) {
                SpanLuxirProj(luxirDamage);
            }
        }
        /// <summary>
        /// 快速创建一个卢克索发射事件的方法，默认在<see cref="LuxirEvent"/>中调用
        /// </summary>
        /// <param name="luxirDamage"></param>
        /// <returns></returns>
        public virtual int SpanLuxirProj(int luxirDamage) {
            return Projectile.NewProjectile(Owner.parent(), GunShootPos, ShootVelocity
                , ModContent.ProjectileType<LuxorsGiftRanged>(), luxirDamage, WeaponKnockback / 2, Owner.whoAmI, 0);
        }
        /// <summary>
        /// 获取枪口位置，一般用于发射口的矫正
        /// </summary>
        /// <param name="toMouLeng"></param>
        /// <param name="norlLeng"></param>
        /// <returns></returns>
        public virtual Vector2 GetShootPos(float toMouLeng, float norlLeng) {
            Vector2 norlVr = (Projectile.rotation + (DirSign > 0 ? MathHelper.PiOver2 : -MathHelper.PiOver2)).ToRotationVector2();
            return Projectile.Center + Projectile.rotation.ToRotationVector2() * toMouLeng + norlVr * norlLeng;
        }
        /// <summary>
        /// 一个快捷的抛壳方法，需要自行调用
        /// </summary>
        /// <param name="slp"></param>
        public virtual void CaseEjection(float slp = 1) {
            Vector2 vr = (Projectile.rotation - Main.rand.NextFloat(-0.1f, 0.1f) * DirSign).ToRotationVector2() * -Main.rand.NextFloat(3, 7) + Owner.velocity;
            int proj = Projectile.NewProjectile(Projectile.parent(), Projectile.Center, vr, ModContent.ProjectileType<GunCasing>(), 10, Projectile.knockBack, Owner.whoAmI);
            Main.projectile[proj].scale = slp;
        }
        /// <summary>
        /// 一个快捷的创造开火烟尘粒子效果的方法
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="velocity"></param>
        /// <param name="splNum"></param>
        /// <param name="dustID1"></param>
        /// <param name="dustID2"></param>
        /// <param name="dustID3"></param>
        public virtual void SpawnGunFireDust(Vector2 pos = default, Vector2 velocity = default, int splNum = 1, int dustID1 = 262, int dustID2 = 54, int dustID3 = 53) {
            if (Main.myPlayer != Projectile.owner) return;
            if (pos == default) {
                pos = GunShootPos;
            }
            if (velocity == default) {
                velocity = ShootVelocity;
            }
            pos += velocity.SafeNormalize(Vector2.Zero) * Projectile.width * Projectile.scale * 0.71f;
            for (int i = 0; i < 30 * splNum; i++) {
                int dustID;
                switch (Main.rand.Next(6)) {
                    case 0:
                        dustID = dustID1;
                        break;
                    case 1:
                    case 2:
                        dustID = dustID2;
                        break;
                    default:
                        dustID = dustID3;
                        break;
                }
                float num = Main.rand.NextFloat(3f, 13f) * splNum;
                float angleRandom = 0.06f;
                Vector2 dustVel = new Vector2(num, 0f).RotatedBy((double)velocity.ToRotation(), default);
                dustVel = dustVel.RotatedBy(0f - angleRandom);
                dustVel = dustVel.RotatedByRandom(2f * angleRandom);
                if (Main.rand.NextBool(4)) {
                    dustVel = Vector2.Lerp(dustVel, -Vector2.UnitY * dustVel.Length(), Main.rand.NextFloat(0.6f, 0.85f)) * 0.9f;
                }
                float scale = Main.rand.NextFloat(0.5f, 1.5f);
                int idx = Dust.NewDust(pos, 1, 1, dustID, dustVel.X, dustVel.Y, 0, default, scale);
                Main.dust[idx].noGravity = true;
                Main.dust[idx].position = pos;
            }
        }

        public override void SpanProj() {
            if (Projectile.ai[1] > heldItem.useTime) {
                if (FiringDefaultSound) {
                    SoundEngine.PlaySound(heldItem.UseSound, Projectile.Center);
                }

                if (onFire) {
                    FiringShoot();
                }
                if (onFireR) {
                    FiringShootR();
                }
                if (Owner.Calamity().luxorsGift || Owner.CWR().theRelicLuxor > 0) {
                    LuxirEvent();
                }

                Projectile.ai[1] = 0;
                onFire = false;
            }
        }

        public override void AI() {
            InOwner();
            if (SetArmRotBool) {
                Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, ArmRotSengsFront * -DirSign);
                Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, ArmRotSengsBack * -DirSign);
            }
            if (Projectile.IsOwnedByLocalPlayer()) {
                UpdateRecoil();
                SpanProj();
            }
            Time++;
            Recover();
        }

        public override bool PreDraw(ref Color lightColor) {
            Main.EntitySpriteDraw(TextureValue, Projectile.Center - Main.screenPosition, null, onFire ? Color.White : lightColor
                , Projectile.rotation, TextureValue.Size() / 2, Projectile.scale, DirSign > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically);
            return false;
        }
    }
}
