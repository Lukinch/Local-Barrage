
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private PlayerHullCollision hullCollision;
    [SerializeField] private PlayerShieldCollision shieldCollision;

    [SerializeField] private float maxHealth;
    [SerializeField] private float maxShield;

    private float currentHealth;
    private float currentShield;

    public event Action<float> OnHullHealthChangedEvent;
    public event Action<float> OnShieldHealthChangedEvent;
    public static event Action OnPlayerKilled;

    private void OnEnable()
    {
        hullCollision.HullProjectileCollisionEvent += ProjectileDamageHull;
        hullCollision.HullPickableCollisionEvent += PickableDamageHull;
        shieldCollision.ShieldProjectileCollisionEvent += ProjectileDamageShield;
        shieldCollision.ShieldPickableCollisionEvent += PickableDamageShield;
    }

    private void OnDisable()
    {
        hullCollision.HullProjectileCollisionEvent -= ProjectileDamageHull;
        hullCollision.HullPickableCollisionEvent -= PickableDamageHull;
        shieldCollision.ShieldProjectileCollisionEvent -= ProjectileDamageShield;
        shieldCollision.ShieldPickableCollisionEvent -= PickableDamageShield;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
        DisableHull();
    }
    private void DisableSelf()
    {
        currentHealth = maxHealth;
        OnHullHealthChangedEvent?.Invoke(currentHealth / maxHealth);
        DisableHull();

        currentShield = maxShield;
        OnShieldHealthChangedEvent?.Invoke(currentShield / maxShield);
        EnableShield();

        gameObject.SetActive(false);
        OnPlayerKilled?.Invoke();
    }

    private void EnableHull() => hullCollision.gameObject.SetActive(true);    
    private void DisableHull() => hullCollision.gameObject.SetActive(false);

    private void EnableShield() => shieldCollision.gameObject.SetActive(true);
    private void DisableShield() => shieldCollision.gameObject.SetActive(false);

    public void ProjectileDamageHull(float damage, int projectileOwner)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;

            GlobalPlayersManager.Instance.PlayersPoints[projectileOwner].AddPoints();

            DisableSelf();
        }

        OnHullHealthChangedEvent?.Invoke(currentHealth / maxHealth);
    }
    public void PickableDamageHull(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DisableSelf();
        }

        OnHullHealthChangedEvent?.Invoke(currentHealth / maxHealth);
    }

    public void ProjectileDamageShield(float damage, int projectileOwner)
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
    public void PickableDamageShield(float damage)
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

        if (currentShield == 0)
        {
            DisableHull();
            EnableShield();
        }

        currentShield += amountToRestore;
        if (currentShield > maxShield) currentShield = maxShield;

        OnShieldHealthChangedEvent?.Invoke(currentShield / maxShield);
    }

    public bool HealthCanBeHealed()
    {
        return currentHealth < maxHealth;
    }

    public bool ShieldCanBeHealed()
    {
        return currentShield < maxShield;
    }

    public bool IsShieldActive()
    {
        return shieldCollision.gameObject.activeSelf;
    }
}
