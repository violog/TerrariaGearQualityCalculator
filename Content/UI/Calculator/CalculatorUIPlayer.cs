using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator
{
    public class CalculatorUIPlayer : ModPlayer
    {
        public bool IsCalculatorOpen = false;

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (TerrariaGearQualityCalculator.CalculatorHotKey.JustPressed && Main.netMode == NetmodeID.SinglePlayer)
            {
                ModContent.GetInstance<CalculatorUISystem>().ChangeDisplayUI(IsCalculatorOpen);

                IsCalculatorOpen = !IsCalculatorOpen;
            }
        }
    }
}