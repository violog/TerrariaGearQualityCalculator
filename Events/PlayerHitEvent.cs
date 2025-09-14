namespace TerrariaGearQualityCalculator.Events;

public readonly struct PlayerHitEvent(int health, int timestamp)
{
    public int Health { get; } = health;
    public int Timestamp { get; } = timestamp;

    public override string ToString()
    {
        return $"Health={Health} Timestamp={Timestamp}";
    }
}