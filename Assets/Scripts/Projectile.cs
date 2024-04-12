using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{
    public float speed = 80;
    public float rotationSpeed = 5;
    Vector3 _forwardVector;
    static float _maxRaycastDistance;
    RaycastHit _raycastHit;
    Vector3 _lastFixedPosition;
    public LayerMask shootableLayerMasks;
    public List<Ray> DebugRays = new ();
    bool _visualizeRaycast = false;
    Vector3 _raycastVector;
    Vector3 _permanentRotationAxis;
    public bool wasShotByPlayer;

    void Start()
    {
        // _forwardVector = new(0, 0, speed); 
        _forwardVector = transform.forward * speed; 
        _lastFixedPosition = transform.position;
        _permanentRotationAxis = new(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }

    void Update()
    {
        var tr = transform;

        transform.Translate(_forwardVector * Time.deltaTime, Space.World);

        transform.Rotate(_permanentRotationAxis, Time.deltaTime * rotationSpeed);

        // Physics.SyncTransforms();

        if (_visualizeRaycast)
            foreach (Ray ray in DebugRays)
                Debug.DrawRay(ray.origin, ray.direction, Random.ColorHSV());

        if (_visualizeRaycast)
        {
            var t = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            t.transform.localScale = new(.06f, .06f, .06f);
            t.transform.position = transform.position;
        }

        _raycastVector = transform.position - _lastFixedPosition;
        _maxRaycastDistance = _raycastVector.magnitude * 2;

        if (_visualizeRaycast)
            DebugRays.Add(new(_lastFixedPosition, _raycastVector));

        if (Physics.Raycast(_lastFixedPosition, _raycastVector, out _raycastHit, _maxRaycastDistance, shootableLayerMasks))
        {
            Destroy(gameObject);

            Tank tank = _raycastHit.collider.gameObject.GetComponent<Tank>();

            if (!tank)
                return;

            if (wasShotByPlayer && !tank.isPlayer || !wasShotByPlayer && tank.isPlayer)
                tank.TakeDamage();
        }

        _lastFixedPosition = transform.position;
    }

    /*void FixedUpdate()
    {
        if (_visualizeRaycast)
        {
            var t = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            t.transform.localScale = new(.06f, .06f, .06f);
            t.transform.position = transform.position;
        }

        _raycastVector = transform.position - _lastFixedPosition;
        _maxRaycastDistance = _raycastVector.magnitude;

        if (_visualizeRaycast)
            DebugRays.Add(new(_lastFixedPosition, _raycastVector));

        if (Physics.Raycast(_lastFixedPosition, _raycastVector, out _raycastHit, _maxRaycastDistance, shootableLayerMasks))
        {
            print("hit: " + _raycastHit.collider.name);
            Destroy(gameObject);
            if (!_raycastHit.collider.gameObject.CompareTag("environment"))
                Destroy(_raycastHit.collider.gameObject);

        }

        _lastFixedPosition = transform.position;
    }*/
}
