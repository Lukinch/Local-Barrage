using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerBuffCountDownController : MonoBehaviour
{
    [SerializeField] private PlayerTurretController turretController;
    [SerializeField] private Image image;

    private float maxBuffDuration;
    private float amountToSubtract;

    private void OnEnable()
    {
        turretController.OnBuffStarted += OnBuffStarted;
    }

    private void OnDisable()
    {
        turretController.OnBuffStarted -= OnBuffStarted;
    }

    private void Awake()
    {
        //ResetUIComponentes();
    }

    private void Start()
    {
        maxBuffDuration = turretController.BuffDuration;
        image.fillAmount = 0;
        // 1 = Max Image Fill Amount, 50 = Fixed Update Calls per second
        amountToSubtract = (1 / maxBuffDuration) / 50;
    }

    private void OnBuffStarted()
    {
        image.fillAmount = 1;
        StartCoroutine(nameof(StartCountdown));
    }

    private IEnumerator StartCountdown()
    {
        while (image.fillAmount > 0)
        {
            image.fillAmount -= amountToSubtract;
            yield return new WaitForFixedUpdate();
        }
    }
}
