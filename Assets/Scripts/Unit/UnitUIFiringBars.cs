using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUIFiringBars : MonoBehaviour
{
    [SerializeField] private PlayerTurretFireController fireController;

    [Space(10)]
    [Tooltip("GameObject that holds the Overheat bar")]
    [SerializeField] private GameObject overHeatGameObject;
    [Tooltip("GameObject that holds the Charge bar")]
    [SerializeField] private GameObject chargeUIGameObject;

    [Space(10)]
    [Tooltip("Overeat image to be filled")]
    [SerializeField] private Image overHeatImage;
    [Tooltip("Charge image to be filled")]
    [SerializeField] private Image chargeImage;
    [Tooltip("Rate at which the bar will update smoothly")]
    [SerializeField] private float easeSpeed = 2f;

    private float currentOverheatAmount;
    private float currentChargeAmount;

    private void OnEnable()
    {
        fireController.OnOverheatAmountChanged += OnOverheatAmountChanged;
        fireController.OnChargeAmountChanged += OnChargeAmountChanged;
    }

    private void OnDisable()
    {
        fireController.OnOverheatAmountChanged -= OnOverheatAmountChanged;
        fireController.OnChargeAmountChanged -= OnChargeAmountChanged;
    }

    private void Awake()
    {
        ResetUIComponentes();
    }

    private void Start()
    {
        if (fireController.holdFireModeActive) overHeatGameObject.SetActive(true);
        if (fireController.chargeFireModeActive) chargeUIGameObject.SetActive(true);
    }

    private void Update()
    {
        if (fireController.holdFireModeActive &&
            overHeatImage.fillAmount != currentOverheatAmount)
            UpdateOverheatVisuals();
        if (fireController.chargeFireModeActive && 
            chargeImage.fillAmount != currentChargeAmount)
            UpdateChargeVisuals();
    }

    private void OnOverheatAmountChanged(float overheatAmount) => currentOverheatAmount = overheatAmount;
    private void OnChargeAmountChanged(float chargeAmount) => currentChargeAmount = chargeAmount;

    private void UpdateOverheatVisuals()
    {
        overHeatImage.fillAmount =
            Mathf.MoveTowards(overHeatImage.fillAmount, currentOverheatAmount, easeSpeed * Time.deltaTime);
    }
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
        overHeatGameObject.SetActive(false);
        chargeUIGameObject.SetActive(false);
        overHeatImage.fillAmount = 0;
        chargeImage.fillAmount = 0;
    }

}
