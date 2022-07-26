using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    private LevelPlayersManager levelManager;
    private List<PlayerInput> players = new List<PlayerInput>();

    private List<PlayerMoveController> playerMoveControllers = new List<PlayerMoveController>();
    private List<PlayerTurretRotationController> playerRotationControllers = new List<PlayerTurretRotationController>();
    private List<KeepInPlacePosition> playerKeepInPlaceControllers = new List<KeepInPlacePosition>();
    private List<TurretBase> playerFiringControllers = new List<TurretBase>();
    private List<GameObject> playerLiveUI = new List<GameObject>();
    private List<GameObject> playerShieldColliders = new List<GameObject>();
    private List<GameObject> palyerMainMenuUI = new List<GameObject>();

    private int currentAmountOfPlayers;
    private int amountOfPlayersReady;
    [SerializeField] private int nextLevelCountDown = 5;

    [SerializeField] private TextMeshProUGUI countDownText;

    private int currentTimer;

    private Coroutine countDown;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelPlayersManager>();

        levelManager.OnPlayerAdded += ManageNewPlayer;

        UIUnitMainMenuManager.isPlayerReady += PlayerReady;

        currentTimer = nextLevelCountDown;
    }

    private void OnDisable()
    {
        levelManager.OnPlayerAdded -= ManageNewPlayer;
    }

    private void ManageNewPlayer(PlayerInput playerInput)
    {
        StopCountDownToLoadNextLevel();
        playerInput.SwitchCurrentActionMap("UI");
        players.Add(playerInput);
        AddComponentsToTheLists(playerInput);
        DisablePlayerInncesaryComponents();
        currentAmountOfPlayers++;
    }

    private void AddComponentsToTheLists(PlayerInput playerInput)
    {
        playerMoveControllers.Add(playerInput.gameObject.GetComponent<PlayerMoveController>());
        palyerMainMenuUI.Add(playerInput.gameObject.GetComponentInChildren<UIUnitMainMenuManager>().gameObject);
        playerKeepInPlaceControllers.Add(playerInput.gameObject.GetComponentInChildren<KeepInPlacePosition>());
        playerLiveUI.Add(playerInput.gameObject.GetComponentInChildren<Billboard>().gameObject);
        playerShieldColliders.Add(playerInput.gameObject.GetComponentInChildren<PlayerShieldCollision>().gameObject);
        playerRotationControllers.Add(playerInput.gameObject.GetComponentInChildren<PlayerTurretRotationController>());
        playerFiringControllers.Add(playerInput.gameObject.GetComponentInChildren<TurretBase>());
    }

    private void DisablePlayerInncesaryComponents()
    {
        playerMoveControllers[currentAmountOfPlayers].enabled = false;
        playerRotationControllers[currentAmountOfPlayers].enabled = false;
        playerKeepInPlaceControllers[currentAmountOfPlayers].enabled = false;
        playerLiveUI[currentAmountOfPlayers].SetActive(false);
        playerFiringControllers[currentAmountOfPlayers].enabled = false;
        playerShieldColliders[currentAmountOfPlayers].SetActive(false);
        palyerMainMenuUI[currentAmountOfPlayers].SetActive(true);
    }

    private void EnableAllPlayerInncesaryComponents()
    {
        for (var i = 0; i < players.Count; i++)
        {
            players[i].SwitchCurrentActionMap("Player");
            playerMoveControllers[i].enabled = true;
            playerRotationControllers[i].enabled = true;
            playerKeepInPlaceControllers[i].enabled = true;
            playerLiveUI[i].SetActive(true);
            playerFiringControllers[i].enabled = true;
            playerShieldColliders[i].SetActive(true);
            palyerMainMenuUI[i].SetActive(false);
        }
    }

    private void PlayerReady(bool isReady)
    {
        if (isReady)
        {
            amountOfPlayersReady++;
            if (amountOfPlayersReady == players.Count)
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

    private void UpdateCountDownText()
    {
        countDownText.text = $"Start in: {currentTimer}";
    }

    private void LoadNextLevel()
    {
        EnableAllPlayerInncesaryComponents();
        int amountOfLevels = SceneManager.sceneCountInBuildSettings;
        int lextLevelIndex = UnityEngine.Random.Range(1, amountOfLevels);
        SceneManager.LoadScene(lextLevelIndex);
    }
}
