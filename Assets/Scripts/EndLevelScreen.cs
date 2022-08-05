using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class EndLevelScreen : MonoBehaviour
{
    [Header("Level Dependencies")]
    [SerializeField] private LevelPlayersManager levelPlayersManager;
    [Header("------------------")]
    [Header("UI Dependencies")]
    [Header("Next Level UI")]
    [SerializeField] private GameObject nextLevelScreenObject;
    [SerializeField] private TextMeshProUGUI nextLevelTitleText;
    [SerializeField] private List<TextMeshProUGUI> nextLevelPlayersScoreTexts;
    [SerializeField] private TextMeshProUGUI countdownText;
    [Header("Player Won UI")]
    [SerializeField] private GameObject playerWonScreenObject;
    [SerializeField] private TextMeshProUGUI playerWonTitleText;
    [SerializeField] private List<TextMeshProUGUI> playerWonPlayersScoreTexts;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitGameButton;
    [Header("------------------")]
    [Header("Settings")]
    [SerializeField] private int timeTillNextlevel;

    private int currentTime;

    private void Awake()
    {
        currentTime = timeTillNextlevel;
        levelPlayersManager.OnLevelEnded += OnLevelEnded;

        for (int i = 0; i < playerWonPlayersScoreTexts.Count; i++)
        {
            playerWonPlayersScoreTexts[i].gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        levelPlayersManager.OnLevelEnded -= OnLevelEnded;
    }

    private void OnLevelEnded()
    {
        int[] playersPoints = GlobalPlayersManager.Instance.GetPlayerPointsInt();

        int maxValue = playersPoints.Max();

        if (maxValue >= 50)
        {
            OnPlayerWon(playersPoints.ToList().IndexOf(maxValue), playersPoints);
        }
        else
        {
            OnNextLevel(playersPoints);
        }
    }

    private void OnNextLevel(int[] playersPoints)
    {
        for (int i = 0; i < playersPoints.Length; i++)
        {
            nextLevelPlayersScoreTexts[i].text = $"Player {i + 1} Points: {playersPoints[i]}";
            nextLevelPlayersScoreTexts[i].gameObject.SetActive(true);
        }

        int lastPlayerIndex = GlobalPlayersManager.Instance.GetPlayerInputs.FindIndex(
            player => player.gameObject.activeInHierarchy);

        nextLevelTitleText.text = $"Player {lastPlayerIndex + 1} Won This Round";

        nextLevelScreenObject.SetActive(true);

        GlobalPlayersManager.Instance.SetPlayersDefaultTurret();
        GlobalPlayersManager.Instance.DisableAllPlayers();

        StartCoroutine(nameof(NextLevelCountdown));
    }

    private void OnPlayerWon(int playerIndex, int[] playersPoints)
    {
        for (int i = 0; i < playersPoints.Length; i++)
        {
            playerWonPlayersScoreTexts[i].text = $"Player {i + 1} Points: {playersPoints[i]}";
            playerWonPlayersScoreTexts[i].gameObject.SetActive(true);
        }

        playerWonTitleText.text = $"Player {playerIndex + 1} Won The Game!";

        mainMenuButton.onClick.AddListener(LoadMainMenu);
        exitGameButton.onClick.AddListener(ExitGame);

        GlobalPlayersManager.Instance.EnableAllPlayers();
        GlobalPlayersManager.Instance.SwitchAllPlayersActionMap("UI");

        playerWonScreenObject.SetActive(true);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
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
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            countdownText.text = $"Next Level Starts In: {currentTime}";
        }
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        int amountOfLevels = SceneManager.sceneCountInBuildSettings;
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        int lextLevelIndex = Random.Range(1, amountOfLevels);
        while (lextLevelIndex == currentLevelIndex)
        {
            lextLevelIndex = Random.Range(1, amountOfLevels);
        }

        SceneManager.LoadScene(lextLevelIndex);
    }
}
