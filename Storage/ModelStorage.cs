using System.Collections.Generic;
using TerrariaGearQualityCalculator.Calculators;
using TGQC = TerrariaGearQualityCalculator.TerrariaGearQualityCalculator;

namespace TerrariaGearQualityCalculator.Storage;

// ModelStorage is a wrapper around MemoryStorage to store models
public class ModelStorage : IModelStorage
{
    // Extra map for quick search is unnecessary, so List is enough, for 3 reasons:
    // 1. We will have at worst only ~200 boss entries
    // 2. Data is updated after every single fight, which can't normally happen more often than once a second
    private readonly List<ICalculation> _calculations;

    private IBackend Persistence { get; }

    public ModelStorage(IBackend backend)
    {
        Persistence = backend;
        _calculations = Persistence.Load();
        Models = new List<ICalculationModelWritable>(_calculations.Count);

        foreach (var calc in _calculations)
        {
            var model = calc.ToModel();
            if (model is null) continue; // happens when the mod for calculation was unloaded
            Models.Add(model);
        }
    }

    private List<ICalculationModelWritable> Models { get; }
    public IReadOnlyList<ICalculationModel> BossList => Models;

    public void Save(ICalculation boss)
    {
        if (!TGQC.IsSingleplayer) return;

        var i = _calculations.FindIndex(c => c.Id == boss.Id);
        var model = boss.ToModel();
        TGQC.Log.Debug($"ModelStorage: updating boss at index={i}: {model}");
        if (i == -1)
        {
            _calculations.Add(boss);
            Models.Add(model);
            Persistence.Store(_calculations);
            return;
        }

        _calculations[i] = boss;
        Models[i].Update(boss);
        Persistence.Store(_calculations);
    }
}