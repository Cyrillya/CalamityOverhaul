using CalamityOverhaul.Common;
using CalamityOverhaul.Common.Effects;
using CalamityOverhaul.Content;
using CalamityOverhaul.Content.Items;
using CalamityOverhaul.Content.NPCs.Core;
using CalamityOverhaul.Content.NPCs.HeavenEaters;
using CalamityOverhaul.Content.Particles.Core;
using CalamityOverhaul.Content.RemakeItems.Core;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul
{
    public class CWRMod : Mod
    {
        internal static CWRMod Instance;
        internal static int GameLoadCount;
        internal Mod musicMod = null;
        internal Mod betterWaveSkipper = null;
        internal Mod fargowiltasSouls = null;
        internal Mod catalystMod = null;
        internal Mod weaponOut = null;

        internal List<Mod> LoadMods = new List<Mod>();
        internal static List<BaseRItem> RItemInstances = new List<BaseRItem>();
        internal static List<EctypeItem> EctypeItemInstance = new List<EctypeItem>();
        internal static List<NPCCustomizer> NPCCustomizerInstances = new List<NPCCustomizer>();

        public override void PostSetupContent() {
            LoadMods = ModLoader.Mods.ToList();

            {
                RItemInstances = new List<BaseRItem>();//����ֱ�ӽ��г�ʼ�����㲻����Ҫ����UnLoadж��
                List<Type> rItemIndsTypes = CWRUtils.GetSubclasses(typeof(BaseRItem));
                foreach (Type type in rItemIndsTypes) {
                    if (type != typeof(BaseRItem)) {
                        object obj = Activator.CreateInstance(type);
                        if (obj is BaseRItem inds) {
                            inds.Load();
                            inds.SetStaticDefaults();
                            //������ж�һ��TargetID�Ƿ�Ϊ0����Ϊ�������һ����Ч��Ritemʵ������ô����TargetID�Ͳ�����Ϊ0����������ӽ�ȥ�ᵼ��LoadRecipe���ֱ���
                            if (inds.TargetID != 0)
                                RItemInstances.Add(inds);
                        }
                    }
                }
            }

            {
                EctypeItemInstance = new List<EctypeItem>();
                List<Type> ectypeIndsTypes = CWRUtils.GetSubclasses(typeof(BaseRItem));
                foreach (Type type in ectypeIndsTypes) {
                    if (type != typeof(EctypeItem)) {
                        object obj = Activator.CreateInstance(type);
                        if (obj is EctypeItem inds) {
                            EctypeItemInstance.Add(inds);
                        }
                    }
                }
            }

            {
                NPCCustomizerInstances = new List<NPCCustomizer>();//����ֱ�ӽ��г�ʼ�����㲻����Ҫ����UnLoadж��
                List<Type> npcCustomizerIndsTypes = CWRUtils.GetSubclasses(typeof(NPCCustomizer));
                foreach (Type type in npcCustomizerIndsTypes) {
                    if (type != typeof(NPCCustomizer)) {
                        object obj = Activator.CreateInstance(type);
                        if (obj is NPCCustomizer inds) {
                            NPCCustomizerInstances.Add(inds);
                        }
                    }
                }
            }

            //����һ��ID�б���������ؿ��Ա������������Ѿ���Ӻ���
            CWRIDs.Load();
        }

        public override void Load() {
            Instance = this;
            GameLoadCount++;
            new InWorldBossPhase().Load();

            FindMod();
            ModGanged.Load();

            LoadClient();

            //����ͷ����ԴIcon
            HEHead.LoadHaedIcon();

            CWRParticleHandler.Load();
            EffectsRegistry.LoadEffects();
            On_Main.DrawInfernoRings += PeSystem.CWRDrawForegroundParticles;

            base.Load();
        }

        public override void Unload() {
            CWRParticleHandler.Unload();
            On_Main.DrawInfernoRings -= PeSystem.CWRDrawForegroundParticles;
            base.Unload();
        }

        public void FindMod() {
            ModLoader.TryGetMod("CalamityModMusic", out musicMod);
            ModLoader.TryGetMod("BetterWaveSkipper", out betterWaveSkipper);
            ModLoader.TryGetMod("FargowiltasSouls", out fargowiltasSouls);
            ModLoader.TryGetMod("CatalystMod", out catalystMod);
            ModLoader.TryGetMod("WeaponOut", out weaponOut);
        }

        public void LoadClient() {
            if (Main.dedServ)
                return;

            MusicLoader.AddMusicBox(Instance, MusicLoader.GetMusicSlot("CalamityOverhaul/Assets/Sounds/Music/BuryTheLight")
                , Find<ModItem>("FoodStallChair").Type, Find<ModTile>("FoodStallChair").Type, 0);
        }
    }
}