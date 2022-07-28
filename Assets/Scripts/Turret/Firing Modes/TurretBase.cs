
using UnityEngine;

public abstract class TurretBase : MonoBehaviour
{
    [Header("Base Fields")]
    [SerializeField] private string turretName;
    [SerializeField] protected GameObject projectilePrefab;

    public string TurretName { get => turretName; }

    protected TurretFiringController turretFiringController;

    public virtual void StopTurret() {}

    protected virtual void Fire() { }
    protected virtual void FireProjectile(Transform firingPoint, float damagePerShot, float projectileForce)
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firingPoint.position,
            firingPoint.rotation);

        Projectile projectileInfo = projectile.GetComponent<Projectile>();
        projectileInfo.Damage = damagePerShot;
        projectileInfo.Owner = turretFiringController.PlayerInput.playerIndex;

        projectile.GetComponent<Rigidbody>()
            .AddRelativeForce(Vector3.forward * projectileForce, ForceMode.Impulse);
    }
}
