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
    float _forwardPower;
    float _sidePower;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _forwardPower += .04f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _forwardPower = 0;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _sidePower = -.8f;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _sidePower = .8f;
        }
        else
            _sidePower = 0;
    }

    void FixedUpdate()
    {
        // hover
        if (Physics.Raycast(_rb.transform.position, _rb.transform.TransformDirection(_vector3Down), out var hit, length))
        {
            float forceAmount = HooksLawDampen(hit.distance);

            _rb.AddForce(_rb.transform.up * forceAmount);
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
