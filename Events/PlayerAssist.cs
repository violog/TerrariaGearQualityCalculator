using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaGearQualityCalculator.Storage;

namespace TerrariaGearQualityCalculator.Events;

// Reused thanks to https://github.com/JavidPack/BossChecklist
internal class PlayerAssist : ModPlayer
{
    internal Dictionary<int, Tracker> Trackers { get; } = [];
    private readonly ModelStorage _storage = State.Instance.Storage;

    public override void PreUpdate()
    {
        foreach (var tracker in Trackers.Values)
        {
            tracker.PlayerTicks++;
        }
    }

    // Track player deaths during boss fights.
    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        foreach (var id in player.Trackers.Keys)
        {
            player.Trackers.Remove(id, out var tracker);
            _storage.Save(tracker!.CalcTrivial);
        }
    }
}