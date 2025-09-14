using System.Collections.Generic;
using TerrariaGearQualityCalculator.Calculators;
using TGQC = TerrariaGearQualityCalculator.TerrariaGearQualityCalculator;

namespace TerrariaGearQualityCalculator.Storage;

// ModelStorage is a wrapper around MemoryStorage to store models
public class ModelStorage : MemoryStorage, IModelStorage
{
    public ModelStorage(IBackend backend) : base(backend)
    {
        Models = new List<ICalculationModelWritable>(Calculations.Count);
        foreach (var calc in Calculations) Models.Add(calc.ToModel());
    }

    private List<ICalculationModelWritable> Models { get; }
    public IReadOnlyList<ICalculationModel> BossList => Models;

    public override int Save(ICalculation boss)
    {
        if (!TGQC.IsSingleplayer) return 0;

        var i = base.Save(boss);
        if (Calculations.Count > Models.Count)
            Models.Add(boss.ToModel());
        else
            Models[i].Update(boss);

        TGQC.Log.Debug($"Saved calc: {boss.ToModel()}");
        return i;
    }
}