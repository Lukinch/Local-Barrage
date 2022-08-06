using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerTurretController : MonoBehaviour
{
    [SerializeField] private PlayerTurretController turretController;

    [SerializeField] private TurretUIOverheatBar holdUI;
    [SerializeField] private TurretUIChargeBar chargeUI;
    [SerializeField] private TurretUIReloadBar reloadUI;

    private void OnEnable()
    {
        turretController.OnHoldTurretPicked += OnHoldUIRequested;
        turretController.OnChargeTurretPicked += OnChargeUIRequested;
        turretController.OnTapTurretPicked += OnTapUIRequested;
    }
    private void OnDisable()
    {
        turretController.OnHoldTurretPicked -= OnHoldUIRequested;
        turretController.OnChargeTurretPicked -= OnChargeUIRequested;
        turretController.OnTapTurretPicked -= OnTapUIRequested;
    }

    private void OnHoldUIRequested(TurretHoldFire holdScript)
    {
        DisableAllUI();
        holdUI.TurretHoldScript = holdScript;
        holdUI.gameObject.SetActive(true);
    }
    private void OnChargeUIRequested(TurretChargeFire chargeScript)
    {
        DisableAllUI();
        chargeUI.TurretChargeScript = chargeScript;
        chargeUI.gameObject.SetActive(true);
    }
    private void OnTapUIRequested(TurretTapFire tapScript)
    {
        DisableAllUI();
        reloadUI.TurretTapScript = tapScript;
        reloadUI.gameObject.SetActive(true);
    }

    private void DisableAllUI()
    {
        holdUI.gameObject.SetActive(false);
        chargeUI.gameObject.SetActive(false);
        reloadUI.gameObject.SetActive(false);
    }
}
