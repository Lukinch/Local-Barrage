using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretChargeFire : TurretBase
{
    [Header("This turret specific")]
    [SerializeField] private List<Transform> _firingPoints;

    [Space(10)]
    [Tooltip("Scriptable Object containing the turret stats")]
    [SerializeField] private TurretChargeFireStatsSO _turretChargeFireStatsSO;

    /// <summary>Minimum value to no overshoot below zero, to avoid zero divisions</summary>
    private static readonly float MIN_VALUE_AMOUNT = 0.001f;

    private float _currentChargeAmount = MIN_VALUE_AMOUNT;

    private bool _isChargeActive;

    private Coroutine _chargeCoroutine;

    public event Action<float> OnChargeAmountChanged;

    private void OnEnable()
    {
        if (turretFiringController == null)
        {
            turretFiringController = GetComponentInParent<TurretFiringController>();
        }

        turretFiringController.OnFireChargeStarted += StartChargeEvent;
        turretFiringController.OnFireChargeCanceled += StopChargeEvent;
        turretFiringController.OnFireChargePerformed += StopChargeEvent;
    }

    private void OnDisable()
    {
        _isChargeActive = false;
        _currentChargeAmount = 0;

        turretFiringController.OnFireChargeStarted -= StartChargeEvent;
        turretFiringController.OnFireChargeCanceled -= StopChargeEvent;
        turretFiringController.OnFireChargePerformed -= StopChargeEvent;
    }

    private void StartChargeEvent()
    {
        _isChargeActive = true;

        _chargeCoroutine = StartCoroutine(nameof(FireCharge));
    }

    private void StopChargeEvent()
    {
        _isChargeActive = false;

        if (_chargeCoroutine != null)
            StopCoroutine(_chargeCoroutine);

        _currentChargeAmount = MIN_VALUE_AMOUNT;

        OnChargeAmountChanged?.Invoke(
            _currentChargeAmount / _turretChargeFireStatsSO.chargeTime);
    }

    private IEnumerator FireCharge()
    {
        while (_isChargeActive && _currentChargeAmount <= _turretChargeFireStatsSO.chargeTime)
        {
            _currentChargeAmount += 0.02f;

            OnChargeAmountChanged?.Invoke(
                _currentChargeAmount / _turretChargeFireStatsSO.chargeTime);

            if (_currentChargeAmount >= _turretChargeFireStatsSO.chargeTime)
            {
                Fire();
                StopChargeEvent();
            }

            yield return new WaitForFixedUpdate();
        }
    }

    protected override void Fire()
    {
        StartCoroutine(nameof(FireFromFiringPoints));
    }

    private IEnumerator FireFromFiringPoints()
    {
        foreach (Transform firingPoint in _firingPoints)
        {
            FireProjectile(
                firingPoint,
                _turretChargeFireStatsSO.damagePerShot,
                _turretChargeFireStatsSO.projectileForce);

            yield return new WaitForSeconds(_turretChargeFireStatsSO.timeBetweenShots);
        }
    }
}
