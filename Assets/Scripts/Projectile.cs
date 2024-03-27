using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 80;
    Vector3 _forwardVector;
    static float _maxRaycastDistance;
    RaycastHit _raycastHit;

    void Start()
    {
        _forwardVector = new(0, 0, speed);
        _maxRaycastDistance = speed * Time.fixedDeltaTime;
    }

    void Update()
    {
        transform.Translate(_forwardVector * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, _forwardVector, out _raycastHit, _maxRaycastDistance))
        {
            print("hit!");
            Destroy(gameObject);
            Destroy(_raycastHit.collider.gameObject);
        }
    }
}
