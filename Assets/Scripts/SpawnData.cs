using JetBrains.Annotations;

public class SpawnData
{
    public int TanksCount;
    public float DelayBeforeSpawn;
    [CanBeNull]
    public SpawnLocation SpawnLocation;

    public SpawnData(int tanksCount, float delayBeforeSpawn = 0, SpawnLocation spawnLocation = null)
    {
        TanksCount = tanksCount;
        DelayBeforeSpawn = delayBeforeSpawn;
        SpawnLocation = spawnLocation;
    }

    public override string ToString()
    {
        return "TanksCount: " + TanksCount + ", DelayBeforeSpawn: " + DelayBeforeSpawn;
    }
}
