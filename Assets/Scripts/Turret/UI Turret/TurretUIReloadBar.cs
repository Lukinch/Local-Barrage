using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretUIReloadBar : MonoBehaviour
{
    [Space(10)]
    [Tooltip("Charge image to be filled")]
    [SerializeField] private Image reloadImage;
    [Tooltip("Rate at which the bar will update smoothly")]
    [SerializeField] private float easeSpeed = 2f;

    private float currentChargeAmount;

    [SerializeField] private TurretTapFire turretTapFire;
    public TurretTapFire TurretTapScript
    {
        get => turretTapFire;
        set
        {
            UnSubscribeFromTurret();
            turretTapFire = value;
            SubscribeToTurret();
        }
    }

    private void SubscribeToTurret()
    {
        turretTapFire.OnReloadTimeChanged += OnTimeAmountChanged;
    }

    private void UnSubscribeFromTurret()
    {
        if (turretTapFire == null) return;

        turretTapFire.OnReloadTimeChanged -= OnTimeAmountChanged;
    }

    private void OnDisable()
    {
        ResetUIComponentes();
        UnSubscribeFromTurret();
    }

    private void Update()
    {
        if (reloadImage.fillAmount != currentChargeAmount)
            UpdateChargeVisuals();
    }

    private void OnTimeAmountChanged(float chargeAmount) => currentChargeAmount = chargeAmount;

    private void UpdateChargeVisuals()
    {
        if (currentChargeAmount == 0) reloadImage.fillAmount = 0;
        else
        {
            reloadImage.fillAmount =
                Mathf.MoveTowards(reloadImage.fillAmount, currentChargeAmount, easeSpeed * Time.deltaTime);
        }
    }

    private void ResetUIComponentes()
    {
        currentChargeAmount = 0;
        reloadImage.fillAmount = 0;
    }
}