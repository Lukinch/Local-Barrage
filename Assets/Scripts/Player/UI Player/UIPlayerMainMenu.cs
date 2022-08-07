
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerMainMenu : MonoBehaviour
{
    [SerializeField] private Button _readyButton;
    [SerializeField] private Image _background;

    public static event Action<bool> IsPlayerReady;

    private bool isReady = false;

    private void OnEnable()
    {
        _readyButton.onClick.AddListener(TogglePlayerReady);
    }
    private void OnDisable()
    {
        _readyButton.onClick.RemoveListener(TogglePlayerReady);
    }

    private void TogglePlayerReady()
    {
        isReady = !isReady;
        UpdateReadyVisuals();
        IsPlayerReady?.Invoke(isReady);
    }

    private void UpdateReadyVisuals()
    {
        _background.color = isReady ? Color.green : Color.white;
    }
}
