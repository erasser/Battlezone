using System.Drawing;
using UnityEngine;

public class WobbleEffect : MonoBehaviour
{
    [Tooltip("Relative increment.\ne.g. .5f => 1.5×")]
    public float maxIncrement = .4f;
    public float speed = 10;
    Mesh _mesh;
    float _initialYPosition;
    float _initialDeltaYPosition;
    float _ySize;
    BoxCollider _boxCollider;
    float _randomPhaseOffset;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _ySize = _boxCollider.bounds.extents.y;
        _initialYPosition = transform.position.y;
        // _initialDeltaYPosition = _initialYPosition -;  // TODO
        _randomPhaseOffset = Random.Range(0, 360);
    }

    void Update()
    {
        var tr = transform;

        var xz = Mathf.Sin(Time.time * speed + _randomPhaseOffset) * maxIncrement + 1;
        var y = 2 - xz;
        // var z = Mathf.Sin(Time.time * speed) * maxIncrement + 1;

        tr.localScale = new(xz, y, xz);

        // var yIncrement = _boxCollider.bounds.; 
        // var Y = _initialYPosition + yIncrement;
        // var position = tr.position;
        // position = new(position.x, Y, position.z);
        // tr.position = position;
    }
}
