using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TerrariaGearQualityCalculator.Storage;

namespace TerrariaGearQualityCalculator.Events;

// Reused thanks to https://github.com/JavidPack/BossChecklist
internal class NpcAssist : GlobalNPC
{
    private static readonly ModelStorage Storage = State.Instance.Storage;

    // When a boss spawns, set up the player tracker for the upcoming fight.
    public override void OnSpawn(NPC npc, IEntitySource source)
    {
        if (!npc.boss || Main.netMode is not NetmodeID.SinglePlayer)
            return;

        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        if (!player.Tracker.IsEmpty)
            Main.NewText(
                "[INFO] another boss is already being tracked, ignoring new boss",
                0, 120);
        else
            player.Tracker = new Tracker(npc.netID);
    }

    // When an NPC is killed and fully inactive the fight has ended, stop tracker and save calculation
    public override void OnKill(NPC npc)
    {
        if (!npc.boss || Main.netMode is not NetmodeID.SinglePlayer)
            return;

        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        if (player.Tracker.NpcId != npc.netID)
            return;

        Storage.Save(player.Tracker.CalcTrivial(npc));
        player.Tracker = new Tracker();
    }
}