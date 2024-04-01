using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class SpawnLocation : MonoBehaviour
{
    Vector2 _minBounds;
    Vector2 _maxBounds;

    void Awake()
    {
        var bounds = GetComponent<BoxCollider>().bounds;
        _minBounds = new(bounds.min.x, bounds.min.z);
        _maxBounds = new(bounds.max.x, bounds.max.z);
    }

    public Vector3 GetRandomPositionInBounds(float y = 0)
    {
        return new(Random.Range(_minBounds.x, _maxBounds.x), y, Random.Range(_minBounds.y, _maxBounds.y));
    }
}
