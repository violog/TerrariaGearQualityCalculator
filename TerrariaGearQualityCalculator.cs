using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaGearQualityCalculator;

// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
internal sealed class TerrariaGearQualityCalculator : Mod
{
    internal static ModKeybind CalculatorHotKey;

    public override void Load()
    {
        if (Main.netMode != NetmodeID.SinglePlayer)
        {
            // TODO: properly load/unload the mod: no logic should be executed if the mod isn't loaded, not just a hotkey
            Logger.Error("TerrariaGearQualityCalculator is not supported in multiplayer!");
            return;
        }

        CalculatorHotKey = KeybindLoader.RegisterKeybind(this, "GearQualityCalculator", "P");
    }

    public override void Unload()
    {
        CalculatorHotKey = null;
    }
}