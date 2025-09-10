using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TerrariaGearQualityCalculator.Storage;

namespace TerrariaGearQualityCalculator.Events;

// Reused thanks to https://github.com/JavidPack/BossChecklist
internal class PlayerAssist : ModPlayer
{
    private readonly ModelStorage _storage = State.Instance.Storage;

    // TODO: maybe, refactor this to a single tracker, as multi-boss fights aren't supported
    internal Dictionary<int, Tracker> Trackers { get; } = [];

    public override void PreUpdate()
    {
        foreach (var tracker in Trackers.Values) tracker.FightTicks++;
    }

    // If you are fighting several bosses simultaneously or get hit by smth else,
    // it still counts into the fight of every boss - intended behavior.
    // The calculator is not designed for multiple bosses fights.
    public override void OnHurt(Player.HurtInfo info)
    {
        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        foreach (var tr in player.Trackers.Values)
            tr.Hits.Add(new PlayerHitEvent(player.Player.statLife, tr.FightTicks));
    }

    // Track player deaths during boss fights.
    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        var npcId = damageSource.SourceNPCIndex;
        NPC boss;
        try
        {
            boss = Main.npc.First(n => n.netID == npcId && n.boss);
        }
        catch (InvalidOperationException)
        {
            // FIXME possible workaround: track last known boss health on every hit
            Main.NewText(
                $"[ERROR] npc id={npcId} has despawned, was not found or is not a boss",
                255, 0, 0);
            return;
        }

        // will not fetch all other bosses and record
        player.Trackers.Remove(npcId, out var tracker);
        _storage.Save(tracker!.CalcTrivial(boss));
    }
}