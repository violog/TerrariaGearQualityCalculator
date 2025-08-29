using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TerrariaGearQualityCalculator.Storage;

namespace TerrariaGearQualityCalculator.Events;

// Reused thanks to https://github.com/JavidPack/BossChecklist
internal class NpcAssist : GlobalNPC
{
    private readonly ModelStorage _storage = State.Instance.Storage;

    // When a boss spawns, set up the world and player trackers for the upcoming fight.
    public override void OnSpawn(NPC npc, IEntitySource source)
    {
        if (Main.netMode is not NetmodeID.SinglePlayer)
            return;
        if (!npc.boss)
            return;

        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        player.Trackers.Add(npc.netID, new Tracker(npc.netID));
    }

    // When an NPC is killed and fully inactive the fight has ended, stop tracker and save calculation
    public override void OnKill(NPC npc)
    {
        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        player.Trackers.Remove(npc.netID, out var tracker);

        if (tracker != null)
            _storage.Save(tracker.CalcTrivial);
        else
            Main.NewText(
                $"[WARN] npc id={npc.netID} name={npc.FullName} was killed, but the respective tracker was not found",
                100, 255, 0);
    }
}