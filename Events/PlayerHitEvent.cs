namespace TerrariaGearQualityCalculator.Events;

public readonly struct PlayerHitEvent(int health, int timestamp)
{
    public int Health { get; } = health;
    public int Timestamp { get; } = timestamp;
}