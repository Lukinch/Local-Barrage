using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUIHealth : MonoBehaviour
{
    [SerializeField] private UnitHealth unitHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private float easeSpeed = 2f;

    private float currentHealth;

    private void OnEnable() => unitHealth.OnTakeDamageEvent += OnTakeHealthDamage;
    private void OnDisable() => unitHealth.OnTakeDamageEvent -= OnTakeHealthDamage;

    private void Start() => currentHealth = healthBar.fillAmount;

    private void Update()
    {
        if (healthBar.fillAmount != currentHealth)
            UpdateHealthVisuals();
    }

    private void OnTakeHealthDamage(float unitHealth) => currentHealth = unitHealth;

    private void UpdateHealthVisuals()
    {
        healthBar.fillAmount =
            Mathf.MoveTowards(healthBar.fillAmount, currentHealth, easeSpeed * Time.deltaTime);
    }
}
