using Unity.Mathematics;

/// <summary>
/// Generates bits at regular intervals by firing a projectile.
/// </summary>
public class BitGenerator : BuildableUnit
{

    /// <summary>
    /// Checks the time elapsed and triggers bit generation if the recharge time has passed.
    /// </summary>
    public override void Check()
    {
        timePassed += 0.1f;
        if (timePassed > stats.rechargeTime)
        {
            timePassed = 0;
            Fire();
            buildableAnimator.Play("GeneratedBits");
        }
    }
    
    //  ------------------ Private ------------------

    private float timePassed = 0;

    /// <summary>
    /// Initializes the BitGenerator when built.
    /// </summary>
    private void Start()
    {
        OnBuild();
    }

    /// <summary>
    /// Fires a projectile that generates bits and applies the defined damage.
    /// </summary>
    public override void Fire()
    {
        PoolManager.Instance.GetObject(stats.projectile, transform.position, quaternion.identity)
            .GetComponent<BitsController>().StartBits(stats.damage, GameManager.Instance.mouseTrigger);
    }
}
