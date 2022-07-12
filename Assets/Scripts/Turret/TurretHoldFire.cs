using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretHoldFire : TurretBase
{
    [Header("This turret specific")]
    [SerializeField] private List<Transform> firingPoints;
    [Space(10)]
    [Tooltip("Pause time between each firing point Fire action")]
    [SerializeField] private float timeBetweenShots = 0.2f;
    [Tooltip("How often the Fire action should be called")]
    [SerializeField] private float timeBetweenFireActions = 10;
    [Tooltip("Time till the turret overheats when hold to fire mode is selected")]
    [SerializeField, Range(1, 20)] private float overheatTime = 5f;
    [Tooltip("Time till the turret cools off when hold to fire mode is selected")]
    [SerializeField, Range(1, 20)] private float coolingTime = 3f;
    [Tooltip("Time that will take to charge to fire a projectile")]
    [SerializeField] private float overheatPerShot = 0.3f;

    private TurretFiringController turretFiringController;
    
    /// <summary>Minimum value to no overshoot below zero, to avoid zero divisions</summary>
    private static readonly float MIN_VALUE_AMOUNT = 0.01f;

    private float currentOverheatAmount = MIN_VALUE_AMOUNT;
    private float currentChargeAmount = MIN_VALUE_AMOUNT;

    private float timeBetweenFireAction;
    private float coolOffValue;

    private bool isHoldActive;
    private bool isOverheated;
    private bool isFireActionTakingPlace;

    private Coroutine holdCoroutine;
    private Coroutine increaseOverheatCoroutine;
    private Coroutine decreaseOverheatCoroutine;

    private bool isFiring;
    
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
        turretFiringController.onFireHoldStarted -= StartHoldEvent;
        turretFiringController.onFireHoldCanceled -= StopHoldEvent;
        turretFiringController.onFireHoldPerformed -= StopHoldEvent;
    }

    // public void OnFireHold(InputAction.CallbackContext context)
    // {
    //     if (context.started) StartHoldEvent();
    //     if (context.canceled) StopHoldEvent();
    //     if (context.performed) StopHoldEvent();
    // }
    
    private void StartHoldEvent()
    {
        if (isOverheated) return;

        timeBetweenFireAction = 1 / (float)timeBetweenFireActions;
        isHoldActive = true;
        StopCoroutine(nameof(DecreaseOverheat));
        holdCoroutine = StartCoroutine(nameof(FireHold));
        //increaseOverheatCoroutine = StartCoroutine(nameof(IncreaseOverheat));
    }

    private void StopHoldEvent()
    {
        if (isOverheated) return;

        isHoldActive = false;
        StopCoroutine(holdCoroutine);
        //StopCoroutine(increaseOverheatCoroutine);

        if (currentOverheatAmount > 0)
        {
            coolOffValue = (overheatTime / coolingTime) / 50;
            decreaseOverheatCoroutine = StartCoroutine(nameof(DecreaseOverheat));
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
        while (isHoldActive && currentOverheatAmount < overheatTime)
        {
            currentOverheatAmount += 0.02f;
            if (currentOverheatAmount > overheatTime) currentOverheatAmount = overheatTime;

            OnOverheatAmountChanged?.Invoke(currentOverheatAmount / overheatTime);

            if (currentOverheatAmount >= overheatTime)
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

            OnOverheatAmountChanged?.Invoke(currentOverheatAmount / overheatTime);

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
            FireProjectile(firingPoint);
            IncreaseOverheatPerShot();
            yield return new WaitForSeconds(timeBetweenShots);
        }

        StartCoroutine(nameof(WaitForNextFireAction));
        isFiring = false;
    }

    private void IncreaseOverheatPerShot()
    {
        currentOverheatAmount += overheatPerShot;
        if (currentOverheatAmount > overheatTime) currentOverheatAmount = overheatTime;

        OnOverheatAmountChanged?.Invoke(currentOverheatAmount / overheatTime);

        if (currentOverheatAmount >= overheatTime)
        {
            StopHoldEvent();
            isOverheated = true;
        }
    }

    private IEnumerator WaitForNextFireAction()
    {
        yield return new WaitForSeconds(timeBetweenFireActions);
        isFireActionTakingPlace = false;
    }
}
