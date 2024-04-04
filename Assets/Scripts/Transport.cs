using UnityEngine;
using static GameController;

public class Transport : MonoBehaviour
{
    public float distanceBeforeDrop;
    public float speed = 25;
    float _distanceTravelled;
    bool _cargoDropped;

    void Update()
    {
        var distance = speed * Time.deltaTime;
        transform.Translate(distance * transform.forward, Space.World);
        _distanceTravelled += distance;

        if (_cargoDropped && _distanceTravelled > GC.GroundSize * 2)  // TODO
            Destroy(gameObject);

        if (!_cargoDropped && _distanceTravelled > distanceBeforeDrop)
            DropCargo();
    }

    void DropCargo()
    {
        var tr = transform;
        Instantiate(GC.tankPrefab, tr.position, tr.rotation);
        _cargoDropped = true;
        speed *= 2;
    }
}
