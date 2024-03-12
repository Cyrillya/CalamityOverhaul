﻿using CalamityOverhaul.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace CalamityOverhaul.Content.Projectiles.Weapons.Ranged.HeldProjs.Vanilla
{
    internal class ProximityMineLauncherHeldProj : BaseFeederGun
    {
        public override string Texture => CWRConstant.Placeholder;
        public override Texture2D TextureValue => TextureAssets.Item[ItemID.ProximityMineLauncher].Value;
        public override int targetCayItem => ItemID.ProximityMineLauncher;
        public override int targetCWRItem => ItemID.ProximityMineLauncher;
        public override void SetRangedProperty() {
            FireTime = 30;
            ShootPosToMouLengValue = 0;
            ShootPosNorlLengValue = 0;
            HandDistance = 15;
            HandDistanceY = 0;
            GunPressure = 0.8f;
            ControlForce = 0.05f;
            RepeatedCartridgeChange = true;
            Recoil = 4.8f;
            RangeOfStress = 48;
            kreloadMaxTime = 60;
        }

        public override void PreInOwnerUpdate() {
            if (kreloadTimeValue > 0) {//设置一个特殊的装弹动作，调整转动角度和中心点，让枪身看起来上抬
                Owner.direction = ToMouse.X > 0 ? 1 : -1;//为了防止抽搐，这里额外设置一次玩家朝向
                FeederOffsetRot = -MathHelper.ToRadians(30) * DirSign;
                FeederOffsetPos = new Vector2(0, -13);
            }
        }

        public override bool KreLoadFulfill() {
            if (BulletNum < 16) {
                BulletNum += 4;
            } else {
                BulletNum = 20;
            }
            if (Item.CWR().AmmoCapacityInFire) {
                Item.CWR().AmmoCapacityInFire = false;
            }
            return true;
        }

        public override void FiringShoot() {
            //火箭弹药特判，感应雷特判
            Item ammoItem = Item.CWR().MagazineContents[0];
            if (ammoItem.type == ItemID.RocketI) {
                AmmoTypes = ProjectileID.ProximityMineI;
            }
            if (ammoItem.type == ItemID.RocketII) {
                AmmoTypes = ProjectileID.ProximityMineII;
            }
            if (ammoItem.type == ItemID.RocketIII) {
                AmmoTypes = ProjectileID.ProximityMineIII;
            }
            if (ammoItem.type == ItemID.RocketIV) {
                AmmoTypes = ProjectileID.ProximityMineIV;
            }
            if (ammoItem.type == ItemID.ClusterRocketI) {
                AmmoTypes = ProjectileID.ClusterMineI;
            }
            if (ammoItem.type == ItemID.ClusterRocketII) {
                AmmoTypes = ProjectileID.ClusterMineII;
            }
            if (ammoItem.type == ItemID.DryRocket) {
                AmmoTypes = ProjectileID.DryMine;
            }
            if (ammoItem.type == ItemID.WetRocket) {
                AmmoTypes = ProjectileID.WetMine;
            }
            if (ammoItem.type == ItemID.HoneyRocket) {
                AmmoTypes = ProjectileID.HoneyMine;
            }
            if (ammoItem.type == ItemID.LavaRocket) {
                AmmoTypes = ProjectileID.LavaMine;
            }
            if (ammoItem.type == ItemID.MiniNukeI) {
                AmmoTypes = ProjectileID.MiniNukeMineI;
            }
            if (ammoItem.type == ItemID.MiniNukeII) {
                AmmoTypes = ProjectileID.MiniNukeMineII;
            }
            SpawnGunFireDust(GunShootPos, ShootVelocity);
            Projectile.NewProjectile(Source, GunShootPos, ShootVelocity, AmmoTypes, WeaponDamage, WeaponKnockback, Owner.whoAmI, 0);
        }
    }
}