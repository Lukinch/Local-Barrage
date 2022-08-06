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
    [SerializeField] private InputSystemUIInputModule inputSystemUI;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject pauseMenuObject;
    [SerializeField] private GameObject defaultSelectedObject;
    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitGameButton;


    private bool isGamePaused;

    private PlayerInput currentPlayer;

    private void Awake()
    {
        PlayerPauseController.OnPauseGame += OnPauseGame;

        resumeButton.onClick.AddListener(ResumeGame);
        //settingsButton.onClick.AddListener();
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        exitGameButton.onClick.AddListener(ExitGame);
    }

    private void OnDestroy()
    {
        PlayerPauseController.OnPauseGame -= OnPauseGame;

        resumeButton.onClick.RemoveAllListeners();
        //settingsButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        exitGameButton.onClick.RemoveAllListeners();
    }

    private void OnPauseGame(PlayerInput playerInput)
    {
        currentPlayer = playerInput;
        if (isGamePaused) UnPauseGame(playerInput);
        else PauseGame(playerInput);
    }

    private void ResumeGame()
    {
        UnPauseGame(currentPlayer);
    }
    private void LoadMainMenu()
    {
        GlobalPlayersManager.Instance.SetPlayersDefaultTurret();
        SceneManager.LoadScene(0);
    }
    private void ExitGame()
    {
        GlobalPlayersManager.Instance.SetPlayersDefaultTurret();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void UnPauseGame(PlayerInput playerInput)
    {
        GlobalPlayersManager.Instance.SwitchPlayerActionMap(playerInput, "Player");
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1.0f;
        isGamePaused = false;
    }

    private void PauseGame(PlayerInput playerInput)
    {
        Time.timeScale = 0.0f;
        playerInput.enabled = true;
        eventSystem.SetSelectedGameObject(defaultSelectedObject);
        GlobalPlayersManager.Instance.SwitchPlayerActionMap(playerInput, "UI");
        inputSystemUI.actionsAsset = playerInput.actions;
        isGamePaused = true;
        pauseMenuObject.SetActive(true);
    }
}
