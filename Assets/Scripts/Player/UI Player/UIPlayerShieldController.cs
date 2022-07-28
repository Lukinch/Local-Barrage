using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerShieldController : MonoBehaviour
{
    [SerializeField] private PlayerStatsController playerDamageController;
    [SerializeField] private Image shieldBar;
    [SerializeField] private float easeSpeed = 2f;

    private float currentShield;

    private void OnEnable() => playerDamageController.OnShieldHealthChangedEvent += OnTakeShieldDamage;
    private void OnDisable()
    {
        currentShield = shieldBar.fillAmount = 1;
        playerDamageController.OnShieldHealthChangedEvent -= OnTakeShieldDamage;
    }

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
