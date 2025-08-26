namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class TrivialCalculation(int id, decimal playerDps, decimal playerTime, decimal bossRemainingHp) : ICalculation
{
    public int Id { get; } = id;
    public decimal PlayerDps { get; } = playerDps;
    public decimal BossRemainingHp { get; } = bossRemainingHp;
    public decimal BossTime { get; } = bossRemainingHp / playerTime;
    public decimal PlayerTime { get; } = playerTime;

    public ICalculationModelWritable ToModel()
    {
        return new CalculationModel(this);
    }
}