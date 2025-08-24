using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace TerrariaGearQualityCalculator
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class TerrariaGearQualityCalculator : Mod
	{
        public static ModKeybind CalculatorHotKey;

        public override void Load()
        {
            CalculatorHotKey = KeybindLoader.RegisterKeybind(this, "Gear Quality Calculator", "P");
        }

        public override void Unload()
        {
            CalculatorHotKey = null;
        }
    }
}
