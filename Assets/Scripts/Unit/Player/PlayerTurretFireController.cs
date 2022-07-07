using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretFireController : MonoBehaviour
{
    #region Serialized Fieds
    [SerializeField] private InputReaderSO inputReader;

    [Header("Turret Dependencies")]
    [Tooltip("The turret that should handle the firing logic")]
    [SerializeField] private TurretBase turret;
    [Tooltip("The desired firing mode logic")]
    [SerializeField] private GlobalEnums.FiringMode firingMode;
    [Tooltip("Bullets per second when hold to fire mode is selected")]
    [SerializeField] private int firingRate = 10;
    [Tooltip("Time till the turret overheats when hold to fire mode is selected")]
    [SerializeField, Range(1, 20)] private float overheatTime = 5f;
    [Tooltip("Time till the turret cools off when hold to fire mode is selected")]
    [SerializeField, Range(1, 20)] private float coolingTime = 3f;
    [Tooltip("Time that will take to charge to fire a projectile")]
    [SerializeField, Range(1, 5)] private float chargeTime = 1.5f;
    #endregion

    #region Private variables
    private float currentOverheatAmount = MIN_VALUE_AMOUNT;
    private float currentChargeAmount = MIN_VALUE_AMOUNT;

    private float timeBetweenShots;
    private float coolOffValue;

    private bool isHoldActive;
    private bool isChargeActive;
    private bool isOverheated;

    private Coroutine holdCoroutine;
    private Coroutine increaseOverheatCoroutine;
    private Coroutine decreaseOverheatCoroutine;
    private Coroutine chargeCoroutine;

    private static readonly float MIN_VALUE_AMOUNT = 0.01f;
    #endregion

    #region Public variables and Actions
    [HideInInspector] public bool holdFireModeActive;
    [HideInInspector] public bool chargeFireModeActive;

    public event Action<float> OnOverheatAmountChanged;
    public event Action<float> OnChargeAmountChanged;
    #endregion

    #region Initialize
    private void OnEnable() => SubscribeToInputs();
    private void OnDisable() => DisableAllInputs();

    private void SubscribeToInputs()
    {
        switch (firingMode)
        {
            case GlobalEnums.FiringMode.TAP_TO_FIRE:
                SubscribeToTapFireEvents();
                break;
            case GlobalEnums.FiringMode.HOLD_TO_FIRE:
                holdFireModeActive = true;
                SubscribeToHoldFireEvents();
                break;
            case GlobalEnums.FiringMode.HOLD_TO_CHARGE:
                chargeFireModeActive = true;
                SubscribeToChargeFireEvents();
                break;
        }
    }
    #endregion

    #region Subscription Handling
    private void SubscribeToTapFireEvents()
    {
        inputReader.FireInstantEvent += OnFireInstantPerformed;
    }
    private void SubscribeToHoldFireEvents()
    {
        inputReader.FireHoldEventStarted += OnFireHoldStarted;
        inputReader.FireHoldEventCanceled += OnFireHoldCanceled;
        inputReader.FireHoldEventPerformed += OnFireHoldPerformed;
    }
    private void SubscribeToChargeFireEvents()
    {
        inputReader.FireChargeEventStarted += OnFireChargeStarted;
        inputReader.FireChargeEventCanceled += OnFireChargeCanceled;
        inputReader.FireChargeEventPerformed += OnFireChargePerformed;
    }

    private void DisableAllInputs()
    {
        inputReader.FireInstantEvent -= OnFireInstantPerformed;

        inputReader.FireHoldEventStarted -= OnFireHoldStarted;
        inputReader.FireHoldEventCanceled -= OnFireHoldCanceled;
        inputReader.FireHoldEventPerformed -= OnFireHoldPerformed;

        inputReader.FireChargeEventStarted -= OnFireChargeStarted;
        inputReader.FireChargeEventCanceled -= OnFireChargeCanceled;
        inputReader.FireChargeEventPerformed -= OnFireChargePerformed;
    }
    #endregion

    #region Actions calling
    private void OnFireInstantPerformed() => turret.Fire();

    private void OnFireHoldStarted() => StartHoldEvent();
    private void OnFireHoldCanceled() => StopHoldEvent();
    private void OnFireHoldPerformed() => StopHoldEvent();

    private void OnFireChargeStarted() => StartChargeEvent();
    private void OnFireChargeCanceled() => StopChargeEvent();
    private void OnFireChargePerformed() => StopChargeEvent();
    #endregion

    #region Hold To Fire Logic
    private void StartHoldEvent()
    {
        if (isOverheated) return;

        timeBetweenShots = 1 / (float)firingRate;
        isHoldActive = true;
        StopCoroutine(nameof(DecreaseOverheat));
        holdCoroutine = StartCoroutine(nameof(FireHold));
        increaseOverheatCoroutine = StartCoroutine(nameof(IncreaseOverheat));
    }

    private void StopHoldEvent()
    {
        if (isOverheated) return;

        isHoldActive = false;
        StopCoroutine(holdCoroutine);
        StopCoroutine(increaseOverheatCoroutine);

        if (currentOverheatAmount > 0)
        {
            coolOffValue = (overheatTime / coolingTime) / 50;
            decreaseOverheatCoroutine = StartCoroutine(nameof(DecreaseOverheat));
        }

        timeBetweenShots = 500;
    }

    private IEnumerator FireHold()
    {
        while (isHoldActive)
        {
            turret.Fire();
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    private IEnumerator IncreaseOverheat()
    {
        while(isHoldActive && currentOverheatAmount < overheatTime)
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
                turret.Fire();
                StopChargeEvent();
            }

            yield return new WaitForFixedUpdate();
        }
    }
    #endregion
}
