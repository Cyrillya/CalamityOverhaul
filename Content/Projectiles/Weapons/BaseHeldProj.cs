﻿using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.Projectiles.Weapons
{
    internal abstract class BaseHeldProj : ModProjectile
    {
        /// <summary>
        /// 一般情况下我们默认该弹幕的玩家作为弹幕主人，因此，弹幕的<see cref="Projectile.owner"/>属性需要被正确设置
        /// </summary>
        internal virtual Player Owner => Main.player[Projectile.owner];
        /// <summary>
        /// 安全的获取一个重力倒转值
        /// </summary>
        internal int SafeGravDir => Math.Sign(Owner.gravDir);
        /// <summary>
        /// 弹幕的理论朝向，这里考虑并没有到<see cref="Player.gravDir"/>属性，为了防止玩家在重力反转的情况下出现问题，可能需要额外编写代码
        /// </summary>
        internal virtual int DirSign => Owner.direction * SafeGravDir;
        /// <summary>
        /// 这个值用于在联机同步中使用，一般来讲，应该使用<see cref="ToMouse"/>
        /// </summary>
        private Vector2 toMouseVecterDate;
        private Vector2 _old_toMouseVecterDate;
        private const float toMouseVer_variationMode = 0.5f;
        /// <summary>
        /// 是否处于开火时间
        /// </summary>
        public virtual bool CanFire => false;
        /// <summary>
        /// 获取玩家到鼠标的向量
        /// </summary>
        internal virtual Vector2 ToMouse { get; private set; }
        /// <summary>
        /// 获取玩家到鼠标的角度
        /// </summary>
        internal virtual float ToMouseA => ToMouse.ToRotation();
        /// <summary>
        /// 获取玩家鼠标的单位向量
        /// </summary>
        internal virtual Vector2 UnitToMouseV => ToMouse.UnitVector();

        public override void SendExtraAI(BinaryWriter writer) {
            writer.WriteVector2(toMouseVecterDate);
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            toMouseVecterDate = reader.ReadVector2();
        }

        public Vector2 UpdateToMouse() {
            if (Projectile.IsOwnedByLocalPlayer()) {
                toMouseVecterDate = Owner.GetPlayerStabilityCenter().To(Main.MouseWorld);
                bool difference = Math.Abs(toMouseVecterDate.X - _old_toMouseVecterDate.X) > toMouseVer_variationMode
                    || Math.Abs(toMouseVecterDate.Y - _old_toMouseVecterDate.Y) > toMouseVer_variationMode;
                if (difference && CanFire) {
                    NetUpdate();
                }
                _old_toMouseVecterDate = toMouseVecterDate;
            }
            return toMouseVecterDate;
        }

        public virtual void ExtraPreSet() {

        }

        public virtual bool PreUpdate() {
            return true;
        }

        public sealed override bool PreAI() {
            ToMouse = UpdateToMouse();
            ExtraPreSet();
            return PreUpdate();
        }

        public sealed override void PostAI() {
            if (!Owner.Alives()) {
                Projectile.Kill();
            }
        }

        protected void SetHeld() => Owner.heldProj = Projectile.whoAmI;

        protected void NetUpdate() => Projectile.netUpdate = true;
    }
}
