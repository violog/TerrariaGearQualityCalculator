using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;

namespace TerrariaGearQualityCalculator.Calculators.Trivial;

internal class PlayerGear
{
    private static readonly AccessorySlotLoader Loader = LoaderManager.Get<AccessorySlotLoader>();

    public PlayerGear(Player player)
    {
        Weapon = player.inventory[0].AffixName(); // TODO: make slot number configurable
        Helmet = player.armor[0].AffixName();
        Chest = player.armor[1].AffixName();
        Legs = player.armor[2].AffixName();
        for (var i = 3; i < 8 + player.extraAccessorySlots; i++)
            Accessories.Add(player.armor[i].AffixName());

        var modPlayer = player.GetModPlayer<ModAccessorySlotPlayer>();
        // Modded accessories are stored separately
        for (var i = 0; i < modPlayer.SlotCount; i++)
        {
            var slot = Loader.Get(i, player);
            if (slot is null or UnloadedAccessorySlot)
                continue;
            Accessories.Add(slot.FunctionalItem.AffixName());
        }
    }

    public string Weapon { get; }
    public string Helmet { get; }
    public string Chest { get; }
    public string Legs { get; }
    public List<string> Accessories { get; } = [];
}