using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TerrariaGearQualityCalculator.Calculators;
using TerrariaGearQualityCalculator.Storage;

namespace TerrariaGearQualityCalculator.Content.UI.Models;

internal class CalculatorModel(MemoryStorage storage)
{
    internal class BossEntryModel(string name, ICalculation calc)
    {
        // Name is not LocalizedText, because NPC.FullName string is already obtained by localization
        internal string Name { get; set; } = name;

        // TODO: uh-oh, can't use all calculations at once, huh?
        // damn, can't use type param, can I? I depend on class specific fields, I actually can not...
        // I would need to convert to string or another type and then parse. This is still possible - I can create calculation model!
        internal ICalculation Calculation { get; set; } = calc;
    }

    // internal IReadOnlyList<BossEntryModel> Bosses => _cachedBosses; // no! use a function, Luke!
    private MemoryStorage MemoryStorage { get; } = storage;

    // private BestiaryDatabase _bestiary { get; } = Main.BestiaryDB;
    // private static ContentSamples _bestiary { get; } = Main.BestiaryDB;
    private Dictionary<int, NPC> _cachedNPC { get; set; }

    // private List<string> _cachedNames { get; set; } = [];
    private List<BossEntryModel> _cachedBosses { get; set; } = []; // pass everything right away, no need to dup

    // internal List<string> GetLocalizedBossNames()
    internal List<BossEntryModel> BossList()
    {
        if (!MemoryStorage.CacheInvalid)
        {
            return _cachedBosses;
        }

        foreach (var boss in MemoryStorage.Calculations)
        {
            if (_cachedBosses[boss.Id] != null)
            {
                // update just the calculation if the boss is cached, as name doesn't change
                // this cache can be further optimized, but with ICalculationModel perhaps
                _cachedBosses[boss.Id].Calculation = boss;
                continue;
            }

            // this will work only for modded bosses!
            // ModNPC modNpc = NPCLoader.GetNPC(boss.Id);
            // modNpc.DisplayName
            var npc = _cachedNPC[boss.Id];
            if (npc == null)
            {
                // var entry = _bestiary.FindEntryByNPCID(boss.Id);
                npc = ContentSamples.NpcsByNetId[boss.Id];
                // this will probably throw on modded, must test
                _cachedNPC[boss.Id] = npc ?? throw new NullReferenceException($"NPC not found for id {boss.Id}");
            }

            if (!npc.boss)
            {
                throw new Exception($"NPC is not a boss: id={boss.Id} name={npc.FullName}");
            }

            _cachedBosses[boss.Id] = new BossEntryModel(npc.FullName, boss);
        }

        MemoryStorage.CacheInvalid = false;
        return _cachedBosses;
    }

    internal ICalculation BossByNpcId(int id)
    {
        return MemoryStorage.Calculations.First(c => c.Id == id);
    }
}