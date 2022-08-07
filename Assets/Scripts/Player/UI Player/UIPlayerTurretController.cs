using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerTurretController : MonoBehaviour
{
    [SerializeField] private PlayerTurretController _turretController;

    [SerializeField] private TurretUIOverheatBar _holdUI;
    [SerializeField] private TurretUIChargeBar _chargeUI;
    [SerializeField] private TurretUIReloadBar _reloadUI;

    private void OnEnable()
    {
        _turretController.OnHoldTurretPicked += OnHoldUIRequested;
        _turretController.OnChargeTurretPicked += OnChargeUIRequested;
        _turretController.OnTapTurretPicked += OnTapUIRequested;
    }
    private void OnDisable()
    {
        _turretController.OnHoldTurretPicked -= OnHoldUIRequested;
        _turretController.OnChargeTurretPicked -= OnChargeUIRequested;
        _turretController.OnTapTurretPicked -= OnTapUIRequested;
    }

    private void OnHoldUIRequested(TurretHoldFire holdScript)
    {
        DisableAllUI();
        _holdUI.TurretHoldScript = holdScript;
        _holdUI.gameObject.SetActive(true);
    }
    private void OnChargeUIRequested(TurretChargeFire chargeScript)
    {
        DisableAllUI();
        _chargeUI.TurretChargeScript = chargeScript;
        _chargeUI.gameObject.SetActive(true);
    }
    private void OnTapUIRequested(TurretTapFire tapScript)
    {
        DisableAllUI();
        _reloadUI.TurretTapScript = tapScript;
        _reloadUI.gameObject.SetActive(true);
    }

    private void DisableAllUI()
    {
        _holdUI.gameObject.SetActive(false);
        _chargeUI.gameObject.SetActive(false);
        _reloadUI.gameObject.SetActive(false);
    }
}
