using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject vehicle1;
    float _forwardPower;
    float _sidePower;
    public static GameObject actualVehicle;

    void Start()
    {

        // Time.timeScale = .2f;
        
        #if UNITY_EDITOR
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));  // Don't auto-switch to the Game tab
        #endif

        actualVehicle = vehicle1;
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

        // _rb.AddForce(_rb.transform.right * _forwardPower, ForceMode.Impulse);
        // _rb.AddForce(_rb.transform.forward * _forwardPower, ForceMode.Impulse);
        // _rb.AddForce(_rb.transform.right * _sidePower * 10000, ForceMode.Impulse);

        // _rb.AddForce(_rb.transform.right * 80, ForceMode.Impulse);
    }


}
