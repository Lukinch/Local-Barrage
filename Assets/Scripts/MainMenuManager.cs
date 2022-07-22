using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    private LevelManager levelManager;
    private List<PlayerInput> players = new List<PlayerInput>();

    private List<PlayerMoveController> playerMoveControllers = new List<PlayerMoveController>();
    private List<PlayerTurretRotationController> playerRotationControllers = new List<PlayerTurretRotationController>();
    private List<KeepInPlace> playerKeepInPlaceControllers = new List<KeepInPlace>();
    private List<TurretBase> playerFiringControllers = new List<TurretBase>();
    private List<GameObject> playerHullColliders = new List<GameObject>();
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
        levelManager = FindObjectOfType<LevelManager>();

        levelManager.OnPlayerAdded += ManageNewPlayer;

        UnitMainManueUIManager.isPlayerReady += PlayerReady;

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
        playerRotationControllers.Add(playerInput.gameObject.GetComponent<PlayerTurretRotationController>());
        playerKeepInPlaceControllers.Add(playerInput.gameObject.GetComponentInChildren<KeepInPlace>());
        playerFiringControllers.Add(playerInput.gameObject.GetComponentInChildren<TurretBase>());
        playerHullColliders.Add(playerInput.gameObject.GetComponentInChildren<UnitHullCollision>().gameObject);
        playerShieldColliders.Add(playerInput.gameObject.GetComponentInChildren<UnitShieldCollision>().gameObject);
        palyerMainMenuUI.Add(playerInput.gameObject.GetComponentInChildren<UnitMainManueUIManager>().gameObject);
    }

    private void DisablePlayerInncesaryComponents()
    {
        playerMoveControllers[currentAmountOfPlayers].enabled = false;
        playerRotationControllers[currentAmountOfPlayers].enabled = false;
        playerKeepInPlaceControllers[currentAmountOfPlayers].enabled = false;
        playerFiringControllers[currentAmountOfPlayers].enabled = false;
        playerHullColliders[currentAmountOfPlayers].SetActive(false);
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
            playerFiringControllers[i].enabled = true;
            playerHullColliders[i].SetActive(true);
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
