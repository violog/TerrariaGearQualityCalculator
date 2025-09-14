using System.Collections.Generic;
using Terraria.ID;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal static class Initializer
{
    internal static List<ICalculation> Init()
    {
        var result = new List<ICalculation>(50);

        foreach (var pair in ContentSamples.NpcsByNetId)
        {
            var npc = pair.Value;
            if (!npc.boss) continue;

            var calc = new TrivialCalculation(npc.netID);
            result.Add(calc);
        }

        return result;
    }
}