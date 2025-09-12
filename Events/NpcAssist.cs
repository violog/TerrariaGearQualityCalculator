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

    // When a boss spawns, set up the world and player trackers for the upcoming fight.
    public override void OnSpawn(NPC npc, IEntitySource source)
    {
        if (!npc.boss || Main.netMode is not NetmodeID.SinglePlayer)
            return;

        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        player.Trackers.Add(npc.netID, new Tracker());
    }

    // When an NPC is killed and fully inactive the fight has ended, stop tracker and save calculation
    public override void OnKill(NPC npc)
    {
        if (!npc.boss || Main.netMode is not NetmodeID.SinglePlayer)
            return;

        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        player.Trackers.Remove(npc.netID, out var tracker);

        if (tracker != null)
            Storage.Save(tracker.CalcTrivial(npc));
        else
            Main.NewText(
                $"[WARN] npc id={npc.netID} name={npc.FullName} was killed, but the respective tracker was not found",
                100, 255, 0);
    }
}