using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController GC;
    public AiPilot tankPrefab;
    public Projectile projectilePrefab;
    public Transport transportPrefab;
    public Fragments fragmentsPrefab;
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
    public float groundSize;
    public float initialTankYPosition;
    [HideInInspector]
    public int killCount;
    public Text textKills; 
    public Text textHealth; 

    void Awake()
    {
        GC = this;
        shootablePlayerLayer = LayerMask.NameToLayer("shootablePlayer");
        shootableEnemyLayer = LayerMask.NameToLayer("shootableEnemy");
        shootableEnvironmentLayer = LayerMask.NameToLayer("shootableEnvironment");
        textKills = GameObject.Find("text kills").GetComponent<Text>();
        textHealth = GameObject.Find("text health").GetComponent<Text>();
        textKills.text = "kills: 0";
        GC.textHealth.text = "health: 100";

        groundSize = GameObject.Find("ground").GetComponent<Renderer>().bounds.size.x;
        initialTankYPosition = tankPrefab.transform.position.y;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ToggleSlowMotion()
    {
        Time.timeScale = Time.timeScale == 1f ? .1f : 1;
    }
}
