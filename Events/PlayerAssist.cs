using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaGearQualityCalculator.Storage;

namespace TerrariaGearQualityCalculator.Events;

// Reused thanks to https://github.com/JavidPack/BossChecklist
internal class PlayerAssist : ModPlayer
{
    private readonly ModelStorage _storage = State.Instance.Storage;

    // Tracker tracks stats only per one boss at the same times
    internal Tracker Tracker { get; set; } = new();

    public override void PreUpdate()
    {
        if (Tracker.IsEmpty || Main.netMode is not NetmodeID.SinglePlayer)
            return;

        var held = Player.HeldItem;
        // anything with damage is a weapon
        if (held is not null && held.damage > 0 && !Tracker.Weapons.Exists(item => item.netID == held.netID))
            Tracker.Weapons.Add(held);

        Tracker.FightTicks++;
    }

    // If you are fighting several bosses simultaneously or get hit by smth else,
    // it still counts into the fight of every boss - intended behavior.
    // The calculator is designed the way to consider all hits.
    public override void OnHurt(Player.HurtInfo info)
    {
        if (Tracker.IsEmpty)
            return;

        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        Tracker.Hits.Add(new PlayerHitEvent(player.Player.statLife, Tracker.FightTicks));
    }

    // Track player deaths during boss fights.
    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        if (Tracker.IsEmpty)
            return;

        var npcId = damageSource.SourceNPCIndex;
        NPC boss;
        try
        {
            boss = Main.npc.First(n => n.netID == npcId && n.boss);
        }
        catch (InvalidOperationException)
        {
            // FIXME possible workaround: track last known boss health on every hit (fix only when it actually happens)
            // This will require either introducing new MockNPC class, or adding a couple of constructors with raw values
            // One more option is to track the boss entry itself, so it won't be garbage-collected
            Main.NewText(
                $"[ERROR] npc id={npcId} has despawned, was not found or is not a boss",
                255, 0, 0);
            return;
        }

        _storage.Save(Tracker.CalcTrivial(boss));
        Tracker = new Tracker();
    }
}