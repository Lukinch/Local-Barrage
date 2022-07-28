
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitMainMenuManager : MonoBehaviour
{
    [SerializeField] Button readyButton;
    [SerializeField] Image background;

    public static event Action<bool> IsPlayerReady;

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
        IsPlayerReady?.Invoke(isReady);
    }

    private void UpdateReadyVisuals()
    {
        background.color = isReady ? Color.green : Color.white;
    }
}
