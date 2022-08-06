
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private int nextLevelCountDown = 5;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private List<Transform> spawnPoints;

    private int currentAmountOfPlayers;
    private int amountOfPlayersReady;
    private int currentTimer;

    private Coroutine countDown;

    private void Awake()
    {
        currentTimer = nextLevelCountDown;
        currentAmountOfPlayers = 0;
        amountOfPlayersReady = 0;
    }

    private void Start()
    {
        GlobalPlayersManager.Instance.ClearPlayersList();
        GlobalPlayersManager.Instance.EnablePlayersJoin();
        GlobalPlayersManager.Instance.SubscribeToNewPlayersEvent();

        UIUnitMainMenuManager.IsPlayerReady += PlayerReady;
        GlobalPlayersManager.Instance.OnNewPlayerAdded += ManageNewPlayer;
    }

    private void OnDestroy()
    {
        UIUnitMainMenuManager.IsPlayerReady -= PlayerReady;
        GlobalPlayersManager.Instance.OnNewPlayerAdded -= ManageNewPlayer;
    }

    private void ManageNewPlayer(PlayerInput playerInput)
    {
        StopCountDownToLoadNextLevel();
        SpawnPlayerIntoPosition(playerInput.gameObject);
        currentAmountOfPlayers++;
    }

    private void SpawnPlayerIntoPosition(GameObject player)
    {
        player.transform.position = spawnPoints[currentAmountOfPlayers].position;
    }

    private void PlayerReady(bool isReady)
    {
        if (isReady)
        {
            amountOfPlayersReady++;
            if (amountOfPlayersReady == currentAmountOfPlayers)
            {
                StartCountDownToLoadNextLevel();
            }
        }
        else
        {
            if (amountOfPlayersReady == 0) return;

            amountOfPlayersReady--;
            StopCountDownToLoadNextLevel();
            currentTimer = nextLevelCountDown;
        }
    }

    private void StartCountDownToLoadNextLevel()
    {
        countDown = StartCoroutine(nameof(StartContDown));
        EnableCountDownText();
        UpdateCountDownText();
    }
    private void StopCountDownToLoadNextLevel()
    {
        if (countDown != null) StopCoroutine(countDown);
        currentTimer = nextLevelCountDown;
        UpdateCountDownText();
        DisableCountDownText();
    }

    private IEnumerator StartContDown()
    {
        while (currentTimer > 0)
        {
            yield return new WaitForSeconds(1);
            currentTimer--;
            UpdateCountDownText();
        }
        LoadNextLevel();
    }

    private void EnableCountDownText() => countDownText.enabled = true;
    private void DisableCountDownText() => countDownText.enabled = false;
    private void UpdateCountDownText() => countDownText.text = $"Start in: {currentTimer}";

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
