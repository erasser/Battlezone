using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To be attached to game controller

public class AiGeneral : MonoBehaviour
{
    // public static AiGeneral aiGeneral;
    public AiPilot tankToBeSpawned;
    public List<SpawnLocation> spawnLocations;
    static SpawnData _spawnPassedData;

    void Start()
    {
        // aiGeneral = this;

        List<SpawnData> data = new();
        data.Add(new(1));
        data.Add(new(1, 1));
        data.Add(new(1, 1));
        data.Add(new(1, 1));
        data.Add(new(1, 1));
        data.Add(new(1, 1));
        data.Add(new(1, 1));

        SpawnSequence(data);
    }

    public void SpawnSequence(List<SpawnData> spawnSequence)
    {
        float cumulativeDelay = 0;
        foreach (var spawnData in spawnSequence)  // Invoke all spawn data at once
        {
            cumulativeDelay += spawnData.DelayBeforeSpawn;
            spawnData.DelayBeforeSpawn = cumulativeDelay;   // Convert relative delays to absolute delays

            StartCoroutine(SpawnEnemies(spawnData));
        }
    }

    public IEnumerator SpawnEnemies(SpawnData spawnData)
    {
        yield return new WaitForSeconds(spawnData.DelayBeforeSpawn);

        for (int i = 0; i < spawnData.TanksCount; ++i)
            SpawnEnemy(spawnData);
    }

    void SpawnEnemy(SpawnData spawnData)
    {
        SpawnLocation location = spawnData.SpawnLocation ? spawnData.SpawnLocation : spawnLocations[Random.Range(0, spawnLocations.Count)];
        tankToBeSpawned.transform.position = location.GetRandomPositionInBounds(.69f);

        Instantiate(tankToBeSpawned);
    }

}
