
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
        projectileInfo.Damage = damagePerShot;
        projectileInfo.Owner = turretFiringController.Owner.playerIndex;

        projectile.GetComponent<Rigidbody>()
            .AddRelativeForce(Vector3.forward * projectileForce, ForceMode.Impulse);
    }
}
