namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class TrivialCalculation(int id, decimal playerDps = 0, decimal playerTime = 0, decimal bossRemainingHp = 0)
    : ICalculation
{
    public decimal PlayerDps { get; } = playerDps;
    public decimal BossRemainingHp { get; } = bossRemainingHp;
    public int Id { get; } = id;
    public decimal BossTime { get; } = bossRemainingHp / playerTime;
    public decimal PlayerTime { get; } = playerTime;

    public ICalculationModelWritable ToModel()
    {
        return new CalculationModel(this);
    }
}