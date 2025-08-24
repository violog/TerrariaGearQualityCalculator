namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class TrivialCalculation(int id, decimal playerDps, decimal playerTime, decimal bossRemainingHp) : ICalculation
{
    public int Id { get; } = id;
    public decimal PlayerDps { get; } =  playerDps;
    public decimal BossRemainingHp = bossRemainingHp;
    public decimal BossTime { get; } = bossRemainingHp / playerTime;
    public decimal PlayerTime { get; } = playerTime;
    
    // I will move this in the future
    // public override string ToString()
    // {
    //     var name = NPCID.Search.GetName(Id);
    //     return name != "" ? name : throw new Exception($"Name does not exist for NPC ID {Id}");
    // }
}