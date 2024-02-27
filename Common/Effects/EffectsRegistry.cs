﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CalamityOverhaul.Common.Effects
{
    public static class EffectsRegistry
    {
        public static Filter ColourModulation => Filters.Scene["ColourModulation"];
        public static Texture2D Ticoninfinity;
        public static Effect ColourModulationShader;
        public static Effect PowerSFShader;
        public static MiscShaderData FlowColorShader;

        public static void LoadEffects() {
            var assets = CWRMod.Instance.Assets;
            LoadRegularShaders(assets);
        }

        public static void LoadRegularShaders(AssetRepository assets) {
            Ref<Effect> shockwave = new(assets.Request<Effect>(CWRConstant.noEffects + "Shockwave", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["Shockwave"] = new Filter(new(shockwave, "Shockwave"), EffectPriority.VeryHigh);

            Ref<Effect> colourModulation = new(assets.Request<Effect>(CWRConstant.noEffects + "ColourModulation", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["ColourModulation"] = new Filter(new(colourModulation, "GoldenPass"), EffectPriority.VeryHigh);

            Ref<Effect> flowColorShader = new(assets.Request<Effect>(CWRConstant.noEffects + "FlowColorShader", AssetRequestMode.ImmediateLoad).Value);
            GameShaders.Misc["CWRMod:FlowColorShader"] = new MiscShaderData(flowColorShader, "PiercePass");

            Ref<Effect> powerSFShader = new(assets.Request<Effect>(CWRConstant.noEffects + "PowerSFShader", AssetRequestMode.ImmediateLoad).Value);
            Filters.Scene["CWRMod:powerSFShader"] = new Filter(new(powerSFShader, "Offset"), EffectPriority.VeryHigh);

            Ticoninfinity = ModContent.Request<Texture2D>("CalamityOverhaul/Assets/UIs/Ticoninfinity", AssetRequestMode.ImmediateLoad).Value;
            ColourModulationShader = CWRMod.Instance.Assets.Request<Effect>(CWRConstant.noEffects + "ColourModulation", AssetRequestMode.ImmediateLoad).Value;
            PowerSFShader = CWRMod.Instance.Assets.Request<Effect>(CWRConstant.noEffects + "PowerSFShader", AssetRequestMode.ImmediateLoad).Value;
            FlowColorShader = GameShaders.Misc["CWRMod:FlowColorShader"];
        }
    }
}
