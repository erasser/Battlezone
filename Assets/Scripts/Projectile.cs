using static GameController;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{
    public float speed = 80;
    Vector3 _forwardVector;
    static float _maxRaycastDistance;
    RaycastHit _raycastHit;
    Vector3 _lastFixedPosition;
    public LayerMask shootableLayerMasks;
    public List<Ray> DebugRays = new ();
    bool _visualizeRaycast = false;
    Vector3 _raycastVector;

    void Start()
    {
        _forwardVector = new(0, 0, speed);
        // _maxRaycastDistance = speed * Time.fixedDeltaTime * 2;
        _lastFixedPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(_forwardVector * Time.deltaTime);
        
        

        Physics.SyncTransforms();

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
        _maxRaycastDistance = _raycastVector.magnitude;

        if (_visualizeRaycast)
            DebugRays.Add(new(_lastFixedPosition, _raycastVector));

        if (Physics.Raycast(_lastFixedPosition, _raycastVector, out _raycastHit, _maxRaycastDistance, shootableLayerMasks))
        {
            // print("hit: " + _raycastHit.collider.name);
            Destroy(gameObject);
            // _forwardVector = Vector3.zero;  // for debug
            if (!_raycastHit.collider.gameObject.CompareTag("environment"))
                _raycastHit.collider.gameObject.GetComponent<Tank>().Destroy();
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

    /*void OnTriggerEnter(Collider other)
    {
        print("hit: " + other.name + ", tag: " + other.tag);
        Destroy(gameObject);
        if (!other.CompareTag("environment"))    // TODO: Use layer
            Destroy(other.gameObject);
    }*/

    // void OnCollisionEnter(Collision other)  // ForGround, which cannot be concave & a trigger
    // {
    //     Destroy(gameObject);
    //     if (!other.collider.gameObject.CompareTag("environment"))    // TODO: Use layer
    //         Destroy(other.gameObject);
    // }
}
