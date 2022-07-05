using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUIManager : MonoBehaviour
{
    [SerializeField] private UnitUIHealth unitHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private float easeSpeed = 2f;

    private float currentHealth;
    private Coroutine easeCoroutine;

    private void OnEnable()
    {
        unitHealth.OnTakeDamageEvent += OnTakeDamage;
    }
    private void OnDisable()
    {
        unitHealth.OnTakeDamageEvent -= OnTakeDamage;
    }

    private void Start()
    {
        currentHealth = healthBar.fillAmount;
    }

    private void Update()
    {
        if (healthBar.fillAmount != currentHealth)
            UpdateHealthVisuals();
    }

    private void OnTakeDamage(float unitHealth)
    {
        currentHealth = unitHealth;
    }

    private void UpdateHealthVisuals()
    {
        healthBar.fillAmount = 
            Mathf.MoveTowards(healthBar.fillAmount, currentHealth, easeSpeed * Time.deltaTime);
    }
}
