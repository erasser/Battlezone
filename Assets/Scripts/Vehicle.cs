using UnityEngine;

public class Vehicle : MonoBehaviour
{
    readonly Vector3 _vector3Down = - Vector3.up;
    Rigidbody _rb;
    [Tooltip("Rozmezí houpání, tj. velikost výkyvu.")]
    public float length = 20;       //    1
    [Tooltip("Vyšší hmotnost vyžaduje vyšší sílu. Síla působí směrem nahoru.")]
    public float strength = 60;     // 6000
    [Tooltip("Tvrdost tlumiče.")]
    public float dampening = 100;   // 6000
    float _lastHitDist;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // gravity
        if (Physics.Raycast(_rb.transform.position, _rb.transform.TransformDirection(_vector3Down), out var hit, length))
        {
            float forceAmount = HooksLawDampen(hit.distance);

            // _rb.AddForceAtPosition(_rb.transform.up * forceAmount, _rb.transform.position);
            _rb.AddForce(_rb.transform.up * forceAmount);
        }
        else
            _lastHitDist = 1.1f * length;
    }
    
    float HooksLawDampen(float hitDistance)
    {
        float forceAmount = strength * (length - hitDistance) + dampening * (_lastHitDist - hitDistance);
        forceAmount = Mathf.Max(0, forceAmount);
        _lastHitDist = hitDistance;

        return forceAmount;
    }
}
