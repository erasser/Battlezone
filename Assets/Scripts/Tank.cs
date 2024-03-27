using UnityEngine;
using static GameController;

// No Physics

public class Tank : MonoBehaviour
{
    [HideInInspector]
    public float controlsForward;
    [HideInInspector]
    public float controlsLeftRight;
    public bool isShooting;
    public float shootDelay = .8f;
    float _lastShotAt;
    public Transform shootSource;

    void Update()
    {
        AutoShoot();

        transform.Translate( 6 * controlsForward * Time.deltaTime * transform.forward, Space.World);
        // print(controlsForward);

        transform.RotateAround(Vector3.up, 4 * controlsLeftRight * Time.deltaTime);
    }

    void AutoShoot()
    {
        if (!isShooting || _lastShotAt + shootDelay > Time.time)
            return;

        Instantiate(GC.projectilePrefab, shootSource.position, transform.rotation);
        _lastShotAt = Time.time;
    }
}
