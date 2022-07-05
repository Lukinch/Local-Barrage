using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPlayerTurretFire : MonoBehaviour
{
    [SerializeField] private InputReaderSO inputReader;

    [Header("Projectile Dependencies")]
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileForce;
    [SerializeField] private float projectileDamage;

    private void OnEnable()
    {
        inputReader.FireInstantEvent += FireProjectile;
    }
    private void OnDisable()
    {
        inputReader.FireInstantEvent -= FireProjectile;
    }

    private void FireProjectile()
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firingPoint.position,
            firingPoint.rotation);

        Projectile projectileInfo = projectile.GetComponent<Projectile>();
        projectileInfo.damage = projectileDamage;
        projectileInfo.projectileOwner = transform;

        projectile.GetComponent<Rigidbody>()
            .AddRelativeForce(Vector3.forward * projectileForce, ForceMode.Impulse);
    }
}
