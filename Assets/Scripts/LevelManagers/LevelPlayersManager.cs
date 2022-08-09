
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelPlayersManager : MonoBehaviour
{
    [SerializeField] private Camera _levelCamera;
    [SerializeField] private LevelObjectiveScreenController _levelObjectiveScreenController;
    [SerializeField] private List<Transform> _spawnPoints;

    private int _amountOfPlayersKilled;
    private int _amountOfPlayers;

    public event Action OnLevelEnded;

    private void Awake()
    {
        _levelCamera = Camera.main;
    }

    private void Start()
    {
        GlobalPlayersManager.Instance.AssignAllPlayersNewCamera(_levelCamera);
        _amountOfPlayers = GlobalPlayersManager.Instance.PlayersAmount;

        SetPlayersInitialPositions();

        _levelObjectiveScreenController.OnObjectiveShown += EnableAllPlayersVisualsAndGameplayComponents;
        PlayerStatsController.OnPlayerKilled += ManagePlayerKilledEvent;
    }

    private void OnDestroy()
    {
        _levelObjectiveScreenController.OnObjectiveShown -= EnableAllPlayersVisualsAndGameplayComponents;
        PlayerStatsController.OnPlayerKilled -= ManagePlayerKilledEvent;
    }

    private void EnableAllPlayersVisualsAndGameplayComponents()
    {
        GlobalPlayersManager.Instance.SwitchAllPlayersActionMap("UI");
        GlobalPlayersManager.Instance.SwitchAllPlayersActionMap("Player");
        GlobalPlayersManager.Instance.EnableAllPlayersVisuals();
        GlobalPlayersManager.Instance.EnableAllPlayersGameplayComponents();
    }

    private void SetPlayersInitialPositions()
    {
        List<PlayerInput> players = GlobalPlayersManager.Instance.GetPlayerInputs;
        for (int i = 0; i < _amountOfPlayers; i++)
        {
            players[i].gameObject.transform.position = _spawnPoints[i].position;
        }
    }

    private void ManagePlayerKilledEvent()
    {
        _amountOfPlayersKilled++;
        if (_amountOfPlayersKilled == _amountOfPlayers - 1)
        {
            OnLevelEnded?.Invoke();
        }
    }
}
