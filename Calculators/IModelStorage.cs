using System.Collections.Generic;

namespace TerrariaGearQualityCalculator.Calculators;

// IModelStorage is a front-end interface to obtain boss list
public interface IModelStorage
{
    public IReadOnlyList<ICalculationModel> BossList { get; }
}