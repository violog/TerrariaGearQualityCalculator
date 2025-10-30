using System.Collections.Generic;
using System.Text.Json.Serialization;
using Terraria;
using TerrariaGearQualityCalculator.Events;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

// ReSharper disable UnusedAutoPropertyAccessor.Global
internal class TrivialCalculation
    : ICalculation
{
    private const double TicksPerSecond = 60;

    [JsonConstructor]
    public TrivialCalculation()
    {
    }

    public TrivialCalculation(int id) => Id = id;

    internal TrivialCalculation(Player player, NPC boss, int fightTimeTicks, List<PlayerHitEvent> hits,
        List<Item> weapons) :
        this(boss.netID)
    {
        var fightTimeSec = fightTimeTicks / TicksPerSecond;
        Gear = new PlayerGear(player, weapons);
        BossRemainingHp = boss.life;
        // Can't deal damage in 0 ticks
        PlayerDps = fightTimeSec == 0 ? 0 : (int)((boss.lifeMax - boss.life) / fightTimeSec);
        BossTime = (double)boss.lifeMax / PlayerDps;

        var prev = new PlayerHitEvent(player.statLifeMax, 0);
        double dps = 0;

        foreach (var hit in hits)
        {
            var dmg = prev.Health - hit.Health;
            var dt = (hit.Timestamp - prev.Timestamp) / TicksPerSecond;
            if (dt == 0) continue;
            dps += dmg / dt;
            prev = hit;
        }

        BossDps = (int)dps / hits.Count;

        if (player.statLife == 0)
        {
            PlayerTime = fightTimeSec;
            return;
        }

        double dmgReceived = player.statLifeMax - player.statLife;
        PlayerTime = player.statLifeMax / dmgReceived * fightTimeSec;
    }

    public int PlayerDps { get; set; }
    public int BossRemainingHp { get; set; }
    public int BossDps { get; set; }
    public PlayerGear Gear { get; set; } = new();

    public int Id { get; set; }
    public double PlayerTime { get; set; }
    public double BossTime { get; set; }

    public ICalculationModelWritable ToModel()
    {
        return CalculationModel.TryCreate(this);
    }
}