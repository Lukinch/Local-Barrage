using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHoldFire : TurretBase
{
    [Header("This turret specific")]
    [SerializeField] private List<Transform> _firingPoints;
    [Space(10)]
    [Tooltip("Scriptable Object containing the turret stats")]
    [SerializeField] private TurretHoldFireStatsSO _turretHoldFireStatsSO;

    /// <summary>Minimum value to no overshoot below zero, to avoid zero divisions</summary>
    private static readonly float MIN_VALUE_AMOUNT = 0.001f;
    private static readonly float FIXED_UPDATE_CALLS_PER_SECOND = 50f;

    private float _currentOverheatAmount = MIN_VALUE_AMOUNT;

    private float _coolOffValue;

    private bool _isHoldActive;
    private bool _isFiring;
    private bool _isOverheated;
    private bool _isFireActionTakingPlace;

    private Coroutine _holdCoroutine;


    public event Action<float> OnOverheatAmountChanged;

    private void OnEnable()
    {
        if (turretFiringController == null)
        {
            turretFiringController = GetComponentInParent<TurretFiringController>();
        }

        turretFiringController.OnFireHoldStarted += StartHoldEvent;
        turretFiringController.OnFireHoldCanceled += StopHoldEvent;
        turretFiringController.OnFireHoldPerformed += StopHoldEvent;
    }

    private void OnDisable()
    {
        _isHoldActive = false;
        _isFireActionTakingPlace = false;
        _isFiring = false;
        _isOverheated = false;
        _currentOverheatAmount = 0;

        turretFiringController.OnFireHoldStarted -= StartHoldEvent;
        turretFiringController.OnFireHoldCanceled -= StopHoldEvent;
        turretFiringController.OnFireHoldPerformed -= StopHoldEvent;
    }

    private void FixedUpdate()
    {
        if (_isHoldActive) return;
        if (_isFireActionTakingPlace) return;
        if (_isFiring) return;

        if (_currentOverheatAmount > MIN_VALUE_AMOUNT)
        {
            _currentOverheatAmount -= _coolOffValue;
            if (_currentOverheatAmount < 0) _currentOverheatAmount = MIN_VALUE_AMOUNT;

            OnOverheatAmountChanged?.Invoke(_currentOverheatAmount / _turretHoldFireStatsSO.overheatTime);

            if (_isOverheated && _currentOverheatAmount <= MIN_VALUE_AMOUNT)
            {
                _isOverheated = false;
            }
        }
    }

    private void StartHoldEvent()
    {
        if (_isOverheated) return;

        _isHoldActive = true;
        _holdCoroutine = StartCoroutine(nameof(FireHold));
    }

    private void StopHoldEvent()
    {
        if (_isOverheated) return;

        _isHoldActive = false;

        if (_holdCoroutine != null)
            StopCoroutine(_holdCoroutine);

        if (_currentOverheatAmount > 0)
        {
            _coolOffValue =
                (_turretHoldFireStatsSO.overheatTime /
                _turretHoldFireStatsSO.coolingTime) /
                FIXED_UPDATE_CALLS_PER_SECOND;
        }
    }

    private IEnumerator FireHold()
    {
        while (_isHoldActive)
        {
            Fire();
            yield return new WaitForFixedUpdate();
        }
    }

    protected override void Fire()
    {
        if (_isFireActionTakingPlace) return;
        if (_isFiring) return;

        _isFireActionTakingPlace = true;
        _isFiring = true;
        StartCoroutine(nameof(FireFromFiringPoints));
    }

    private IEnumerator FireFromFiringPoints()
    {
        foreach (Transform firingPoint in _firingPoints)
        {
            FireProjectile(
                firingPoint,
                _turretHoldFireStatsSO.damagePerShot,
                _turretHoldFireStatsSO.projectileForce);

            IncreaseOverheatPerShot();
            yield return new WaitForSeconds(_turretHoldFireStatsSO.timeBetweenShots);
        }

        StartCoroutine(nameof(WaitForNextFireAction));
        _isFiring = false;
    }

    private void IncreaseOverheatPerShot()
    {
        _currentOverheatAmount += _turretHoldFireStatsSO.overheatPerShot;
        if (_currentOverheatAmount > _turretHoldFireStatsSO.overheatTime)
            _currentOverheatAmount = _turretHoldFireStatsSO.overheatTime;

        OnOverheatAmountChanged?.Invoke(
            _currentOverheatAmount / _turretHoldFireStatsSO.overheatTime);

        if (_currentOverheatAmount >= _turretHoldFireStatsSO.overheatTime)
        {
            StopHoldEvent();
            _isOverheated = true;
        }
    }

    private IEnumerator WaitForNextFireAction()
    {
        yield return new WaitForSeconds(_turretHoldFireStatsSO.timeBetweenFireActions);
        _isFireActionTakingPlace = false;
    }
}
