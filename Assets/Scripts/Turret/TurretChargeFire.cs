using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretChargeFire : TurretBase
{
    #region Serialized Fieds
    [Header("This turret specific")]
    [SerializeField] private List<Transform> firingPoints;
    [Space(10)]
    [Tooltip("Pause time between each firing point Fire action")]
    [SerializeField] private float timeBetweenShots = 0.2f;
    [Tooltip("Time that will take to charge to fire a projectile")]
    [SerializeField, Range(0.1f, 5)] private float chargeTime = 1.5f;
    #endregion

    #region Private variables
    private float currentChargeAmount = MIN_VALUE_AMOUNT;

    private bool isChargeActive;

    private Coroutine chargeCoroutine;

    /// <summary>Minimum value to no overshoot below zero, to avoid zero divisions</summary>
    private static readonly float MIN_VALUE_AMOUNT = 0.01f;
    #endregion

    #region Public variables and Actions
    public event Action<float> OnChargeAmountChanged;
    #endregion

    #region Initialize
    private void OnEnable() => SubscribeToInputs();
    private void OnDisable()
    {
        DisableAllInputs();
        StopAllCoroutines();
    }
    #endregion

    #region Subscription Handling
    private void SubscribeToInputs()
    {
        inputReader.FireChargeEventStarted += OnFireChargeStarted;
        inputReader.FireChargeEventCanceled += OnFireChargeCanceled;
        inputReader.FireChargeEventPerformed += OnFireChargePerformed;
    }

    private void DisableAllInputs()
    {
        inputReader.FireChargeEventStarted -= OnFireChargeStarted;
        inputReader.FireChargeEventCanceled -= OnFireChargeCanceled;
        inputReader.FireChargeEventPerformed -= OnFireChargePerformed;
    }
    #endregion

    #region Actions calling
    private void OnFireChargeStarted() => StartChargeEvent();
    private void OnFireChargeCanceled() => StopChargeEvent();
    private void OnFireChargePerformed() => StopChargeEvent();
    #endregion

    #region Charge To Fire Logic
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
    #endregion
}
