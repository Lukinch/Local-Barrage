using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretChargeFire : TurretBase
{
    [Header("This turret specific")]
    [SerializeField] private List<Transform> firingPoints;
    [Space(10)]
    [Tooltip("Scriptable Object containing the turret stats")]
    [SerializeField] private TurretChargeFireStatsSO turretChargeFireStatsSO;
    
    /// <summary>Minimum value to no overshoot below zero, to avoid zero divisions</summary>
    private float currentChargeAmount = MIN_VALUE_AMOUNT;

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

        OnChargeAmountChanged?.Invoke(currentChargeAmount / turretChargeFireStatsSO.chargeTime);
    }

    private IEnumerator FireCharge()
    {
        while (isChargeActive && currentChargeAmount <= turretChargeFireStatsSO.chargeTime)
        {
            currentChargeAmount += 0.02f;

            OnChargeAmountChanged?.Invoke(currentChargeAmount / turretChargeFireStatsSO.chargeTime);

            if (currentChargeAmount >= turretChargeFireStatsSO.chargeTime)
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
            FireProjectile(firingPoint, turretChargeFireStatsSO.damagePerShot, turretChargeFireStatsSO.projectileForce);
            yield return new WaitForSeconds(turretChargeFireStatsSO.timeBetweenShots);
        }
    }
}
