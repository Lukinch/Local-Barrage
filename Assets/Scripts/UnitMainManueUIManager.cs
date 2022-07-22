using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMainManueUIManager : MonoBehaviour
{
    [SerializeField] Button readyButton;
    [SerializeField] Image background;

    public static event Action<bool> isPlayerReady;

    private bool isReady = false;

    private void OnEnable()
    {
        readyButton.onClick.AddListener(TogglePlayerReady);
    }
    private void OnDisable()
    {
        readyButton.onClick.RemoveListener(TogglePlayerReady);
    }

    private void TogglePlayerReady()
    {
        isReady = !isReady;
        UpdateReadyVisuals();
        isPlayerReady?.Invoke(isReady);
    }

    private void UpdateReadyVisuals()
    {
        background.color = isReady ? Color.green : Color.white;
    }
}
