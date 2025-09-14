namespace TerrariaGearQualityCalculator.Calculators;

// ICalculationModel is a wrapper around ICalculation with localized and grouped text for display.
// This interface is intended for backend-end read-write usage.
public interface ICalculationModelWritable : ICalculationModel
{
    // Update updates details view by the respective calculation passed.
    // ICalculation must implement the respective underlying calculation.
    public void Update(ICalculation calculation);
}