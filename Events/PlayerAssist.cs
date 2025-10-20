using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using TerrariaGearQualityCalculator.Storage;
using TGQC = TerrariaGearQualityCalculator.TerrariaGearQualityCalculator;

namespace TerrariaGearQualityCalculator.Events;

// Reused thanks to https://github.com/JavidPack/BossChecklist
internal class PlayerAssist : ModPlayer
{
    private readonly ModelStorage _storage = TGQC.Storage;

    // Tracker tracks stats only per one boss at the same times
    internal Tracker Tracker { get; set; } = new();

    public override void PreUpdate()
    {
        if (!TGQC.IsSingleplayer || Tracker.IsEmpty)
            return;

        var held = Player.HeldItem;
        // anything with damage is a weapon
        if (held is not null && held.damage > 0 && !Tracker.Weapons.Exists(item => item.netID == held.netID))
        {
            TGQC.Log.Debug($"Player used new weapon: {held.Name}");
            Tracker.Weapons.Add(held);
        }

        Tracker.FightTicks++;
    }

    // If you are fighting several bosses simultaneously or get hit by smth else,
    // it still counts into the fight of every boss - intended behavior.
    // The calculator is designed the way to consider all hits.
    public override void OnHurt(Player.HurtInfo info)
    {
        if (!TGQC.IsSingleplayer || Tracker.IsEmpty)
            return;

        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        // damage is not yet applied on statLife
        var hit = new PlayerHitEvent(player.Player.statLife - info.Damage, Tracker.FightTicks);
        TGQC.Log.Debug($"Player hit: {hit}");
        Tracker.Hits.Add(hit);
    }

    // Track player deaths during boss fights.
    // BUG: when Nycro's NoHit or similar mod to interfere with death hooks is enabled, the stats won't be correct.
    // For example, BossRemainingHp is 0, and fatal hit is not recorded when die-on-hit setting is enabled
    public override void UpdateDead()
    {
        if (!TGQC.IsSingleplayer || Tracker.IsEmpty)
            return;

        var npcId = Tracker.NpcId;
        try
        {
            var boss = Main.npc.First(n => n.netID == npcId && n.boss);
            _storage.Save(Tracker.CalcTrivial(boss));
            TGQC.Log.Debug($"Stored tracker of NPC id={Tracker.NpcId}: fightTicks={Tracker.FightTicks}");
            Tracker = new Tracker();
        }
        catch (Exception e) when (e is InvalidOperationException or KeyNotFoundException)
        {
            // Possible workaround: track last known boss health on every hit (fix only when it actually happens)
            // This will require either introducing new MockNPC class, or adding a couple of constructors with raw values.
            // One more option is to track the boss entry itself, so it won't be garbage-collected.
            // Due to Nycro's NoHit interference issue, the best option would be to track boss health separately.
            TGQC.Log.Error($"NPC id={npcId} has despawned, was not found or is not a boss");
        }
    }
}