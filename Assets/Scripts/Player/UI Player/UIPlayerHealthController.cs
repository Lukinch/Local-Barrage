using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHealthController : MonoBehaviour
{
    [SerializeField] private PlayerStatsController _playerDamageController;
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _easeSpeed = 2f;

    private float currentHealth;

    private void OnEnable() => _playerDamageController.OnHullHealthChangedEvent += OnTakeHealthDamage;
    private void OnDisable()
    {
        currentHealth = _healthBar.fillAmount = 1;
        _playerDamageController.OnHullHealthChangedEvent -= OnTakeHealthDamage;
    }

    private void Start() => currentHealth = _healthBar.fillAmount;

    private void Update()
    {
        if (_healthBar.fillAmount != currentHealth)
            UpdateHealthVisuals();
    }

    private void OnTakeHealthDamage(float unitHealth) => currentHealth = unitHealth;

    private void UpdateHealthVisuals()
    {
        _healthBar.fillAmount =
            Mathf.MoveTowards(_healthBar.fillAmount, currentHealth, _easeSpeed * Time.deltaTime);
    }
}
