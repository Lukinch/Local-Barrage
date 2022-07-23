using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerHullCollision unitCollision;
    [SerializeField] private float maxHealth;

    private float currentHealth;

    public event Action<float> OnTakeDamageEvent; 

    private void OnEnable()
    {
        unitCollision.HullCollisionEvent += GetProjectileInfo;
    }

    private void OnDisable()
    {
        unitCollision.HullCollisionEvent -= GetProjectileInfo;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void GetProjectileInfo(float projectileReceived)
    {
        TakeDamage(projectileReceived);
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
