
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Cameras Dependencies")]
    [SerializeReference] private Animator _cinemachineAnimator;
    [Header("Systems Dependencies")]
    [SerializeField] private InputSystemUIInputModule _inputSystemUI;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameObject _defaultSelectedObject;
    [Header("Main Menu UI Dependencies")]
    [SerializeField] private GameObject _mainMenuTitleObject;
    [SerializeField] private GameObject _pressAnyKeyObject;
    [SerializeField] private GameObject _menuButtonContainer;
    [SerializeField] private Button _newGameButton;
    [SerializeField] private Button _exitGameButton;
    [Header("New Game Dependencies")]
    [SerializeField] private TextMeshProUGUI _countDownText;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private int _nextLevelCountDown = 5;

    private bool _isFirstPlayerSpawned;

    private int _currentAmountOfPlayers;
    private int _amountOfPlayersReady;
    private int _currentTimer;

    private Coroutine _countDown;

    private void Awake()
    {
        _currentTimer = _nextLevelCountDown;
        _currentAmountOfPlayers = 0;
        _amountOfPlayersReady = 0;
    }

    private void Start()
    {
        GlobalPlayersManager.Instance.ClearPlayersList();
        GlobalPlayersManager.Instance.EnablePlayersJoin();
        GlobalPlayersManager.Instance.SubscribeToNewPlayersEvent();
        GlobalPlayersManager.Instance.OnNewPlayerAdded += OnFirstPlayerAdded;
    }

    private void OnDestroy()
    {
        UIPlayerMainMenu.IsPlayerReady -= PlayerReady;
        GlobalPlayersManager.Instance.OnNewPlayerAdded -= ManageNewPlayer;
        GlobalPlayersManager.Instance.OnNewPlayerAdded -= OnFirstPlayerAdded;
    }

    private void OnFirstPlayerAdded(PlayerInput playerInput)
    {
        _pressAnyKeyObject.SetActive(false);

        GlobalPlayersManager.Instance.DisablePlayersJoin();
        GlobalPlayersManager.Instance.UnsubscribeToNewPlayersEvent();
        GlobalPlayersManager.Instance.OnNewPlayerAdded -= OnFirstPlayerAdded;

        _currentAmountOfPlayers++;
        ShowMenuButtons(playerInput);
    }

    private void ShowMenuButtons(PlayerInput playerInput)
    {
        _pressAnyKeyObject.SetActive(false);

        _eventSystem.SetSelectedGameObject(_defaultSelectedObject);
        _inputSystemUI.actionsAsset = playerInput.actions;

        _menuButtonContainer.SetActive(true);

        _newGameButton.onClick.AddListener(OnNewGameSelected);
        _exitGameButton.onClick.AddListener(ExitGame);
    }

    private void OnNewGameSelected()
    {
        _mainMenuTitleObject.SetActive(false);
        _menuButtonContainer.SetActive(false);

        if (!_isFirstPlayerSpawned) SpawnFirstPlayer();
        SwitchCamera();
        if (_isFirstPlayerSpawned) GlobalPlayersManager.Instance.SwitchAllPlayersToUiInput();

        GlobalPlayersManager.Instance.EnablePlayersJoin();
        GlobalPlayersManager.Instance.SubscribeToNewPlayersEvent();
        GlobalPlayersManager.Instance.OnNewPlayerAdded += ManageNewPlayer;

        UIPlayerMainMenu.IsPlayerReady += PlayerReady;
        UIPlayerMainMenu.OnAnyPlayerUIBackTriggered += OnReturnFromNewGame;
    }

    private void ManageNewPlayer(PlayerInput playerInput)
    {
        StopCountDownToLoadNextLevel();
        SpawnPlayerIntoPosition(playerInput.gameObject);
        _currentAmountOfPlayers++;
    }

    private void OnReturnFromNewGame(PlayerInput playerInput)
    {
        GlobalPlayersManager.Instance.DisablePlayersJoin();
        UIPlayerMainMenu.IsPlayerReady -= PlayerReady;
        UIPlayerMainMenu.OnAnyPlayerUIBackTriggered -= OnReturnFromNewGame;
        GlobalPlayersManager.Instance.OnNewPlayerAdded -= ManageNewPlayer;

        GlobalPlayersManager.Instance.UnsubscribeToNewPlayersEvent();
        GlobalPlayersManager.Instance.SwitchAllPlayersToEmptyInput();
        GlobalPlayersManager.Instance.SwitchPlayerActionMap(playerInput, "UI");

        _eventSystem.SetSelectedGameObject(_defaultSelectedObject);
        _inputSystemUI.actionsAsset = GlobalPlayersManager.Instance.GetPlayerInputs[0].actions;

        _mainMenuTitleObject.SetActive(true);
        _menuButtonContainer.SetActive(true);

        SwitchCamera();
    }

    private void SpawnFirstPlayer()
    {
        GlobalPlayersManager.Instance.GetPlayerInputs[0].transform.position = _spawnPoints[0].position;
        _isFirstPlayerSpawned = true;
    }

    private void SpawnPlayerIntoPosition(GameObject player)
    {
        player.transform.position = _spawnPoints[_currentAmountOfPlayers].position;
    }

    private void PlayerReady(bool isReady)
    {
        if (isReady)
        {
            _amountOfPlayersReady++;
            if (_amountOfPlayersReady == _currentAmountOfPlayers)
            {
                StartCountDownToLoadNextLevel();
            }
        }
        else
        {
            if (_amountOfPlayersReady == 0) return;

            _amountOfPlayersReady--;
            StopCountDownToLoadNextLevel();
            _currentTimer = _nextLevelCountDown;
        }
    }

    private void StartCountDownToLoadNextLevel()
    {
        _countDown = StartCoroutine(nameof(StartContDown));
        EnableCountDownText();
        UpdateCountDownText();
    }
    private void StopCountDownToLoadNextLevel()
    {
        if (_countDown != null) StopCoroutine(_countDown);
        _currentTimer = _nextLevelCountDown;
        UpdateCountDownText();
        DisableCountDownText();
    }

    private IEnumerator StartContDown()
    {
        while (_currentTimer > 0)
        {
            yield return new WaitForSeconds(1);
            _currentTimer--;
            UpdateCountDownText();
        }
        LoadNextLevel();
    }

    private void SwitchCamera()
    {
        _cinemachineAnimator.SetTrigger("Switch");
    }

    private void LoadNextLevel()
    {
        GlobalPlayersManager.Instance.DisablePlayersJoin();
        GlobalPlayersManager.Instance.UnsubscribeToNewPlayersEvent();
        GlobalPlayersManager.Instance.DisableAllPlayersGameplayComponents();
        GlobalPlayersManager.Instance.DisableAllPlayersVisuals();
        GlobalPlayersManager.Instance.DisableAllPlayersUIs();

        int amountOfLevels = SceneManager.sceneCountInBuildSettings;
        int lextLevelIndex = UnityEngine.Random.Range(1, amountOfLevels);
        SceneManager.LoadScene(lextLevelIndex);
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

    private void EnableCountDownText() => _countDownText.enabled = true;
    private void DisableCountDownText() => _countDownText.enabled = false;
    private void UpdateCountDownText() => _countDownText.text = $"Start in: {_currentTimer}";
}
