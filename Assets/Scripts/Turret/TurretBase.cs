using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurretBase : MonoBehaviour
{
    [Header("Base Fields")]
    [SerializeField] protected InputReaderSO inputReader;
    [SerializeField] protected SO_TurretStats turretStats;
    [SerializeField] protected GameObject projectilePrefab;

    protected virtual void Fire() { }
    protected virtual void FireProjectile(Transform firingPoint)
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firingPoint.position,
            firingPoint.rotation);

        Projectile projectileInfo = projectile.GetComponent<Projectile>();
        projectileInfo.damage = turretStats.damagePerShot;
        projectileInfo.projectileOwner = transform;

        projectile.GetComponent<Rigidbody>()
            .AddRelativeForce(Vector3.forward * turretStats.projectileForce, ForceMode.Impulse);
    }
}
