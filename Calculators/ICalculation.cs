namespace TerrariaGearQualityCalculator.Calculators;

public interface ICalculation
{
    // Id is a boss NPCID (we will decide later for modded)
    public int Id { get; }
    public decimal BossTime { get; }

    public decimal PlayerTime { get; }

    // Sr is Survivability Ratio that effectively shows gear quality for the specific boss fight
    public decimal Sr => PlayerTime / BossTime;

    // CacheValid shows whether the cache can be reused, it should be updated accordingly
    // It is convenient to store on lowest calculation level and propagate as values change
    public bool CacheValid { get; set; }
}