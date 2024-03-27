using UnityEngine;
using static GameController;

public class PlayerPilot : MonoBehaviour
{
    Tank _tank;

    void Start()
    {
        _tank = GetComponent<Tank>();
    }

    void Update()
    {
        #if UNITY_EDITOR
            ProcessControls();
        #else
            ProcessControlsMobile();
        #endif
    }

    void ProcessControls()
    {
        _tank.controlsForward = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? - 1 : 0;
        _tank.controlsLeftRight = Input.GetKey(KeyCode.A) ? - .4f : Input.GetKey(KeyCode.D) ? .4f : 0;

        if (Input.GetKeyDown(KeyCode.Space))
            _tank.isShooting = true;
        else if (Input.GetKeyUp(KeyCode.Space))
            _tank.isShooting = false;
    }

    void ProcessControlsMobile()
    {
        _tank.controlsForward = GC.joystick.Vertical;
        _tank.controlsLeftRight = GC.joystick.Horizontal;
        print(GC.joystick.Vertical);
    }
}
