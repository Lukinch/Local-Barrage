using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHoldFire : TurretBase
{
    [Header("This turret specific")]
    [SerializeField] private List<Transform> firingPoints;
    [Space(10)]
    [Tooltip("Scriptable Object containing the turret stats")]
    [SerializeField] private TurretHoldFireStatsSO turretHoldFireStatsSO;
    
    /// <summary>Minimum value to no overshoot below zero, to avoid zero divisions</summary>
    private static readonly float MIN_VALUE_AMOUNT = 0.001f;

    private float currentOverheatAmount = MIN_VALUE_AMOUNT;

    private float timeBetweenFireAction;
    private float coolOffValue;

    private bool isHoldActive;
    private bool isFiring;
    private bool isOverheated;
    private bool isFireActionTakingPlace;

    private Coroutine holdCoroutine;
    private Coroutine decreaseOverheatCoroutine;

    
    public event Action<float> OnOverheatAmountChanged;
    
    private void OnEnable()
    {
        if (turretFiringController == null)
        {
            turretFiringController = GetComponentInParent<TurretFiringController>();
        }
        
        turretFiringController.onFireHoldStarted += StartHoldEvent;
        turretFiringController.onFireHoldCanceled += StopHoldEvent;
        turretFiringController.onFireHoldPerformed += StopHoldEvent;
    }
    
    private void OnDisable()
    {
        isHoldActive = false;
        isFireActionTakingPlace = false;
        isFiring = false;
        currentOverheatAmount = 0;

        turretFiringController.onFireHoldStarted -= StartHoldEvent;
        turretFiringController.onFireHoldCanceled -= StopHoldEvent;
        turretFiringController.onFireHoldPerformed -= StopHoldEvent;
    }

    private void FixedUpdate()
    {
        if (isHoldActive) return;
        if (isFireActionTakingPlace) return;
        if (isFiring) return;
        
        if (currentOverheatAmount > MIN_VALUE_AMOUNT)
        {
            currentOverheatAmount -= coolOffValue;
            if (currentOverheatAmount < 0) currentOverheatAmount = MIN_VALUE_AMOUNT;

            OnOverheatAmountChanged?.Invoke(currentOverheatAmount / turretHoldFireStatsSO.overheatTime);

            if (isOverheated && currentOverheatAmount <= MIN_VALUE_AMOUNT)
            {
                isOverheated = false;
            }
        }
    }
    
    private void StartHoldEvent()
    {
        if (isOverheated) return;

        timeBetweenFireAction = 1 / (float)turretHoldFireStatsSO.timeBetweenFireActions;
        isHoldActive = true;
        //StopCoroutine(nameof(DecreaseOverheat));
        holdCoroutine = StartCoroutine(nameof(FireHold));
    }

    private void StopHoldEvent()
    {
        if (isOverheated) return;

        isHoldActive = false;
        StopCoroutine(holdCoroutine);

        if (currentOverheatAmount > 0)
        {
            coolOffValue = (turretHoldFireStatsSO.overheatTime / turretHoldFireStatsSO.coolingTime) / 50;
            //decreaseOverheatCoroutine = StartCoroutine(nameof(DecreaseOverheat));
        }
    }

    private IEnumerator FireHold()
    {
        while (isHoldActive)
        {
            Fire();
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator IncreaseOverheat()
    {
        while (isHoldActive && currentOverheatAmount < turretHoldFireStatsSO.overheatTime)
        {
            currentOverheatAmount += 0.02f;
            if (currentOverheatAmount > turretHoldFireStatsSO.overheatTime) currentOverheatAmount = turretHoldFireStatsSO.overheatTime;

            OnOverheatAmountChanged?.Invoke(currentOverheatAmount / turretHoldFireStatsSO.overheatTime);

            if (currentOverheatAmount >= turretHoldFireStatsSO.overheatTime)
            {
                StopHoldEvent();
                isOverheated = true;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator DecreaseOverheat()
    {
        while (currentOverheatAmount > MIN_VALUE_AMOUNT)
        {
            currentOverheatAmount -= coolOffValue;
            if (currentOverheatAmount < 0) currentOverheatAmount = MIN_VALUE_AMOUNT;

            OnOverheatAmountChanged?.Invoke(currentOverheatAmount / turretHoldFireStatsSO.overheatTime);

            if (isOverheated && currentOverheatAmount <= MIN_VALUE_AMOUNT)
            {
                isOverheated = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    protected override void Fire()
    {
        if (isFireActionTakingPlace) return;
        if (isFiring) return;

        isFireActionTakingPlace = true;
        isFiring = true;
        StartCoroutine(nameof(FireFromFirginPoints));
    }

    private IEnumerator FireFromFirginPoints()
    {
        foreach (Transform firingPoint in firingPoints)
        {
            FireProjectile(firingPoint, turretHoldFireStatsSO.damagePerShot, turretHoldFireStatsSO.projectileForce);
            IncreaseOverheatPerShot();
            yield return new WaitForSeconds(turretHoldFireStatsSO.timeBetweenShots);
        }

        StartCoroutine(nameof(WaitForNextFireAction));
        isFiring = false;
    }

    private void IncreaseOverheatPerShot()
    {
        currentOverheatAmount += turretHoldFireStatsSO.overheatPerShot;
        if (currentOverheatAmount > turretHoldFireStatsSO.overheatTime) currentOverheatAmount = turretHoldFireStatsSO.overheatTime;

        OnOverheatAmountChanged?.Invoke(currentOverheatAmount / turretHoldFireStatsSO.overheatTime);

        if (currentOverheatAmount >= turretHoldFireStatsSO.overheatTime)
        {
            StopHoldEvent();
            isOverheated = true;
        }
    }

    private IEnumerator WaitForNextFireAction()
    {
        yield return new WaitForSeconds(turretHoldFireStatsSO.timeBetweenFireActions);
        isFireActionTakingPlace = false;
    }
}
