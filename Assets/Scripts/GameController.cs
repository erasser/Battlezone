using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController GC;
    public GameObject projectilePrefab;
    // public FixedJoystick fixedJoystick;
    public FloatingJoystick floatingJoystick;
    // public UiClickDetector buttonShootUiDetector;
    [HideInInspector]
    public int shootablePlayerLayer;
    [HideInInspector]
    public int shootableEnemyLayer;
    [HideInInspector]
    public int shootableEnvironmentLayer;
    public static List<Tank> Enemies = new();
    public static Tank Player;

    void Awake()
    {
        GC = this;
        shootablePlayerLayer = LayerMask.NameToLayer("shootablePlayer");
        shootableEnemyLayer = LayerMask.NameToLayer("shootableEnemy");
        shootableEnvironmentLayer = LayerMask.NameToLayer("shootableEnvironment");
    }
}
