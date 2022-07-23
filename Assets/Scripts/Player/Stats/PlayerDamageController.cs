using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageController : MonoBehaviour
{
    [SerializeField] private PlayerHullCollision hullCollision;
    [SerializeField] private PlayerShieldCollision shieldCollision;

    [SerializeField] private float maxHealth;
    [SerializeField] private float maxShield;

    private float currentHealth;
    private float currentShield;

    public event Action<float> OnHullHealthChangedEvent;
    public event Action<float> OnShieldHealthChangedEvent;

    private void OnEnable()
    {
        hullCollision.HullCollisionEvent += OnHullDamaged;
        shieldCollision.ShieldCollisionEvent += OnShieldDamage;
    }

    private void OnDisable()
    {
        hullCollision.HullCollisionEvent -= OnHullDamaged;
        shieldCollision.ShieldCollisionEvent -= OnShieldDamage;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;

        hullCollision.gameObject.SetActive(false);
    }

    private void OnHullDamaged(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DisableSelf();
        }

        OnHullHealthChangedEvent?.Invoke(currentHealth / maxHealth);
    }

    private void OnShieldDamage(float damage)
    {
        currentShield -= damage;
        if (currentShield <= 0)
        {
            currentShield = 0;
            DisableShield();
            EnableHull();
        }
        
        OnShieldHealthChangedEvent?.Invoke(currentShield / maxShield);
    }

    private void DisableSelf()
    {
        currentHealth = maxHealth;
        OnHullHealthChangedEvent?.Invoke(currentHealth / maxHealth);

        currentShield = maxShield;
        OnShieldHealthChangedEvent?.Invoke(currentShield / maxShield);

        gameObject.SetActive(false);
    }

    private void EnableHull() => hullCollision.gameObject.SetActive(true);    
    private void DisableHull() => hullCollision.gameObject.SetActive(false);

    private void EnableShield() => shieldCollision.gameObject.SetActive(true);
    private void DisableShield() => shieldCollision.gameObject.SetActive(false);

    public void RestoreHealth(float amountToRestore)
    {
        if (currentHealth == maxHealth) return;

        currentHealth += amountToRestore;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        OnHullHealthChangedEvent?.Invoke(currentHealth / maxHealth);
    }

    public void RestoreShield(float amountToRestore)
    {
        if (currentShield == maxShield) return;

        if (currentShield == 0) EnableShield();

        currentShield += amountToRestore;
        if (currentShield > maxShield) currentShield = maxShield;

        OnShieldHealthChangedEvent?.Invoke(currentShield / maxShield);
    }
}
