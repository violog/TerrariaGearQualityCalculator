using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TerrariaGearQualityCalculator.Storage;
using TGQC = TerrariaGearQualityCalculator.TerrariaGearQualityCalculator;

namespace TerrariaGearQualityCalculator.Events;

// Reused thanks to https://github.com/JavidPack/BossChecklist
internal class NpcAssist : GlobalNPC
{
    private static readonly ModelStorage Storage = TGQC.Storage;

    // When a boss spawns, set up the player tracker for the upcoming fight.
    public override void OnSpawn(NPC npc, IEntitySource source)
    {
        if (!TGQC.IsSingleplayer || !npc.boss)
            return;

        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        if (!player.Tracker.IsEmpty)
            TGQC.Log.Info(
                $"Another boss id={player.Tracker.NpcId} is already being tracked, ignoring new boss id={npc.netID} name={npc.TypeName}");
        else
        {
            TGQC.Log.Debug($"Started tracking of npc id={npc.netID} name={npc.TypeName}");
            player.Tracker = new Tracker(npc.netID);
        }
    }

    // When an NPC is killed and fully inactive the fight has ended, stop tracker and save calculation
    public override void OnKill(NPC npc)
    {
        if (!TGQC.IsSingleplayer || !npc.boss)
            return;

        var player = Main.LocalPlayer.GetModPlayer<PlayerAssist>();
        if (player.Tracker.NpcId != npc.netID)
        {
            TGQC.Log.Debug($"Killed NPC id={npc.netID}, but tracking NPC id={player.Tracker.NpcId}");
            return;
        }

        Storage.Save(player.Tracker.CalcTrivial(npc));
        TGQC.Log.Debug($"Stored tracker of NPC id={npc.netID}: fightTicks={player.Tracker.FightTicks}");
        player.Tracker = new Tracker();
    }
}