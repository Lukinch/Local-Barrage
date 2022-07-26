using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretUIOverheatBar : MonoBehaviour
{
    [SerializeField] private TurretHoldFire turretHoldScript;

    [Space(10)]
    [Tooltip("Overeat image to be filled")]
    [SerializeField] private Image overHeatImage;
    [Tooltip("Rate at which the bar will update smoothly")]
    [SerializeField] private float easeSpeed = 2f;

    private float currentOverheatAmount;

    private void OnEnable()
    {
        turretHoldScript.OnOverheatAmountChanged += OnOverheatAmountChanged;
    }

    private void OnDisable()
    {
        ResetUIComponentes();
        turretHoldScript.OnOverheatAmountChanged -= OnOverheatAmountChanged;
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