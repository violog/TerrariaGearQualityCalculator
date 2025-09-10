using System.Collections.Generic;
using Terraria;
using TerrariaGearQualityCalculator.Calculators.Trivial;

namespace TerrariaGearQualityCalculator.Events;

public class Tracker
{
    protected internal int FightTicks { get; set; }
    protected internal List<PlayerHitEvent> Hits { get; } = [];

    internal TrivialCalculation CalcTrivial(NPC boss)
    {
        return new TrivialCalculation(Main.LocalPlayer, boss, FightTicks, null);
    }
}