using Unity.Collections;
using UnityEngine;

/// <summary>
/// Base class for enemy behavior, including movement, health management, and death handling.
/// </summary>
public abstract class EnemyBase : MonoBehaviour
{
    //  ------------------ Public ------------------

    [ReadOnly] 
    [Tooltip("The stats that define the enemy's attributes.")]
    public EnemyStats stats;

    [Tooltip("The health component of the enemy.")]
    public HealthComponent healthComponent;

    [Header("Enemy Movement")]
    [Tooltip("Direction of movement (normalized).")]
    public Vector2 moveDirection = Vector2.left;

    [Tooltip("The particle system that spawns upon enemy death.")]
    public GameObject bitsParticleSystem;

    [Tooltip("Callback invoked when the enemy dies.")]
    public System.Action<GameObject> OnDeathCallback;

    /// <summary>
    /// Initializes the enemy's health using the stats.
    /// </summary>
    public virtual void Init() => healthComponent.InitializeHealth(stats.health);

    //  ------------------ Protected ------------------

    /// <summary>
    /// Moves the enemy in the specified direction, adjusting speed based on status effects.
    /// </summary>
    protected virtual void Move()
    {
        float speed = (healthComponent.currentStatus != DamageStatus.SLOW) ? stats.movementSpeed : (stats.movementSpeed * 0.25f);
        transform.position += (Vector3)(moveDirection.normalized * speed * Time.deltaTime);
    }

    /// <summary>
    /// Handles the death of the enemy, including spawning bits and notifying the enemy manager.
    /// </summary>
    protected virtual void OnDeath()
    {
        // Notify the enemy manager of the death
        OnDeathCallback?.Invoke(gameObject);

        // Spawn the bits particle system
        PoolManager.Instance.GetObject(bitsParticleSystem, transform.position, Quaternion.identity)
            .GetComponent<BitsController>().StartBits(stats.bitsOnKill, GameManager.Instance.mouseTrigger);

        // Return the enemy object to the pool
        PoolManager.Instance.ReturnObject(gameObject);
    }

    /// <summary>
    /// Called when the enemy reaches the end of the level, triggering a game over.
    /// </summary>
    protected virtual void OnEndReached()
    {
        EnemyManager.Instance.GameOver();
    }

    //  ------------------ Private ------------------

    /// <summary>
    /// Detects when the enemy enters the "EndTrigger" area and triggers game over.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("EndTrigger")) return;
        OnEndReached();
    }

    /// <summary>
    /// Subscribes to the death event when the object is enabled.
    /// </summary>
    private void OnEnable() =>
        healthComponent.OnDeath += OnDeath;

    /// <summary>
    /// Unsubscribes from the death event when the object is disabled.
    /// </summary>
    private void OnDisable() =>
        healthComponent.OnDeath -= OnDeath;
}
