using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretChargeFire : TurretBase
{
    [Header("This turret specific")]
    [SerializeField] private List<Transform> firingPoints;
    [Space(10)]
    [Tooltip("Pause time between each firing point Fire action")]
    [SerializeField] private float timeBetweenShots = 0.2f;
    [Tooltip("Time that will take to charge to fire a projectile")]
    [SerializeField, Range(0.1f, 5)] private float chargeTime = 1.5f;
    
    /// <summary>Minimum value to no overshoot below zero, to avoid zero divisions</summary>
    private float currentChargeAmount = MIN_VALUE_AMOUNT;

    private TurretFiringController turretFiringController;

    private bool isChargeActive;

    private Coroutine chargeCoroutine;

    /// <summary>Minimum value to no overshoot below zero, to avoid zero divisions</summary>
    private static readonly float MIN_VALUE_AMOUNT = 0.01f;
    
    public event Action<float> OnChargeAmountChanged;
    
    private void OnEnable()
    {
        if (turretFiringController == null)
        {
            turretFiringController = GetComponentInParent<TurretFiringController>();
        }

        turretFiringController.onFireChargeStarted += StartChargeEvent;
        turretFiringController.onFireChargeCanceled += StopChargeEvent;
        turretFiringController.onFireChargePerformed += StopChargeEvent;
    }
    
    private void OnDisable()
    {
        turretFiringController.onFireChargeStarted -= StartChargeEvent;
        turretFiringController.onFireChargeCanceled -= StopChargeEvent;
        turretFiringController.onFireChargePerformed -= StopChargeEvent;
    }
    
    private void StartChargeEvent()
    {
        isChargeActive = true;

        chargeCoroutine = StartCoroutine(nameof(FireCharge));
    }

    private void StopChargeEvent()
    {
        isChargeActive = false;
        StopCoroutine(chargeCoroutine);

        currentChargeAmount = MIN_VALUE_AMOUNT;

        OnChargeAmountChanged?.Invoke(currentChargeAmount / chargeTime);
    }

    private IEnumerator FireCharge()
    {
        while (isChargeActive && currentChargeAmount <= chargeTime)
        {
            currentChargeAmount += 0.02f;

            OnChargeAmountChanged?.Invoke(currentChargeAmount / chargeTime);

            if (currentChargeAmount >= chargeTime)
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
        foreach (Transform firingPoint in firingPoints)
        {
            FireProjectile(firingPoint);
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
