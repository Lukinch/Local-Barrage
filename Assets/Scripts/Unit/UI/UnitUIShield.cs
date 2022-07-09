using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUIShield : MonoBehaviour
{
    [SerializeField] private UnitShield unitShield;
    [SerializeField] private Image shieldBar;
    [SerializeField] private float easeSpeed = 2f;

    private float currentShield;

    private void OnEnable() => unitShield.OnTakeDamageEvent += OnTakeShieldDamage;
    private void OnDisable() => unitShield.OnTakeDamageEvent -= OnTakeShieldDamage;

    private void Start() => currentShield = shieldBar.fillAmount;

    private void Update()
    {
        if (shieldBar.fillAmount != currentShield)
            UpdateShieldVisuals();
    }

    private void OnTakeShieldDamage(float unitShield) => currentShield = unitShield;

    private void UpdateShieldVisuals()
    {
        shieldBar.fillAmount =
            Mathf.MoveTowards(shieldBar.fillAmount, currentShield, easeSpeed * Time.deltaTime);
    }
}
