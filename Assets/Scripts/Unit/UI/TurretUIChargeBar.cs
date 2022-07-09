using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretUIChargeBar : MonoBehaviour
{
    [SerializeField] private TurretChargeFire turretChargeScript;

    [Space(10)]
    [Tooltip("Charge image to be filled")]
    [SerializeField] private Image chargeImage;
    [Tooltip("Rate at which the bar will update smoothly")]
    [SerializeField] private float easeSpeed = 2f;

    private float currentChargeAmount;

    private void OnEnable()
    {
        turretChargeScript.OnChargeAmountChanged += OnChargeAmountChanged;
    }

    private void OnDisable()
    {
        turretChargeScript.OnChargeAmountChanged -= OnChargeAmountChanged;
    }

    private void Awake()
    {
        ResetUIComponentes();
    }

    private void Update()
    {
        if (chargeImage.fillAmount != currentChargeAmount)
            UpdateChargeVisuals();
    }

    private void OnChargeAmountChanged(float chargeAmount) => currentChargeAmount = chargeAmount;

    private void UpdateChargeVisuals()
    {
        if (currentChargeAmount <= chargeImage.fillAmount) chargeImage.fillAmount = 0;
        else
        {
            chargeImage.fillAmount =
                Mathf.MoveTowards(chargeImage.fillAmount, currentChargeAmount, easeSpeed * Time.deltaTime);
        }
    }

    private void ResetUIComponentes()
    {
        chargeImage.fillAmount = 0;
    }
}