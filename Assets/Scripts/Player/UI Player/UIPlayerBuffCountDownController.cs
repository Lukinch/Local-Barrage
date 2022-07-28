using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerBuffCountDownController : MonoBehaviour
{
    [SerializeField] private PlayerTurretController turretController;
    [SerializeField] private Image fillImage;

    private float maxBuffDuration;
    private float amountToSubtract;

    private void OnEnable()
    {
        maxBuffDuration = turretController.BuffDuration;
        fillImage.fillAmount = 0;
        // 1 = Max Image Fill Amount, 50 = Fixed Update Calls per second
        amountToSubtract = (1 / maxBuffDuration) / 50;
        
        turretController.OnBuffStarted += OnBuffStarted;
    }

    private void OnDisable()
    {
        turretController.OnBuffStarted -= OnBuffStarted;
    }

    private void OnBuffStarted()
    {
        fillImage.fillAmount = 1;
        StartCoroutine(nameof(StartCountdown));
    }

    private IEnumerator StartCountdown()
    {
        while (fillImage.fillAmount > 0)
        {
            fillImage.fillAmount -= amountToSubtract;
            yield return new WaitForFixedUpdate();
        }
    }
}
