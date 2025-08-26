namespace TerrariaGearQualityCalculator.Calculators;

public interface ICalculation
{
    // Id is a boss NPCID (we will decide later for modded)
    public int Id { get; }
    public decimal BossTime { get; }

    public decimal PlayerTime { get; }

    // Sr is Survivability Ratio that effectively shows gear quality for the specific boss fight
    public decimal Sr => PlayerTime / BossTime;

    // ToModel generates a writable model of the calculation.
    // It is recommended to call this method once and cache the model,
    // updating only calculation values.
    public ICalculationModelWritable ToModel();
}