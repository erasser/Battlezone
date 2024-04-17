using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GameController : MonoBehaviour
{
    public static GameController Gc;
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
    [HideInInspector]
    public Text textKills;
    [HideInInspector]
    public Text textHealth;
    public Camera camera1;
    public Camera camera2;
    public Image crosshair1;
    public Image crosshair2;
    [HideInInspector]
    public ChromaticAberration chromaticAberration;
    [HideInInspector]
    public ColorGrading colorGrading;
    public List<GameObject> damageUiElements;
    // public Transform portal1;
    // public Transform portal2;
    public static float resetDamageEffectAt;
    public Image damageDirectionIndicatorPrefab;
    public GameObject ui;

    void Awake()
    {
        Gc = this;
        shootablePlayerLayer = LayerMask.NameToLayer("shootablePlayer");
        shootableEnemyLayer = LayerMask.NameToLayer("shootableEnemy");
        shootableEnvironmentLayer = LayerMask.NameToLayer("shootableEnvironment");
        textKills = GameObject.Find("text kills").GetComponent<Text>();
        textHealth = GameObject.Find("text health").GetComponent<Text>();
        textKills.text = "kills: 0";
        Gc.textHealth.text = "health: 100";
        camera2.gameObject.SetActive(false);
        crosshair2.gameObject.SetActive(false);
        groundSize = GameObject.Find("ground").GetComponent<Renderer>().bounds.size.x;
        initialTankYPosition = tankPrefab.transform.position.y;
        var ppVolume = GetComponent<PostProcessVolume>();
        chromaticAberration = ppVolume.profile.GetSetting<ChromaticAberration>();
        colorGrading = ppVolume.profile.GetSetting<ColorGrading>();
        ui = GameObject.Find("UI");
    }

    public void RestartLevel()
    {
        BlurAfterTouch();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ToggleSlowMotion()
    {
        BlurAfterTouch();
        Time.timeScale = Time.timeScale == 1f ? .15f : 1;
    }

    public void ToggleCamera()
    {
        BlurAfterTouch();
        var b = camera1.gameObject.activeSelf;
        camera1.gameObject.SetActive(!b);
        crosshair1.gameObject.SetActive(!b);
        camera2.gameObject.SetActive(b);
        crosshair2.gameObject.SetActive(b);

    }

    void BlurAfterTouch()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ToggleDamageEffect(bool enable)
    {
        foreach (GameObject element in damageUiElements)
            element.SetActive(enable);

        resetDamageEffectAt = Time.time + .3f;  // Ale ťuťu, zde není použitý enumerator
        chromaticAberration.intensity.value = enable ? 1 : 0;
        colorGrading.saturation.value = enable ? -50 : 0;
        colorGrading.contrast.value = enable ? 100 : 0;
        // Gc.chromaticAberration.enabled.value = enable;
    }
    
    public void ToggleDamageEffect(bool enable, Vector3 shotInitialPosition)
    {
        ToggleDamageEffect(enable);

        var newIndicator = Instantiate(Gc.damageDirectionIndicatorPrefab, Gc.ui.transform).GetComponent<DamageDirectionIndicator>();
        newIndicator.shotInitialPosition = shotInitialPosition;
        
    }

}
