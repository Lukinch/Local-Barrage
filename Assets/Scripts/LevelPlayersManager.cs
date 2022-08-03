
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelPlayersManager : MonoBehaviour
{
    [SerializeField] private Camera levelCamera;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<PlayerInput> playerInputs;
    public List<PlayerInput> PlayerInputs { get => playerInputs; }

    private GlobalPlayersManager globalPlayerManager;
    private int amountOfPlayersKilled;

    public event Action<PlayerInput> OnPlayerAdded;

    private void Awake()
    {
        globalPlayerManager = FindObjectOfType<GlobalPlayersManager>();

        levelCamera = Camera.main;
    }

    private void Start()
    {
        List<PlayerInput> playerManagerPlayers = globalPlayerManager.GetPlayerInputs;

        if (playerManagerPlayers.Count > 0)
        {
            playerInputs = playerManagerPlayers;
            AssignAllPlayersNewCamera();
            SpawnPlayers();
        }
        
        globalPlayerManager.OnNewPlayerAdded += AddNewPlayer;
        PlayerStatsController.OnPlayerKilled += ManagePlayerKilledEvent;
    }

    private void OnDisable()
    {
        globalPlayerManager.OnNewPlayerAdded -= AddNewPlayer;
    }

    private void AddNewPlayer(PlayerInput playerInput)
    {
        playerInputs.Add(playerInput);
        playerInput.transform.position = spawnPoints[playerInputs.Count - 1].position;
        OnPlayerAdded?.Invoke(playerInput);
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < playerInputs.Count; i++)
        {
            playerInputs[i].gameObject.GetComponent<PlayerMoveController>().StopMovement();
            playerInputs[i].gameObject.transform.position = spawnPoints[i].position;
            playerInputs[i].gameObject.SetActive(true);
        }
    }

    private void AssignAllPlayersNewCamera()
    {
        for (int i = 0; i < playerInputs.Count; i++)
        {
            playerInputs[i].gameObject.GetComponentInChildren<PlayerTurretRotationController>().SetNewCamera(levelCamera);
            playerInputs[i].gameObject.GetComponentInChildren<Billboard>().SetNewCamera(levelCamera);
            playerInputs[i].gameObject.GetComponentInChildren<TurretBase>().GetComponentInChildren<Billboard>().SetNewCamera(levelCamera);

            playerInputs[i].gameObject.transform.position = spawnPoints[i].position;
        }
    }

    private void ManagePlayerKilledEvent()
    {
        amountOfPlayersKilled++;
        if (amountOfPlayersKilled == globalPlayerManager.CurrentAmountOfPlayers - 1)
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        DisabelAllPlayers();
        int amountOfLevels = SceneManager.sceneCountInBuildSettings;
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        int lextLevelIndex = UnityEngine.Random.Range(1, amountOfLevels);
        while(lextLevelIndex == currentLevelIndex)
        {
            lextLevelIndex = UnityEngine.Random.Range(1, amountOfLevels);
        }
        
        SceneManager.LoadScene(lextLevelIndex);
    }

    private void DisabelAllPlayers()
    {
        for (int i = 0; i < playerInputs.Count; i++)
        {
            playerInputs[i].gameObject.GetComponent<PlayerTurretController>().SetToDefaultTurret();
            playerInputs[i].gameObject.SetActive(false);
        }
    }
}
