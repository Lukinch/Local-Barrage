
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("Cameras Dependencies")]
    [SerializeReference] private Animator _cinemachineAnimator;
    [Header("Systems Dependencies")]
    [SerializeField] private InputSystemUIInputModule _inputSystemUI;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameObject _defaultSelectedObject;
    [Header("Main Menu UI Dependencies")]
    [SerializeField] private Transform _mainMenuObject;
    [SerializeField] private GameObject _pressAnyKeyObject;
    [SerializeField] private GameObject _menuButtonContainer;
    [SerializeField] private ButtonEventEmitter _newGameButton;
    [SerializeField] private ButtonEventEmitter _exitGameButton;
    [SerializeField] private Vector3 _transitionPositionFrom;
    [SerializeField] private Vector3 _transitionPositionTo;
    [SerializeField] private float _transitionDuration = 0.5f;
    [SerializeField] private float _timeBeforeNextTransition = 0.5f;
    [Header("New Game Dependencies")]
    [SerializeField] private TextMeshProUGUI _countDownText;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private int _nextLevelCountDown = 5;

    private GlobalPlayersManager _playersManager;
    private bool _isFirstPlayerSpawned;
    private bool _isTransitioning;

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

    private void OnEnable()
    {
        _playersManager = GlobalPlayersManager.Instance;
        _playersManager.ClearPlayersList();
        _playersManager.EnablePlayersJoin();
        _playersManager.SubscribeToNewPlayersEvent();
        _playersManager.OnFirstPlayerAdded += OnFirstPlayerAdded;
    }

    private void OnDisable()
    {
        _playersManager.OnFirstPlayerAdded -= OnFirstPlayerAdded;
        _playersManager.OnNewPlayerAdded -= OnNewPlayerAdded;

        UIPlayerMainMenu.IsPlayerReady -= PlayerReady;
        UIPlayerMainMenu.OnAnyPlayerUIBackTriggered -= OnAnyPlayerBacked;
    }

    private void OnFirstPlayerAdded(PlayerInput playerInput)
    {
        _playersManager.OnFirstPlayerAdded -= OnFirstPlayerAdded;

        _playersManager.DisablePlayersJoin();
        _playersManager.UnsubscribeToNewPlayersEvent();

        SetupPlayerForMainMenu(playerInput.playerIndex);
        _playersManager.DisablePlayerPersonalEventSystem(playerInput.playerIndex);

        _pressAnyKeyObject.SetActive(false);
        _menuButtonContainer.SetActive(true);

        SetupMainMenuEventSystem(playerInput, "UI");

        _newGameButton.onClick.AddListener(OnNewGameSelected);
        _exitGameButton.onClick.AddListener(OnExitGameSelected);
    }

    private void SetupMainMenuEventSystem(PlayerInput playerInput, string playerActionMap)
    {
        _eventSystem.SetSelectedGameObject(null);
        if (playerInput.currentControlScheme == "Gamepad")
        {
            _eventSystem.SetSelectedGameObject(_defaultSelectedObject);
        }

        _playersManager.SwitchPlayerActionMap(playerInput.playerIndex, playerActionMap);

        _inputSystemUI.actionsAsset = playerInput.actions;
    }

    private void SetupPlayerForMainMenu(int playerIndex)
    {
        _playersManager.MakePlayerKinematic(playerIndex);
        _playersManager.DisablePlayerGameplayComponents(playerIndex);
        _playersManager.EnablePlayerMenuUI(playerIndex);
        _playersManager.EnablePlayerPersonalEventSystem(playerIndex);
        _playersManager.SetupPlayerPersonalEventSystem(playerIndex);
        SetPlayerPosition(playerIndex);
        _currentAmountOfPlayers++;
    }

    private void SetPlayerPosition(int playerIndex)
    {
        _playersManager.GetPlayerInputs[playerIndex].transform.position = 
            _spawnPoints[playerIndex].position;
    }

    private void OnNewGameSelected()
    {
        if (_isTransitioning) return;

        StartCoroutine(TransitionCooldown());
        StartCoroutine(LerpPosition(_mainMenuObject, _transitionPositionFrom, _transitionPositionTo, _transitionDuration));

        _newGameButton.onClick.RemoveListener(OnNewGameSelected);
        _exitGameButton.onClick.RemoveListener(OnExitGameSelected);

        _playersManager.EnablePlayersJoin();
        _playersManager.SubscribeToNewPlayersEvent();
        
        _playersManager.OnNewPlayerAdded += OnNewPlayerAdded;

        _eventSystem.gameObject.SetActive(false);

        SwitchCamera();

        _playersManager.EnableAllPlayersPersonalEventSystem();
        _playersManager.SetupAllPlayersPersonalEventSystem();

        UIPlayerMainMenu.IsPlayerReady += PlayerReady;
        UIPlayerMainMenu.OnAnyPlayerUIBackTriggered += OnAnyPlayerBacked;

        if (_amountOfPlayersReady == _currentAmountOfPlayers)
        {
            StartCountDownToLoadNextLevel();
        }
    }

    private void OnAnyPlayerBacked(PlayerInput playerInput)
    {
        if (_isTransitioning) return;

        StopCountDownToLoadNextLevel();

        _playersManager.OnNewPlayerAdded -= OnNewPlayerAdded;
        UIPlayerMainMenu.IsPlayerReady -= PlayerReady;
        UIPlayerMainMenu.OnAnyPlayerUIBackTriggered -= OnAnyPlayerBacked;

        _playersManager.DisablePlayersJoin();
        _playersManager.UnsubscribeToNewPlayersEvent();

        StartCoroutine(nameof(TransitionCooldown));
        StartCoroutine(LerpPosition(_mainMenuObject, _transitionPositionTo, _transitionPositionFrom, _transitionDuration));

        _playersManager.DisableAllPlayersPersonalEventSystem();

        _eventSystem.gameObject.SetActive(true);

        SetupMainMenuEventSystem(playerInput, "UI");

        SwitchCamera();

        _newGameButton.onClick.AddListener(OnNewGameSelected);
        _exitGameButton.onClick.AddListener(OnExitGameSelected);
    }

    private void OnNewPlayerAdded(PlayerInput playerInput)
    {
        SetupPlayerForMainMenu(playerInput.playerIndex);
    }

    private void SwitchCamera()
    {
        _cinemachineAnimator.SetTrigger("Switch");
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

    private IEnumerator TransitionCooldown()
    {
        _isTransitioning = true;
        yield return new WaitForSeconds(_timeBeforeNextTransition);
        _isTransitioning = false;
    }
    private IEnumerator LerpPosition(Transform transform ,Vector3 StartPos, Vector3 EndPos, float LerpTime)
    {
        float StartTime = Time.time;
        float EndTime = StartTime + LerpTime;

        while (Time.time < EndTime)
        {
            float timeProgressed = (Time.time - StartTime) / LerpTime;  // this will be 0 at the beginning and 1 at the end.
            transform.position = Vector3.Lerp(StartPos, EndPos, timeProgressed);

            yield return new WaitForFixedUpdate();
        }
    }

    private void LoadNextLevel()
    {
        _playersManager.DisablePlayersJoin();
        _playersManager.UnsubscribeToNewPlayersEvent();
        _playersManager.DisableAllPlayersGameplayComponents();
        _playersManager.DisableAllPlayersVisuals();
        _playersManager.DisableAllPlayersUIs();

        int amountOfLevels = SceneManager.sceneCountInBuildSettings;
        int lextLevelIndex = UnityEngine.Random.Range(1, amountOfLevels);
        SceneManager.LoadScene(lextLevelIndex);
    }

    private void OnExitGameSelected()
    {
        _playersManager.SetAllPlayersDefaultTurret();
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
