using Terraria.Localization;

namespace TerrariaGearQualityCalculator.Calculators;

// ICalculationModel is a wrapper around ICalculation with localized and grouped text for display.
// This interface is intended for front-end read-only usage.
public interface ICalculationModel
{
    // Name is localized boss name
    public string Name { get; }

    // Sr is Survivability Ratio that must be up-to-date with the original ICalculation.Sr
    public string Sr { get; }

    // DetailsAttributes are the public properties of every calculation, like SR, DPS, boss and player time etc.
    public LocalizedText[] DetailsAttributes { get; }

    // DetailsValues are the values matching DetailsAttributes
    public string[] DetailsValues { get; }
}