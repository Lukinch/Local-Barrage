using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDummyShieldController : MonoBehaviour
{
    [SerializeField] private DummyStatsController _playerDamageController;
    [SerializeField] private Image _shieldBar;
    [SerializeField] private float _easeSpeed = 2f;

    private float _currentShield;

    private void OnEnable() => _playerDamageController.OnShieldHealthChangedEvent += OnTakeShieldDamage;
    private void OnDisable()
    {
        _currentShield = _shieldBar.fillAmount = 1;
        _playerDamageController.OnShieldHealthChangedEvent -= OnTakeShieldDamage;
    }

    private void Start() => _currentShield = _shieldBar.fillAmount;

    private void Update()
    {
        if (_shieldBar.fillAmount != _currentShield)
            UpdateShieldVisuals();
    }

    private void OnTakeShieldDamage(float unitShield) => _currentShield = unitShield;

    private void UpdateShieldVisuals()
    {
        _shieldBar.fillAmount =
            Mathf.MoveTowards(_shieldBar.fillAmount, _currentShield, _easeSpeed * Time.deltaTime);
    }
}
