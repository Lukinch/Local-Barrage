
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIPlayerMainMenu : MonoBehaviour
{
    [Header("Event System")]
    [SerializeField] private MultiplayerEventSystem _eventSystem;
    [SerializeField] private InputSystemUIInputModule _inputSystemUI;
    [SerializeField] private GameObject _defaultSelectedObject;
    [Header("Player Input")]
    [SerializeField] private UnityEngine.InputSystem.PlayerInput _playerInput;
    [Header("UI Items")]
    [SerializeField] private Button _readyButton;
    [SerializeField] private Image _background;

    private bool _isReady = false;
    private bool _eventSystemActive;

    public static event Action<bool> IsPlayerReady;
    public static event Action<UnityEngine.InputSystem.PlayerInput> OnAnyPlayerUIBackTriggered;

    private void OnEnable()
    {
        //DisableEventSystem();
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

    public void SetupEventSystem()
    {
        if (_playerInput.currentControlScheme == "Gamepad")
            _eventSystem.SetSelectedGameObject(_defaultSelectedObject);
        else
            _eventSystem.SetSelectedGameObject(null);
    }

    public void EnableEventSystem()
    {
        _eventSystem.gameObject.SetActive(true);
        _readyButton.onClick.AddListener(TogglePlayerReady);
    }
    public void DisableEventSystem()
    {
        _readyButton.onClick.RemoveListener(TogglePlayerReady);
        _eventSystem.gameObject.SetActive(false);
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.performed) OnAnyPlayerUIBackTriggered?.Invoke(_playerInput);
    }
}
