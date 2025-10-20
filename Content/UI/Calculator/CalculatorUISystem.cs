using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TerrariaGearQualityCalculator.Calculators;

namespace TerrariaGearQualityCalculator.Content.UI.Calculator;

public class CalculatorUISystem : ModSystem
{
    private GameTime _lastUpdateUiGameTime;

    internal UserInterface CalculatorInterface;

    internal CalculatorUI CalculatorUi;
    internal IModelStorage Storage = TerrariaGearQualityCalculator.Storage;

    public override void Load()
    {
        CalculatorInterface = new UserInterface();

        CalculatorUi = new CalculatorUI();

        CalculatorUi.Activate();
    }

    public override void UpdateUI(GameTime gameTime)
    {
        _lastUpdateUiGameTime = gameTime;

        if (CalculatorInterface?.CurrentState != null) CalculatorInterface.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        var mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

        if (mouseTextIndex != -1)
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "TerrariaGearQualityCalculator: TerrariaGearQualityCalculatorInterface",
                delegate
                {
                    if (_lastUpdateUiGameTime != null && CalculatorInterface?.CurrentState != null)
                        CalculatorInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);

                    return true;
                },
                InterfaceScaleType.UI));
    }

    public void ChangeDisplayUI(bool isOpen = false)
    {
        if (isOpen)
            HideUI();
        else
            ShowUI();
    }

    public void ShowUI()
    {
        CalculatorInterface?.SetState(CalculatorUi);
    }

    public void HideUI()
    {
        CalculatorInterface?.SetState(null);
    }

    public override void Unload()
    {
        Storage = null;
        CalculatorInterface = null;
        CalculatorUi = null;
        _lastUpdateUiGameTime = null;
    }
}