
using System;
using UnityEngine;

public class DummyStatsController : MonoBehaviour
{
    [SerializeField] private GameObject _explosionVsf;
    [SerializeField] private DummyHullCollider _hullCollision;
    [SerializeField] private DummyShieldCollider _shieldCollision;

    [SerializeField] private float _maxHealth;
    [SerializeField] private float _maxShield;

    private float _currentHealth;
    private float _currentShield;

    public event Action<float> OnHullHealthChangedEvent;
    public event Action<float> OnShieldHealthChangedEvent;
    public static event Action OnDummyKilled;

    private void OnEnable()
    {
        _hullCollision.HullProjectileCollisionEvent += ProjectileDamageHull;
        _hullCollision.HullPickableCollisionEvent += PickableDamageHull;
        _shieldCollision.ShieldProjectileCollisionEvent += ProjectileDamageShield;
        _shieldCollision.ShieldPickableCollisionEvent += PickableDamageShield;
    }

    private void OnDisable()
    {
        _hullCollision.HullProjectileCollisionEvent -= ProjectileDamageHull;
        _hullCollision.HullPickableCollisionEvent -= PickableDamageHull;
        _shieldCollision.ShieldProjectileCollisionEvent -= ProjectileDamageShield;
        _shieldCollision.ShieldPickableCollisionEvent -= PickableDamageShield;
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
        _currentShield = _maxShield;
        DisableHull();
    }
    private void DisableSelf()
    {
        Instantiate(_explosionVsf, transform.position, transform.rotation);
        Destroy(gameObject);
        OnDummyKilled?.Invoke();
    }

    private void EnableHull() => _hullCollision.gameObject.SetActive(true);
    private void DisableHull() => _hullCollision.gameObject.SetActive(false);

    private void EnableShield() => _shieldCollision.gameObject.SetActive(true);
    private void DisableShield() => _shieldCollision.gameObject.SetActive(false);

    public void ProjectileDamageHull(float damage, int projectileOwner)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;

            DisableSelf();
        }

        OnHullHealthChangedEvent?.Invoke(_currentHealth / _maxHealth);
    }
    public void PickableDamageHull(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            DisableSelf();
        }

        OnHullHealthChangedEvent?.Invoke(_currentHealth / _maxHealth);
    }

    public void ProjectileDamageShield(float damage, int projectileOwner)
    {
        _currentShield -= damage;
        if (_currentShield <= 0)
        {
            _currentShield = 0;
            DisableShield();
            EnableHull();
        }

        OnShieldHealthChangedEvent?.Invoke(_currentShield / _maxShield);
    }
    public void PickableDamageShield(float damage)
    {
        _currentShield -= damage;
        if (_currentShield <= 0)
        {
            _currentShield = 0;
            DisableShield();
            EnableHull();
        }

        OnShieldHealthChangedEvent?.Invoke(_currentShield / _maxShield);
    }

    public void RestoreHealth(float amountToRestore)
    {
        if (_currentHealth == _maxHealth) return;

        _currentHealth += amountToRestore;
        if (_currentHealth > _maxHealth) _currentHealth = _maxHealth;

        OnHullHealthChangedEvent?.Invoke(_currentHealth / _maxHealth);
    }

    public void RestoreShield(float amountToRestore)
    {
        if (_currentShield == _maxShield) return;

        if (_currentShield == 0)
        {
            DisableHull();
            EnableShield();
        }

        _currentShield += amountToRestore;
        if (_currentShield > _maxShield) _currentShield = _maxShield;

        OnShieldHealthChangedEvent?.Invoke(_currentShield / _maxShield);
    }

    public bool HealthCanBeHealed()
    {
        return _currentHealth < _maxHealth;
    }

    public bool ShieldCanBeHealed()
    {
        return _currentShield < _maxShield;
    }

    public bool IsShieldActive()
    {
        return _shieldCollision.gameObject.activeSelf;
    }
}
