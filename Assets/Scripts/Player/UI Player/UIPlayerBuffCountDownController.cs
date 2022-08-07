using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerBuffCountDownController : MonoBehaviour
{
    [SerializeField] private PlayerTurretController _turretController;
    [SerializeField] private Image _fillImage;

    private float _maxBuffDuration;
    private float _amountToSubtract;

    private void OnEnable()
    {
        _maxBuffDuration = _turretController.BuffDuration;
        _fillImage.fillAmount = 0;
        // 1 = Max Image Fill Amount, 50 = Fixed Update Calls per second
        _amountToSubtract = (1 / _maxBuffDuration) / 50;
        
        _turretController.OnBuffStarted += OnBuffStarted;
    }

    private void OnDisable()
    {
        _turretController.OnBuffStarted -= OnBuffStarted;
    }

    private void OnBuffStarted()
    {
        _fillImage.fillAmount = 1;
        StartCoroutine(nameof(StartCountdown));
    }

    private IEnumerator StartCountdown()
    {
        while (_fillImage.fillAmount > 0)
        {
            _fillImage.fillAmount -= _amountToSubtract;
            yield return new WaitForFixedUpdate();
        }
    }
}
