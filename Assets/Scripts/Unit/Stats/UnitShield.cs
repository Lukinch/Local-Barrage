using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShield : MonoBehaviour
{
    [SerializeField] private UnitShieldCollision shieldCollision;
    [SerializeField] private float maxShield;

    private float currentShield;

    public event Action<float> OnTakeDamageEvent; 

    private void OnEnable()
    {
        shieldCollision.ShieldCollisionEvent += GetProjectileInfo;
    }

    private void OnDisable()
    {
        shieldCollision.ShieldCollisionEvent -= GetProjectileInfo;
    }

    private void Start()
    {
        currentShield = maxShield;
    }

    private void GetProjectileInfo(Projectile projectileReceived)
    {
        TakeDamage(projectileReceived.damage);
    }

    private void TakeDamage(float damageAmount)
    {
        currentShield -= damageAmount;
        if (currentShield <= 0)
        {
            currentShield = 0;
            DisableShield();
        }
        
        OnTakeDamageEvent?.Invoke(currentShield / maxShield);
    }

    private void EnableShield()
    {
        shieldCollision.gameObject.SetActive(true);
    }

    private void EnableShield(float amountToRecover)
    {
        if (amountToRecover > 0)
        {
            currentShield += amountToRecover;
            shieldCollision.gameObject.SetActive(true);
        }
    }

    private void DisableShield()
    {
        shieldCollision.gameObject.SetActive(false);
    }
}
