using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaGearQualityCalculator;

// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
internal class TerrariaGearQualityCalculator : Mod
{
    public static ModKeybind CalculatorHotKey;

    public override void Load()
    {
        if (Main.netMode != NetmodeID.SinglePlayer)
        {
            Main.NewText($"TerrariaGearQualityCalculator is not supported in multiplayer!", 255, 0, 0);
            return;
        }

        CalculatorHotKey = KeybindLoader.RegisterKeybind(this, "GearQualityCalculator", "P");
    }

    public override void Unload()
    {
        CalculatorHotKey = null;
    }
}