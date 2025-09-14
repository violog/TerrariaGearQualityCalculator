using System.Collections.Generic;
using Terraria;
using TerrariaGearQualityCalculator.Events;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class TrivialCalculation(int id)
    : ICalculation
{
    private const double TicksPerSecond = 60;

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

        BossDps = (int)dps;

        if (player.statLife == 0)
            PlayerTime = fightTimeSec;
        else
            // This is the right measurement, because all life regen and health potions
            // have been taken into account while counting BossDps.
            // This should be tested and improved, e.g. for the case when player uses big
            // health potion and kills the boss too quickly, not counting health potion cooldown
            // if the fight would proceed further.
            PlayerTime = (double)player.statLifeMax / BossDps;
    }

    public int PlayerDps { get; }
    public int BossRemainingHp { get; }
    public int BossDps { get; }
    public PlayerGear Gear { get; } = new();

    public int Id { get; } = id;
    public double PlayerTime { get; }
    public double BossTime { get; }

    public ICalculationModelWritable ToModel()
    {
        return new CalculationModel(this);
    }
}