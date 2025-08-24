using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria;
using Terraria.ModLoader;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator
{
    public class CalculatorUIPlayer : ModPlayer
    {
        public bool IsCalculatorOpen = false;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (TerrariaGearQualityCalculator.CalculatorHotKey.JustPressed)
            {
                ModContent.GetInstance<CalculatorUISystem>().ChangeDisplayUI(IsCalculatorOpen);

                IsCalculatorOpen = !IsCalculatorOpen;
            }
        }
    }
}
