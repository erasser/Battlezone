using UnityEngine;

public class HoverRay : MonoBehaviour
{
    public Rigidbody vehicleRb;

    [Tooltip("Rozmezí houpání, tj. velikost výkyvu.")]
    public float length = 20;       //    1
    [Tooltip("Vyšší hmotnost vyžaduje vyšší sílu. Síla působí směrem nahoru.")]
    public float strength = 60;     // 6000
    [Tooltip("Tvrdost tlumiče.")]
    public float dampening = 100;   // 6000
    readonly Vector3 _vector3Down = - Vector3.up;
    Rigidbody _rb;
    float _lastHitDist;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        vehicleRb = transform.parent.gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // hover
        if (Physics.Raycast(_rb.transform.position, _rb.transform.TransformDirection(_vector3Down), out var hit, length))
        {
            float forceAmount = HooksLawDampen(hit.distance);  // TODO: Fuse these two lines

            // _rb.AddForce(_rb.transform.up * forceAmount);
            vehicleRb.AddForceAtPosition(_rb.transform.up * forceAmount, transform.position);
        }
        else
            _lastHitDist = 1.1f * length;

        // forward movement
        
        // _rb.AddForce(_rb.transform.right * _forwardPower, ForceMode.Impulse);
        // _rb.AddForce(_rb.transform.forward * _forwardPower, ForceMode.Impulse);
        // _rb.AddForce(_rb.transform.right * _sidePower * 10000, ForceMode.Impulse);

        // _rb.AddForce(_rb.transform.right * 80, ForceMode.Impulse);
    }
    
    float HooksLawDampen(float hitDistance)
    {
        float forceAmount = strength * (length - hitDistance) + dampening * (_lastHitDist - hitDistance);
        forceAmount = Mathf.Max(0, forceAmount);
        _lastHitDist = hitDistance;

        return forceAmount;
    }
}
