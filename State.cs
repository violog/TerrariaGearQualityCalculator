using System;
using System.IO;
using Terraria;
using TerrariaGearQualityCalculator.Calculators.Trivial;
using TerrariaGearQualityCalculator.Storage;

namespace TerrariaGearQualityCalculator;

// State is a singleton communication layer between UI and backend
internal sealed class State
{
    private const string FileName = "TrivialCalculation.json";

    // Lazy providees thread safety for singleton initialisation
    private static readonly Lazy<State> Lazy = new(() => new State());
    internal static State Instance => Lazy.Value;
    internal ModelStorage Storage { get; private set; }

    private State()
    {
        var backend = new FileBackend<TrivialCalculation>(FileName);
        Storage = new ModelStorage(backend);
    }
}