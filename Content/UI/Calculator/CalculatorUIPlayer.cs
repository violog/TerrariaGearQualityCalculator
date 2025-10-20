using Terraria.GameInput;
using Terraria.ModLoader;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator;

public class CalculatorUIPlayer : ModPlayer
{
    public bool IsCalculatorOpen;

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (TerrariaGearQualityCalculator.CalculatorHotKey.JustPressed)
        {
            ModContent.GetInstance<CalculatorUISystem>().ChangeDisplayUI(IsCalculatorOpen);

            IsCalculatorOpen = !IsCalculatorOpen;
        }
    }
}