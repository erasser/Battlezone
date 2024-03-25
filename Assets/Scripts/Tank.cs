using UnityEngine;

// No Physics

public class Tank : MonoBehaviour
{
    [HideInInspector]
    public float controlsForward;
    [HideInInspector]
    public float controlsLeftRight;

    void Update()
    {
        transform.Translate( 6 * controlsForward * Time.deltaTime * transform.forward, Space.World);

        transform.RotateAround(Vector3.up, 5 * controlsLeftRight * Time.deltaTime);
    }
}
