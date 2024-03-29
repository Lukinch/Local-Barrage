﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretUIChargeBar : MonoBehaviour
{
    [Space(10)]
    [Tooltip("Charge image to be filled")]
    [SerializeField] private Image chargeImage;
    [Tooltip("Rate at which the bar will update smoothly")]
    [SerializeField] private float easeSpeed = 2f;

    private float currentChargeAmount;

    [SerializeField] private TurretChargeFire turretChargeScript;
    public TurretChargeFire TurretChargeScript
    {
        get => turretChargeScript;
        set
        {
            UnSubscribeFromTurret();
            turretChargeScript = value;
            SubscribeToTurret();
        }
    }

    private void SubscribeToTurret()
    {
        turretChargeScript.OnChargeAmountChanged += OnChargeAmountChanged;
    }

    private void UnSubscribeFromTurret()
    {
        if (turretChargeScript == null) return;

        turretChargeScript.OnChargeAmountChanged -= OnChargeAmountChanged;
    }

    private void OnDisable()
    {
        ResetUIComponentes();
        UnSubscribeFromTurret();
    }

    private void Update()
    {
        if (chargeImage.fillAmount != currentChargeAmount)
            UpdateChargeVisuals();
    }

    private void OnChargeAmountChanged(float chargeAmount) => currentChargeAmount = chargeAmount;

    private void UpdateChargeVisuals()
    {
        if (currentChargeAmount <= chargeImage.fillAmount) chargeImage.fillAmount = 0;
        else
        {
            chargeImage.fillAmount =
                Mathf.MoveTowards(chargeImage.fillAmount, currentChargeAmount, easeSpeed * Time.deltaTime);
        }
    }

    private void ResetUIComponentes()
    {
        currentChargeAmount = 0;
        chargeImage.fillAmount = 0;
    }
}