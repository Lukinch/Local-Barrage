using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretUIReloadBar : MonoBehaviour
{
    [SerializeField] private TurretTapFire turretTapScript;

    [Space(10)]
    [Tooltip("Charge image to be filled")]
    [SerializeField] private Image reloadImage;
    [Tooltip("Rate at which the bar will update smoothly")]
    [SerializeField] private float easeSpeed = 2f;

    private float currentChargeAmount;

    private void OnEnable()
    {
        turretTapScript.OnReloadTimeChanged += OnTimeAmountChanged;
    }

    private void OnDisable()
    {
        ResetUIComponentes();
        turretTapScript.OnReloadTimeChanged -= OnTimeAmountChanged;
    }

    private void Update()
    {
        if (reloadImage.fillAmount != currentChargeAmount)
            UpdateChargeVisuals();
    }

    private void OnTimeAmountChanged(float chargeAmount) => currentChargeAmount = chargeAmount;

    private void UpdateChargeVisuals()
    {
        if (currentChargeAmount == 0) reloadImage.fillAmount = 0;
        else
        {
            reloadImage.fillAmount =
                Mathf.MoveTowards(reloadImage.fillAmount, currentChargeAmount, easeSpeed * Time.deltaTime);
        }
    }

    private void ResetUIComponentes()
    {
        currentChargeAmount = 0;
        reloadImage.fillAmount = 0;
    }
}