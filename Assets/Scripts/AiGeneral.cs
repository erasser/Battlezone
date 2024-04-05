using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

// To be attached to game controller

public class AiGeneral : MonoBehaviour
{
    public List<SpawnLocation> spawnLocations;
    static SpawnData _spawnPassedData;
    float _transportAltitude;
    float _transportRadius;
    List<SpawnData> _data = new();

    void Start()
    {
        // aiGeneral = this;
        _transportAltitude = GC.transportPrefab.transform.position.y;
        _transportRadius = GC.groundSize / 2 + 250;

        _data.Add(new(2));
        _data.Add(new(2, 1));
        _data.Add(new(2, 2));
        _data.Add(new(2, 2));
        _data.Add(new(2, 2));
        _data.Add(new(2, 2));
        _data.Add(new(2, 2));

        SpawnSequence(_data);

        StartCoroutine(StartSpawnCycle());
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

        var dropPosition = location.GetRandomPositionInBounds(_transportAltitude);
        // var dropPosition = Vector3.zero;
        var α = Random.Range(0, 360);
        Vector3 transportEntryPosition = new(Mathf.Cos(α) * _transportRadius, _transportAltitude, Mathf.Sin(α) * _transportRadius);

        GC.transportPrefab.transform.position = transportEntryPosition;
        GC.transportPrefab.transform.LookAt(dropPosition, Vector3.up);

        var transport = Instantiate(GC.transportPrefab).GetComponent<Transport>();
        transport.distanceBeforeDrop = (dropPosition - transportEntryPosition).magnitude;

        // Instantiate(tankToBeSpawned);
    }

    IEnumerator StartSpawnCycle()
    {
        yield return new WaitForSeconds(Random.Range(20f, 40f));

        SpawnSequence(_data);
        StartCoroutine(StartSpawnCycle());
    }

}
