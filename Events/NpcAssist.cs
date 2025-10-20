using System.Linq;
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
        {
            TGQC.Log.Info(
                $"Another boss id={player.Tracker.NpcId} is already being tracked, ignoring new boss id={npc.netID} name={npc.TypeName}");
        }
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
        var npcId = player.Tracker.NpcId;
        if (npcId != npc.netID)
        {
            var boss = Main.npc.FirstOrDefault(n => n.netID == npcId && n.boss);
            // continue if we just killed an untracked boss, stop tracking when it has bugged
            if (boss is not null && boss.netID == npc.netID) return;

            TGQC.Log.Warn(
                $"Killed boss id={npc.netID}, but tracking id={npcId}; tracking bugged, stopping and skipping");
            player.Tracker = new Tracker();
            return;
        }

        Storage.Save(player.Tracker.CalcTrivial(npc));
        TGQC.Log.Debug($"Stored tracker of NPC id={npc.netID}: fightTicks={player.Tracker.FightTicks}");
        player.Tracker = new Tracker();
    }
}