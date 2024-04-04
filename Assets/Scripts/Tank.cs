using UnityEngine;
using static GameController;

// No Physics

public class Tank : MonoBehaviour
{
    public float speed;
    [HideInInspector]
    public float controlsForward;
    [HideInInspector]
    public float controlsLeftRight;
    [HideInInspector]
    public bool isShooting;
    public float shootDelay = .8f;
    float _lastShotAt;
    public Transform shootSource;
    Vector3 _forwardVector;
    RaycastHit _raycastHitPredictCollision;
    BoxCollider _boxCollider;
    Vector3 _halvesExtents;
    [HideInInspector]
    public LayerMask shootableLayerMasks;  // All layers that this tank shoots
    AiPilot _aiPilot;
    bool _isPlayer;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _halvesExtents = _boxCollider.size / 2;
        _aiPilot = GetComponent<AiPilot>();

        if (CompareTag("Player"))
        {
            shootableLayerMasks = 1 << GC.shootableEnvironmentLayer | 1 << GC.shootableEnemyLayer;
            _isPlayer = true;
        }
        else  // enemy
            shootableLayerMasks = 1 << GC.shootableEnvironmentLayer | 1 << GC.shootablePlayerLayer;
    }

    void Update()
    {
        if (_isPlayer || !_isPlayer && _aiPilot.state == AiPilot.State.GoingToTarget)   
            ProcessTransform();
    }

    void ProcessTransform()
    {
        AutoShoot();

        _forwardVector = speed * controlsForward * Time.deltaTime * transform.forward;

        // TODO: Pokud to budou collidery a ne triggery, tohle nebude zapotřebí
        if (CompareTag("Enemy") || !Physics.BoxCast(transform.position, _halvesExtents, _forwardVector, transform.rotation, _forwardVector.magnitude))
            transform.Translate(_forwardVector, Space.World);

        transform.RotateAround(Vector3.up, 4 * controlsLeftRight * Time.deltaTime);
    }

    void AutoShoot()
    {
        if (!isShooting || _lastShotAt + shootDelay > Time.time)
            return;

        var projectile = Instantiate(GC.projectilePrefab, shootSource.position, transform.rotation).GetComponent<Projectile>();
        projectile.shootableLayerMasks = shootableLayerMasks;
        _lastShotAt = Time.time;
    }
}
