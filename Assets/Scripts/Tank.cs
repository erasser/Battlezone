using UnityEngine;
using static GameController;
using static SoundManager;

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
    public bool isPlayer;
    float _health = 100;

    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _halvesExtents = _boxCollider.size / 2;
        _aiPilot = GetComponent<AiPilot>();

        if (CompareTag("Player"))
        {
            shootableLayerMasks = 1 << Gc.shootableEnvironmentLayer | 1 << Gc.shootableEnemyLayer;
            isPlayer = true;
        }
        else  // enemy
            shootableLayerMasks = 1 << Gc.shootableEnvironmentLayer | 1 << Gc.shootablePlayerLayer | 1 << Gc.shootableEnemyLayer;
    }

    void Update()
    {
        if (isPlayer || !isPlayer && _aiPilot.state == AiPilot.State.GoingToTarget)   
            ProcessTransform();

        if (!isPlayer)
            CheckAndAttackPlayer();

        if (isPlayer && resetDamageEffectAt != Mathf.Infinity && Time.time >= resetDamageEffectAt)
        {
            Gc.ToggleDamageEffect(false);
            resetDamageEffectAt = Mathf.Infinity;
        }
    }

    void ProcessTransform()
    {
        AutoShoot();

        _forwardVector = speed * controlsForward * Time.deltaTime * transform.forward;

        // TODO: Pokud to budou collidery a ne triggery, tohle nebude zapotřebí
        if (CompareTag("Enemy") || !Physics.BoxCast(transform.position, _halvesExtents, _forwardVector, transform.rotation, _forwardVector.magnitude, 1 << Gc.shootableEnvironmentLayer))
            transform.Translate(_forwardVector, Space.World);
    }

    void AutoShoot()
    {
        if (!isShooting || _lastShotAt + shootDelay > Time.time)
            return;

        var projectile = Instantiate(Gc.projectilePrefab, shootSource.position, transform.rotation).GetComponent<Projectile>();
        projectile.shootableLayerMasks = shootableLayerMasks;
        projectile.wasShotByPlayer = isPlayer;
        _lastShotAt = Time.time;
    }

    public void TakeDamage(Vector3 shotInitialPosition)
    {
        if (isPlayer)
        {
            _health -= 1;
            Gc.textHealth.text = "health: " + _health;
            Gc.ToggleDamageEffect(true, shotInitialPosition);

            if (_health == 0)
            {
                Gc.textHealth.text = "DEAD!";
                Gc.textHealth.color = Color.red;
                Time.timeScale = 0;
            }
            return;
        }

        for (int i = 0; i < 8; ++i)
            Instantiate(Gc.fragmentsPrefab, transform.position, Quaternion.identity);

        Enemies.Remove(this);
        ++Gc.killCount;
        Gc.textKills.text = "kills: " + Gc.killCount;
        Sm.PlayClip(Sm.tankDestroyEffect, gameObject);
        Destroy(gameObject);
    }

    void CheckAndAttackPlayer()
    {
        var toPlayer = Player.transform.position - transform.position;
        var angle = Vector3.Angle(toPlayer, transform.forward);

        isShooting = angle < 5;
    }
}
