using System.Collections.Generic;
using TerrariaGearQualityCalculator.Calculators;

namespace TerrariaGearQualityCalculator.Storage;

public class MemoryStorage
{
    // Extra map for quick search is unnecessary, so List is enough, for 3 reasons:
    // 1. We will have at worst only ~200 boss entries
    // 2. Data is updated after every single fight, which can't normally happen more often than once a second
    public IReadOnlyList<ICalculation> Calculations => _calculations;
    
    private readonly List<ICalculation> _calculations;
    private IBackend Persistence { get; }
    
    public MemoryStorage() {}
    
    public MemoryStorage(List<ICalculation> calculations) => _calculations = calculations;

    public MemoryStorage(IBackend backend)
    {
        Persistence = backend;
        _calculations = Persistence.Load();
    }

    public void Save(ICalculation boss)
    {
        var i = _calculations.FindIndex(c => c.Id == boss.Id);
        if (i==-1) // returned when not found
        {
            _calculations.Add(boss);
        }
        else
        {
            _calculations[i] = boss;
        }
    }

    public ICalculation GetCalculationById(int id)
    {
        return _calculations.Find(c => c.Id == id);
    }
}