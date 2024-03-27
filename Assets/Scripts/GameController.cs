using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController GC;
    public GameObject projectilePrefab;
    public FixedJoystick joystick;
    // public UiClickDetector buttonShootUiDetector;

    void Start()
    {
        GC = this;
    }
}
