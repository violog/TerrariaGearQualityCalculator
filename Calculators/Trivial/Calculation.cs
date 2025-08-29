using Terraria;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class TrivialCalculation(int id, int playerDps = 0, decimal playerTime = 0, decimal bossRemainingHp = 0)
    : ICalculation
{
    internal TrivialCalculation(NPC boss, decimal fightTime): this(boss.netID, 0, fightTime, boss.life)
    {
            PlayerDps = (int)((boss.lifeMax - boss.life) / fightTime);
    }
        
    public int Id { get; } = id;
    public decimal PlayerTime { get; } = playerTime;
    public decimal BossTime { get; } = bossRemainingHp / playerTime;
    public int PlayerDps { get; } = playerDps;
    public decimal BossRemainingHp { get; } = bossRemainingHp;

    public ICalculationModelWritable ToModel()
    {
        return new CalculationModel(this);
    }
}