using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuScreen : MonoBehaviour
{
    [Header("Systems Dependencies")]
    [SerializeField] private InputSystemUIInputModule _inputSystemUI;
    [SerializeField] private EventSystem _eventSystem;
    [Header("UI Dependencies")]
    [Header("Objects")]
    [SerializeField] private GameObject _pauseMenuObject;
    [SerializeField] private GameObject _defaultSelectedObject;
    [SerializeField] private GameObject _playerWonScreenObject;
    [Header("Buttons")]
    [SerializeField] private ButtonEventEmitter _resumeButton;
    [SerializeField] private ButtonEventEmitter _mainMenuButton;
    [SerializeField] private ButtonEventEmitter _exitGameButton;
    [SerializeField] private ButtonEventEmitter _currentBackButton;
    [SerializeField] private ButtonEventEmitter _nonFunctionalButton;


    private bool _isGamePaused;

    private PlayerInput _currentPlayer;

    private void Awake()
    {
        PlayerPauseController.OnPauseGame += OnPauseGame;

        _resumeButton.onClick.AddListener(ResumeGame);
        _mainMenuButton.onClick.AddListener(LoadMainMenu);
        _exitGameButton.onClick.AddListener(ExitGame);
        UIPlayerMainMenu.OnAnyPlayerUIBackTriggered += OnAnyPlayerBacked;
    }

    private void OnDestroy()
    {
        PlayerPauseController.OnPauseGame -= OnPauseGame;

        _resumeButton.onClick.RemoveAllListeners();
        _mainMenuButton.onClick.RemoveAllListeners();
        _exitGameButton.onClick.RemoveAllListeners();
        UIPlayerMainMenu.OnAnyPlayerUIBackTriggered -= OnAnyPlayerBacked;
    }

    private void OnPauseGame(PlayerInput playerInput)
    {
        if (_playerWonScreenObject.activeSelf) return;

        _currentPlayer = playerInput;
        if (_isGamePaused) UnPauseGame(playerInput);
        else PauseGame(playerInput);
    }

    private void OnAnyPlayerBacked(PlayerInput playerInput)
    {
        if (_currentBackButton)
        {
            _currentBackButton.onClick.Invoke();
        }
    }

    private void ResumeGame()
    {
        UnPauseGame(_currentPlayer);
    }
    private void LoadMainMenu()
    {
        GlobalPlayersManager.Instance.SetAllPlayersDefaultTurret();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
        BackgroundMusicManager.Instance.PlayMenuTheme();
    }
    private void ExitGame()
    {
        GlobalPlayersManager.Instance.SetAllPlayersDefaultTurret();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void UnPauseGame(PlayerInput playerInput)
    {
        _currentBackButton = _nonFunctionalButton;
        _eventSystem.SetSelectedGameObject(null);

        BackgroundMusicManager.Instance.ResumeMusic();
        GlobalPlayersManager.Instance.SwitchPlayerActionMap(playerInput, "Player");
        _pauseMenuObject.SetActive(false);
        Time.timeScale = 1.0f;
        _isGamePaused = false;
    }

    private void PauseGame(PlayerInput playerInput)
    {
        UiSfxManager.Instance.ShouldPlayButtonsSounds = false;
        UiSfxManager.Instance.ShouldPlaySelectedSounds = false;

        BackgroundMusicManager.Instance.PauseMusic();
        UiSfxManager.Instance.PlayPauseMenuClip();
        Time.timeScale = 0.0f;
        playerInput.enabled = true;

        GlobalPlayersManager.Instance.SwitchPlayerActionMap(playerInput, "UI");
        _inputSystemUI.actionsAsset = playerInput.actions;
        _isGamePaused = true;
        _pauseMenuObject.SetActive(true);

        _eventSystem.SetSelectedGameObject(null);
        if (playerInput.currentControlScheme == "Gamepad")
        {
            _eventSystem.SetSelectedGameObject(_defaultSelectedObject);
            UiSfxManager.Instance.ShouldPlaySelectedSounds = true;
        }

        StartCoroutine(WaitForButtonSelectionToFinish());
        _currentBackButton = _resumeButton;
    }

    private IEnumerator WaitForButtonSelectionToFinish()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        UiSfxManager.Instance.ShouldPlayButtonsSounds = true;
    }

    public void SetSelectedGameObject(GameObject gameObject)
    {
        _eventSystem.SetSelectedGameObject(null);
        if (_currentPlayer.currentControlScheme == "Gamepad")
        {
            _eventSystem.SetSelectedGameObject(gameObject);
        }
    }

    public void SetCurrentBackButton(ButtonEventEmitter button)
    {
        _currentBackButton = button;
    }
}
