using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public Rigidbody rb;

    float _forwardPower;
    float _sidePower;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Time.timeScale = .3f;
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

}
