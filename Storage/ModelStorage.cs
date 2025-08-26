using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using TerrariaGearQualityCalculator.Calculators;

namespace TerrariaGearQualityCalculator.Storage;

// ModelStorage is a wrapper around MemoryStorage to store models
public class ModelStorage : MemoryStorage
{
    public IReadOnlyList<ICalculationModel> BossList => Models;
    private List<ICalculationModelWritable> Models { get; }

    public ModelStorage(IBackend backend) : base(backend)
    {
        Models = new List<ICalculationModelWritable>(Calculations.Count);
        foreach (var calc in Calculations)
        {
            Models.Add(calc.ToModel());
        }
    }

    public override int Save(ICalculation boss)
    {
        var i = base.Save(boss);
        if (Calculations.Count > Models.Count)
        {
            Models.Add(boss.ToModel());
        }
        else
        {
            Models[i].Update(boss);
        }

        return i;
    }
}