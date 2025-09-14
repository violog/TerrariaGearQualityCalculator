using System.Collections.Generic;
using Terraria;
using TerrariaGearQualityCalculator.Calculators.Trivial;
using TGQC = TerrariaGearQualityCalculator.TerrariaGearQualityCalculator;

namespace TerrariaGearQualityCalculator.Events;

public class Tracker(int npcId = 0)
{
    public int NpcId { get; } = npcId;
    public int FightTicks { get; set; }
    public List<PlayerHitEvent> Hits { get; } = [];
    public List<Item> Weapons { get; } = [];
    public bool IsEmpty => NpcId == 0 && FightTicks == 0 && Hits.Count == 0 && Weapons.Count == 0;

    internal TrivialCalculation CalcTrivial(NPC boss)
    {
        return !TGQC.IsSingleplayer
            ? null
            : new TrivialCalculation(Main.LocalPlayer, boss, FightTicks, Hits, Weapons);
    }
}