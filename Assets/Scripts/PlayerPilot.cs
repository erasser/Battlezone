using UnityEngine;
using static GameController;
using static Portal;

[RequireComponent(typeof(Tank))]

public class PlayerPilot : MonoBehaviour
{
    Tank _tank;

    void Awake()
    {
        _tank = GetComponent<Tank>();
        Player = _tank;
    }

    void Update()
    {
        #if UNITY_EDITOR
            ProcessControls();
        #else
            ProcessControlsMobile();
        #endif
        
        Rotate();

        // CheckPortals();
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

    void Rotate()
    {
        transform.RotateAround(Vector3.up, 4 * _tank.controlsLeftRight * Time.deltaTime);
    }

    void ProcessControlsMobile()
    {
        _tank.controlsForward = Gc.floatingJoystick.Vertical;
        _tank.controlsLeftRight = Gc.floatingJoystick.Horizontal * .3f;
    }

    void CheckPortals()
    {
        var enableFirst = (Portals[0].transform.position - transform.position).sqrMagnitude < (Portals[1].transform.position - transform.position).sqrMagnitude;

        Portals[0].gameObject.SetActive(enableFirst);
        Portals[1].gameObject.SetActive(!enableFirst);
    }

}
