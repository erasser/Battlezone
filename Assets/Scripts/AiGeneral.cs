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
        _transportAltitude = Gc.transportPrefab.transform.position.y;
        _transportRadius = Gc.groundSize / 2 + 250;

        _data.Add(new(1));
        _data.Add(new(1, 1));
        // _data.Add(new(1, 2));
        // _data.Add(new(2, 2));
        // _data.Add(new(2, 2));
        // _data.Add(new(2, 2));
        // _data.Add(new(3, 3));

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

        Gc.transportPrefab.transform.position = transportEntryPosition;
        Gc.transportPrefab.transform.LookAt(dropPosition, Vector3.up);

        var transport = Instantiate(Gc.transportPrefab).GetComponent<Transport>();
        transport.distanceBeforeDrop = (dropPosition - transportEntryPosition).magnitude;

        // Instantiate(tankToBeSpawned);
    }

    IEnumerator StartSpawnCycle()
    {
        yield return new WaitForSeconds(Random.Range(2000f, 4000f));

        SpawnSequence(_data);
        StartCoroutine(StartSpawnCycle());
    }

}
