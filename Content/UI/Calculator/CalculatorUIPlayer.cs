using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using TGQC = TerrariaGearQualityCalculator.TerrariaGearQualityCalculator;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator;

public class CalculatorUIPlayer : ModPlayer
{
    private bool _isOpen;

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        var system = ModContent.GetInstance<CalculatorUISystem>();

        if (TGQC.CalculatorHotKey.JustPressed)
        {
            if (!_isOpen)
            {
                system.ShowUI();
            }
            else
            {
                system.HideUI();
            }

            _isOpen = !_isOpen;
        }

        if (Main.keyState.IsKeyDown(Keys.Escape))
        {
            system.HideUI();
            _isOpen = false;
        }
    }
}