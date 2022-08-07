
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private int _nextLevelCountDown = 5;
    [SerializeField] private TextMeshProUGUI _countDownText;
    [SerializeField] private List<Transform> _spawnPoints;

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

        UIPlayerMainMenu.IsPlayerReady += PlayerReady;
        GlobalPlayersManager.Instance.OnNewPlayerAdded += ManageNewPlayer;
    }

    private void OnDestroy()
    {
        UIPlayerMainMenu.IsPlayerReady -= PlayerReady;
        GlobalPlayersManager.Instance.OnNewPlayerAdded -= ManageNewPlayer;
    }

    private void ManageNewPlayer(PlayerInput playerInput)
    {
        StopCountDownToLoadNextLevel();
        SpawnPlayerIntoPosition(playerInput.gameObject);
        _currentAmountOfPlayers++;
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

    private void EnableCountDownText() => _countDownText.enabled = true;
    private void DisableCountDownText() => _countDownText.enabled = false;
    private void UpdateCountDownText() => _countDownText.text = $"Start in: {_currentTimer}";

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
}
