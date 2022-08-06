
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelPlayersManager : MonoBehaviour
{
    [SerializeField] private Camera levelCamera;
    [SerializeField] private LevelObjectiveScreenController levelObjectiveScreenController;
    [SerializeField] private List<Transform> spawnPoints;

    private int amountOfPlayersKilled;
    private int amountOfPlayers;

    public event Action OnLevelEnded;

    private void Awake()
    {
        levelCamera = Camera.main;
    }

    private void Start()
    {
        GlobalPlayersManager.Instance.AssignAllPlayersNewCamera(levelCamera);
        GlobalPlayersManager.Instance.StopPlayersMovement();
        amountOfPlayers = GlobalPlayersManager.Instance.PlayersAmount;

        SetPlayersInitialPositions();

        levelObjectiveScreenController.OnObjectiveShown += InitializePlayerSpawn;
        PlayerStatsController.OnPlayerKilled += ManagePlayerKilledEvent;
    }

    private void OnDestroy()
    {
        levelObjectiveScreenController.OnObjectiveShown -= InitializePlayerSpawn;
        PlayerStatsController.OnPlayerKilled -= ManagePlayerKilledEvent;
    }

    private void InitializePlayerSpawn()
    {
        GlobalPlayersManager.Instance.EnableAllPlayersVisualsAndUI();
    }

    private void SetPlayersInitialPositions()
    {
        List<PlayerInput> players = GlobalPlayersManager.Instance.GetPlayerInputs;
        for (int i = 0; i < amountOfPlayers; i++)
        {
            players[i].gameObject.transform.position = spawnPoints[i].position;
        }
    }

    private void ManagePlayerKilledEvent()
    {
        amountOfPlayersKilled++;
        if (amountOfPlayersKilled == amountOfPlayers - 1)
        {
            OnLevelEnded?.Invoke();
        }
    }
}
