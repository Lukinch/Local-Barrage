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
    [Header("Buttons")]
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _exitGameButton;


    private bool _isGamePaused;

    private PlayerInput _currentPlayer;

    private void Awake()
    {
        PlayerPauseController.OnPauseGame += OnPauseGame;

        _resumeButton.onClick.AddListener(ResumeGame);
        //settingsButton.onClick.AddListener();
        _mainMenuButton.onClick.AddListener(LoadMainMenu);
        _exitGameButton.onClick.AddListener(ExitGame);
    }

    private void OnDestroy()
    {
        PlayerPauseController.OnPauseGame -= OnPauseGame;

        _resumeButton.onClick.RemoveAllListeners();
        //settingsButton.onClick.RemoveAllListeners();
        _mainMenuButton.onClick.RemoveAllListeners();
        _exitGameButton.onClick.RemoveAllListeners();
    }

    private void OnPauseGame(PlayerInput playerInput)
    {
        _currentPlayer = playerInput;
        if (_isGamePaused) UnPauseGame(playerInput);
        else PauseGame(playerInput);
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
        GlobalPlayersManager.Instance.SwitchPlayerActionMap(playerInput, "Player");
        _pauseMenuObject.SetActive(false);
        Time.timeScale = 1.0f;
        _isGamePaused = false;
    }

    private void PauseGame(PlayerInput playerInput)
    {
        Time.timeScale = 0.0f;
        playerInput.enabled = true;

        _eventSystem.SetSelectedGameObject(null);
        if (playerInput.currentControlScheme == "Gamepad")
            _eventSystem.SetSelectedGameObject(_defaultSelectedObject);

        GlobalPlayersManager.Instance.SwitchPlayerActionMap(playerInput, "UI");
        _inputSystemUI.actionsAsset = playerInput.actions;
        _isGamePaused = true;
        _pauseMenuObject.SetActive(true);
    }
}
