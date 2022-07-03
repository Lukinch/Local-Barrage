using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] private UnitCollision unitCollision;
    [SerializeField] private float maxHealth;

    private float currentHealth;

    public event Action<float> OnTakeDamageEvent; 

    private void OnEnable()
    {
        unitCollision.ProjectileCollisionEvent += GetProjectileInfo;
    }

    private void OnDisable()
    {
        unitCollision.ProjectileCollisionEvent -= GetProjectileInfo;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void GetProjectileInfo(Projectile projectileReceived)
    {
        TakeDamage(projectileReceived.damage);
    }

    private void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DestroySelf();
        }
        
        OnTakeDamageEvent?.Invoke(currentHealth / maxHealth);
    }

    private void DestroySelf() => Destroy(gameObject);
}
