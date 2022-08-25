
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelPlayersManager : MonoBehaviour
{
    [SerializeField] private GameObject _dummyPlayerPrefab;
    [SerializeField] private Camera _levelCamera;
    [SerializeField] private LevelObjectiveScreenController _levelObjectiveScreenController;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField, Range(1, 3)] private int _maxAmountOfDummies = 3;

    private int _amountOfPlayersKilled;
    private int _amountOfDummiesKilled;
    private int _amountOfPlayers;
    private PlayerComponentReferences[] _dummies = new PlayerComponentReferences[3];

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

        if (_amountOfPlayers < 2)
        {
            SpawnDummyplayers();
            DummyStatsController.OnDummyKilled += ManageDummyKilledEvent;
        }
        else
        {
            PlayerStatsController.OnPlayerKilled += ManagePlayerKilledEvent;
        }
    }

    private void ManageDummyKilledEvent()
    {
        _amountOfDummiesKilled++;
        if (_amountOfDummiesKilled == _maxAmountOfDummies)
        {
            OnLevelEnded?.Invoke();
        }
    }

    private void SpawnDummyplayers()
    {
        for (int i = 1; i < _spawnPoints.Count; i++)
        {
            Vector3 position = _spawnPoints[i].position;
            Quaternion rotation = _spawnPoints[i].rotation;
            GameObject dummy = Instantiate(_dummyPlayerPrefab, position, rotation);
            PlayerComponentReferences player = dummy.GetComponent<PlayerComponentReferences>();
            _dummies[i - 1] = player;
            player.TurretPosition.SetActive(false);
            player.SphereRenderer.enabled = false;
            player.LiveUI.SetActive(false);
            player.PlayerColliders.SetActive(false);
            player.GameplayPlayerName.text = $"Dummy {i}";
        }
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

        if (_amountOfPlayers < 2)
        {
            EnableDummiesVisuals();
        }
    }

    private void EnableDummiesVisuals()
    {
        for (int i = 0; i < _dummies.Length; i++)
        {
            _dummies[i].TurretPosition.SetActive(true);
            _dummies[i].SphereRenderer.enabled = true;
            _dummies[i].LiveUI.SetActive(true);
            _dummies[i].PlayerColliders.SetActive(true);
        }
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
