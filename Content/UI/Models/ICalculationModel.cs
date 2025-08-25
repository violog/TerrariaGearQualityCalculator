using Terraria.Localization;
using TerrariaGearQualityCalculator.Calculators;

namespace TerrariaGearQualityCalculator.Content.UI.Models;

// ICalculationModel is a wrapper around ICalculation with localized and grouped text for display
public interface ICalculationModel
{
    // Name is localized boss name
    public string Name { get; }

    // Sr is Survivability Ratio that must be up-to-date with the original ICalculation.Sr
    public decimal Sr { get; }

    // DetailsAttributes are the public properties of every calculation, like SR, DPS, boss and player time etc.
    public static abstract LocalizedText[] DetailsAttributes { get; }

    // DetailsValues are the values matching DetailsAttributes
    public string[] DetailsValues { get; }
}