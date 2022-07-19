
using UnityEngine;

public abstract class TurretBase : MonoBehaviour
{
    [Header("Base Fields")]
    [SerializeField] protected GameObject projectilePrefab;

    protected TurretFiringController turretFiringController;

    protected virtual void Fire() { }
    protected virtual void FireProjectile(Transform firingPoint, float damagePerShot, float projectileForce)
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firingPoint.position,
            firingPoint.rotation);

        Projectile projectileInfo = projectile.GetComponent<Projectile>();
        projectileInfo.damage = damagePerShot;
        projectileInfo.owner = turretFiringController.owner;

        projectile.GetComponent<Rigidbody>()
            .AddRelativeForce(Vector3.forward * projectileForce, ForceMode.Impulse);
    }
}
