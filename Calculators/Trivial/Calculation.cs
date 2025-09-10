using System.Collections.Generic;
using Terraria;
using TerrariaGearQualityCalculator.Events;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class TrivialCalculation(int id)
    : ICalculation
{
    internal const decimal Infinity = decimal.MaxValue;
    private const decimal TicksPerSecond = 60;

    internal TrivialCalculation(Player player, NPC boss, int fightTimeTicks, List<PlayerHitEvent> hits) :
        this(boss.netID)
    {
        var fightTimeSec = fightTimeTicks / TicksPerSecond;
        BossRemainingHp = boss.life;
        PlayerDps = (int)((boss.lifeMax - boss.life) / fightTimeSec);
        BossTime = (decimal)boss.lifeMax / PlayerDps;

        var prev = new PlayerHitEvent(player.statLifeMax, 0);
        decimal dps = 0;

        foreach (var hit in hits)
        {
            var dmg = prev.Health - hit.Health;
            var dt = (hit.Timestamp - prev.Timestamp) / TicksPerSecond;
            if (dt == 0) continue;
            dps += dmg / dt;
            prev = hit;
        }

        BossDps = (int)dps;

        if (player.statLife == 0)
            PlayerTime = fightTimeSec;
        else
            PlayerTime = BossDps == 0 ? Infinity : (decimal)player.statLifeMax / BossDps;
    }

    internal int PlayerDps { get; }
    internal int BossRemainingHp { get; }
    internal int BossDps { get; }

    public int Id { get; } = id;
    public decimal PlayerTime { get; }
    public decimal BossTime { get; }

    public ICalculationModelWritable ToModel()
    {
        return new CalculationModel(this);
    }
}