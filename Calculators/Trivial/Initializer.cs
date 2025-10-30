using System.Collections.Generic;
using Terraria.ID;
using TGQC = TerrariaGearQualityCalculator.TerrariaGearQualityCalculator;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal static class Initializer
{
    internal static List<ICalculation> Init()
    {
        var result = new List<ICalculation>(50);

        // TODO: The Twins are 2 separate boss entries; also The Torch God, Moon Lord's parts got into entries, despite not being bosses
        foreach (var pair in ContentSamples.NpcsByNetId)
        {
            var npc = pair.Value;
            if (!npc.boss) continue;

            var calc = new TrivialCalculation(npc.netID);
            result.Add(calc);
        }

        TGQC.Log.Info($"Initialized list with {result.Count} bosses.");
        return result;
    }
}