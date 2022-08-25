using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem;

public class EndLevelScreen : MonoBehaviour
{
    [Header("Settings Dependency")]
    [SerializeField] private GameplaySettingsSO _gameplaySettings;

    [Header("Level Dependencies")]
    [SerializeField] private LevelPlayersManager _levelPlayersManager;
    [SerializeField] private InputSystemUIInputModule _inputSystemUI;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameObject _defaultSelectedObject;
    [Header("------------------")]
    [Header("UI Dependencies")]
    [Header("Next Level UI")]
    [SerializeField] private GameObject _nextLevelScreenObject;
    [SerializeField] private TextMeshProUGUI _nextLevelTitleText;
    [SerializeField] private List<TextMeshProUGUI> _nextLevelPlayersScoreTexts;
    [SerializeField] private TextMeshProUGUI _countdownText;
    [Header("Player Won UI")]
    [SerializeField] private GameObject _playerWonScreenObject;
    [SerializeField] private TextMeshProUGUI _playerWonTitleText;
    [SerializeField] private List<TextMeshProUGUI> _playerWonPlayersScoreTexts;
    [SerializeField] private ButtonEventEmitter _mainMenuButton;
    [SerializeField] private ButtonEventEmitter _exitGameButton;

    private int _timeTillNextLevel;

    private int _currentTime;

    private void Awake()
    {
        _timeTillNextLevel = _gameplaySettings.TimeTillNextLevel;
        _currentTime = _timeTillNextLevel;
        _countdownText.text = $"Next Level Starts In: {_timeTillNextLevel}";
        _levelPlayersManager.OnLevelEnded += OnLevelEnded;

        for (int i = 0; i < _playerWonPlayersScoreTexts.Count; i++)
        {
            _playerWonPlayersScoreTexts[i].gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        _levelPlayersManager.OnLevelEnded -= OnLevelEnded;
    }

    private void OnLevelEnded()
    {
        BackgroundMusicManager.Instance.StopMusic();

        int lastPlayerIndex = GlobalPlayersManager.Instance.GetLastPlayerStandingIndex();
        GlobalPlayersManager.Instance.AddPointsToPlayer(lastPlayerIndex);

        float[] playersPoints = GlobalPlayersManager.Instance.GetPlayerPointsInt();

        float maxValue = playersPoints.Max();

        if (maxValue >= _gameplaySettings.AmountOfPointsToWin)
        {
            OnPlayerWon(playersPoints.ToList().IndexOf(maxValue), playersPoints);
        }
        else
        {
            OnNextLevel(playersPoints);
        }
    }

    private void OnNextLevel(float[] playersPoints)
    {
        BackgroundMusicManager.Instance.PlayNextLevelClip();
        for (int i = 0; i < playersPoints.Length; i++)
        {
            _nextLevelPlayersScoreTexts[i].text = $"Player {i + 1} Points: {playersPoints[i]}";
            _nextLevelPlayersScoreTexts[i].gameObject.SetActive(true);
        }

        int lastPlayerIndex = GlobalPlayersManager.Instance.GetLastPlayerStandingIndex();

        GlobalPlayersManager.Instance.SetAllPlayersDefaultTurret();
        GlobalPlayersManager.Instance.DisableAllPlayersGameplayComponents();
        GlobalPlayersManager.Instance.DisableAllPlayersVisuals();

        _nextLevelTitleText.text = $"Player {lastPlayerIndex + 1} Won This Round";

        _nextLevelScreenObject.SetActive(true);

        StartCoroutine(nameof(NextLevelCountdown));
    }

    private void OnPlayerWon(int playerIndex, float[] playersPoints)
    {
        BackgroundMusicManager.Instance.PlayPlayerWonClip();
        for (int i = 0; i < playersPoints.Length; i++)
        {
            _playerWonPlayersScoreTexts[i].text = $"Player {i + 1} Points: {playersPoints[i]}";
            _playerWonPlayersScoreTexts[i].gameObject.SetActive(true);
        }

        GlobalPlayersManager.Instance.SetAllPlayersDefaultTurret();
        GlobalPlayersManager.Instance.DisableAllPlayersGameplayComponents();
        GlobalPlayersManager.Instance.DisableAllPlayersVisuals();

        _playerWonTitleText.text = $"Player {playerIndex + 1} Won The Game!";

        _mainMenuButton.onClick.AddListener(LoadMainMenu);
        _exitGameButton.onClick.AddListener(ExitGame);

        PlayerInput winner = GlobalPlayersManager.Instance.GetPlayerInputs[playerIndex];

        winner.enabled = true;

        GlobalPlayersManager.Instance.SwitchPlayerActionMap(winner, "UI");
        _inputSystemUI.actionsAsset = winner.actions;

        _playerWonScreenObject.SetActive(true);

        _eventSystem.SetSelectedGameObject(null);
        if (winner.currentControlScheme == "Gamepad")
        {
            _eventSystem.SetSelectedGameObject(_defaultSelectedObject);
        }
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        BackgroundMusicManager.Instance.PlayMenuTheme();
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator NextLevelCountdown()
    {
        while (_currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            _currentTime--;
            _countdownText.text = $"Next Level Starts In: {_currentTime}";
        }
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        int amountOfLevels = SceneManager.sceneCountInBuildSettings;
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        int nextLevelIndex = Random.Range(1, amountOfLevels);
        while (nextLevelIndex == currentLevelIndex)
        {
            nextLevelIndex = Random.Range(1, amountOfLevels);
        }

        SceneManager.LoadScene(nextLevelIndex);
    }
}
