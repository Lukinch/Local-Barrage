using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretUIOverheatBar : MonoBehaviour
{
    [Space(10)]
    [Tooltip("Overeat image to be filled")]
    [SerializeField] private Image overHeatImage;
    [Tooltip("Rate at which the bar will update smoothly")]
    [SerializeField] private float easeSpeed = 2f;

    private float currentOverheatAmount;

    [SerializeField] private TurretHoldFire turretHoldScript;
    public TurretHoldFire TurretHoldScript
    {
        get => turretHoldScript;
        set
        {
            UnSubscribeFromTurret();
            turretHoldScript = value;
            SubscribeToTurret();
        }
    }

    private void SubscribeToTurret()
    {
        turretHoldScript.OnOverheatAmountChanged += OnOverheatAmountChanged;
    }

    private void UnSubscribeFromTurret()
    {
        if (turretHoldScript == null) return;

        turretHoldScript.OnOverheatAmountChanged -= OnOverheatAmountChanged;
    }

    private void OnDisable()
    {
        ResetUIComponentes();
        UnSubscribeFromTurret();
    }

    private void Update()
    {
        if (overHeatImage.fillAmount != currentOverheatAmount)
            UpdateOverheatVisuals();
    }

    private void OnOverheatAmountChanged(float overheatAmount) => currentOverheatAmount = overheatAmount;

    private void UpdateOverheatVisuals()
    {
        overHeatImage.fillAmount =
            Mathf.MoveTowards(overHeatImage.fillAmount, currentOverheatAmount, easeSpeed * Time.deltaTime);
    }

    private void ResetUIComponentes()
    {
        currentOverheatAmount = 0;
        overHeatImage.fillAmount = 0;
    }
}