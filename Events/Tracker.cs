using System;
using System.Linq;
using Terraria;
using TerrariaGearQualityCalculator.Calculators.Trivial;

namespace TerrariaGearQualityCalculator.Events;

public class Tracker(int npcId)
{
    protected internal int PlayerTicks { get; set; }
    private const decimal TicksPerSecond = 60;

    internal TrivialCalculation CalcTrivial
    {
        get
        {
            var playerHp = Main.LocalPlayer.statLife;
            if (playerHp > 0)
                throw new NotImplementedException("don't win fights yet!");
            
            NPC boss;
            try
            {
                // what if it died or despawned?
                boss = Main.npc.First(n => n.netID == npcId && n.boss);
            }
            catch (InvalidOperationException)
            {
                Main.NewText(
                    $"[ERROR] npc id={npcId} was not found or is not a boss",
                    255, 0, 0);
                return null;
            }

            return new TrivialCalculation(boss, PlayerTicks/TicksPerSecond);
        }
    }
}