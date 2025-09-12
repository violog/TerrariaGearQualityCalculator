using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class PlayerGear()
{
    private const string MissingName = "--";
    private static readonly AccessorySlotLoader Loader = LoaderManager.Get<AccessorySlotLoader>();

    public PlayerGear(Player player) : this()
    {
        Weapon = GetItemName(player.inventory[0]); // TODO: make slot number configurable
        Helmet = GetItemName(player.armor[0]);
        Chest = GetItemName(player.armor[1]);
        Legs = GetItemName(player.armor[2]);
        for (var i = 3; i < 8 + player.extraAccessorySlots; i++)
            _accessories.Add(GetItemName(player.armor[i]));

        var modPlayer = player.GetModPlayer<ModAccessorySlotPlayer>();
        // Modded accessories are stored separately
        for (var i = 0; i < modPlayer.SlotCount; i++)
        {
            var slot = Loader.Get(i, player);
            if (slot is null or UnloadedAccessorySlot)
                continue;
            _accessories.Add(GetItemName(slot.FunctionalItem));
        }
    }

    public string Weapon { get; } = MissingName;
    public string Helmet { get; } = MissingName;
    public string Chest { get; } = MissingName;
    public string Legs { get; } = MissingName;
    public string[] Accessories => _accessories.Count > 0 ? _accessories.ToArray() : [MissingName];
    private List<string> _accessories { get; } = [];

    private static string GetItemName(Item item)
    {
        return item.AffixName() == "" ? MissingName : item.AffixName();
    }
}