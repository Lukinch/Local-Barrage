
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIPlayerMainMenu : MonoBehaviour
{
    [SerializeField] private UnityEngine.InputSystem.PlayerInput _playerInput;
    [SerializeField] private Button _readyButton;
    [SerializeField] private Image _background;

    private bool _isReady = false;

    public static event Action<bool> IsPlayerReady;
    public static event Action<UnityEngine.InputSystem.PlayerInput> OnAnyPlayerUIBackTriggered;

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
        _isReady = !_isReady;
        UpdateReadyVisuals();
        IsPlayerReady?.Invoke(_isReady);
    }

    private void UpdateReadyVisuals()
    {
        _background.color = _isReady ? Color.green : Color.white;
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        OnAnyPlayerUIBackTriggered?.Invoke(_playerInput);
    }
}
