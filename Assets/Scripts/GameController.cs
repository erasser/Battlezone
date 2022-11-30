using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject vehicle1;
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

    }

    void FixedUpdate()
    {

    }


}
