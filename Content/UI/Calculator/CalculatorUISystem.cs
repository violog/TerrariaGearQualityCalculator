using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator;

public class CalculatorUISystem : ModSystem
{
    private UserInterface _calculatorInterface;
    private CalculatorUI _calculatorUi;

    public override void PostSetupContent()
    {
        _calculatorInterface = new UserInterface();
        _calculatorUi = new CalculatorUI();
        _calculatorUi.Activate();
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (_calculatorInterface?.CurrentState != null)
        {
            _calculatorInterface?.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        var mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        if (mouseTextIndex != -1)
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "TerrariaGearQualityCalculator: TerrariaGearQualityCalculatorInterface",
                delegate
                {
                    if (_calculatorInterface?.CurrentState != null)
                        _calculatorInterface.Draw(Main.spriteBatch, new GameTime());

                    return true;
                },
                InterfaceScaleType.UI));
    }

    internal void ShowUI()
    {
        _calculatorInterface?.SetState(_calculatorUi);
    }

    internal void HideUI()
    {
        _calculatorInterface?.SetState(null);
    }

    public override void Unload()
    {
        _calculatorInterface = null;
        _calculatorUi = null;
    }
}