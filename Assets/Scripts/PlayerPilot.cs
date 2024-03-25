using UnityEngine;
using UnityEngine.Serialization;

public class PlayerPilot : MonoBehaviour
{
    Tank _tank;

    void Start()
    {
        _tank = GetComponent<Tank>();
    }

    void Update()
    {
        ProcessControls();
    }

    void ProcessControls()
    {

        _tank.controlsForward = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? - 1 : 0;
        // ControlsBackward = Input.GetKey(KeyCode.S) ? 1 : 0;
        _tank.controlsLeftRight = Input.GetKey(KeyCode.A) ? - 1 : Input.GetKey(KeyCode.D) ? 1 : 0;
        // ControlsRight = Input.GetKey(KeyCode.A) ? 1 : 0;
    }


}
