using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBase : MonoBehaviour
{
    [SerializeField] protected SO_TurretStats stats;

    public void Fire() { }

    //private void FireProjectile()
    //{
    //    GameObject projectile = Instantiate(
    //        projectilePrefab,
    //        firingPoint.position,
    //        firingPoint.rotation);

    //    Projectile projectileInfo = projectile.GetComponent<Projectile>();
    //    projectileInfo.damage = projectileDamage;
    //    projectileInfo.projectileOwner = transform;

    //    projectile.GetComponent<Rigidbody>()
    //        .AddRelativeForce(Vector3.forward * projectileForce, ForceMode.Impulse);
    //}
}
