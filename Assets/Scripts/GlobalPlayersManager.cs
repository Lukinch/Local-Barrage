
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalPlayersManager : MonoBehaviour
{
    [SerializeField] private int _maxNumerOfPlayers;
    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private int _maxAmountOfPointsPerPlayer = 50;

    private int _playersAmount;
    private List<PlayerInput> _players = new List<PlayerInput>();

    public static GlobalPlayersManager Instance;
    public int PlayersAmount => _playersAmount;
    public int MaxAmountOfPointsPerPlayer => _maxAmountOfPointsPerPlayer;
    public List<PlayerInput> GetPlayerInputs { get => _players; }

    public event Action<PlayerInput> OnFirstPlayerAdded;
    public event Action<PlayerInput> OnNewPlayerAdded;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _playersAmount = 0;
    }

    private void OnEnable() => SubscribeToNewPlayersEvent();
    private void OnDisable() => UnsubscribeToNewPlayersEvent();

    private void AddPlayer(PlayerInput playerInput)
    {
        if (_playersAmount == _maxNumerOfPlayers) return;

        _players.Add(playerInput);

        _playersAmount++;
        
        if (_playersAmount < 2) OnFirstPlayerAdded?.Invoke(playerInput);
        else OnNewPlayerAdded?.Invoke(playerInput);
    }

    #region Public Methods
    public void MakePlayerKinematic(int playerIndex)
    {
        _players[playerIndex].gameObject.GetComponent<PlayerComponentReferences>().MoveController.MakePlayerKinematic();
    }

    public void MakeAllPlayerNonKinematic()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            _players[i].gameObject.GetComponent<PlayerComponentReferences>().MoveController.MakePlayerNonKinematic();
        }
    }

    public void MakeAllPlayerKinematic()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            _players[i].gameObject.GetComponent<PlayerComponentReferences>().MoveController.MakePlayerKinematic();
        }
    }

    public void SwitchPlayerActionMap(PlayerInput player ,string actionMap)
    {
        player.SwitchCurrentActionMap(actionMap);
    }
    public void SwitchPlayerActionMap(int playerIndex, string actionMap)
    {
        _players[playerIndex].SwitchCurrentActionMap(actionMap);
    }

    public void SwitchAllPlayersActionMap(string actionMap)
    {
        _players.ForEach(player => player.SwitchCurrentActionMap(actionMap));
    }

    public void EnablePlayerGameplayComponents(int index)
    {
        PlayerComponentReferences player = _players[index].gameObject.GetComponent<PlayerComponentReferences>();
        player.TurretRotationController.enabled = true;
        player.TurretController.enabled = true;
        player.TurretFiringController.ShouldTriggerEvents = true;
        player.LiveUI.SetActive(true);
        player.PlayerColliders.SetActive(true);
    }

    public void DisablePlayerGameplayComponents(int index)
    {
        PlayerComponentReferences player = _players[index].gameObject.GetComponent<PlayerComponentReferences>();
        player.TurretRotationController.enabled = false;
        player.TurretController.enabled = false;
        player.TurretFiringController.ShouldTriggerEvents = false;
        player.LiveUI.SetActive(false);
        player.PlayerColliders.SetActive(false);
    }


    public void EnableAllPlayersGameplayComponents()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            EnablePlayerGameplayComponents(i);
        }
    }

    public void DisableAllPlayersGameplayComponents()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            DisablePlayerGameplayComponents(i);
        }
    }

    public void SwitchAllPlayersToUiInput()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            _players[i].SwitchCurrentActionMap("UI");
        }
    }
    public void SwitchAllPlayersToEmptyInput()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            _players[i].SwitchCurrentActionMap("Empty");
        }
    }

    public void SetupPlayerPersonalEventSystem(int index)
    {
        _players[index].gameObject
            .GetComponent<PlayerComponentReferences>()
            .PlayerUiManager
            .SetupEventSystem();
    }

    public void SetupAllPlayersPersonalEventSystem()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            SetupPlayerPersonalEventSystem(i);
        }
    }

    public void EnablePlayerPersonalEventSystem(int index)
    {
        _players[index].gameObject
            .GetComponent<PlayerComponentReferences>()
            .PlayerUiManager
            .EnableEventSystem();
    }

    public void EnableAllPlayersPersonalEventSystem()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            EnablePlayerPersonalEventSystem(i);
        }
    }

    public void DisablePlayerPersonalEventSystem(int index)
    {
        _players[index].gameObject
            .GetComponent<PlayerComponentReferences>()
            .PlayerUiManager
            .DisableEventSystem();
    }

    public void DisableAllPlayersPersonalEventSystem()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            DisablePlayerPersonalEventSystem(i);
        }
    }

    public void DisableAllPlayersUIs()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            PlayerComponentReferences player = _players[i].gameObject.GetComponent<PlayerComponentReferences>();
            player.LiveUI.SetActive(false);
            player.MenuUI.SetActive(false);
        }
    }

    public void EnablePlayerMenuUI(int index)
    {
        _players[index].gameObject.GetComponent<PlayerComponentReferences>().MenuUI.SetActive(true);
    }

    public void EnableAllPlayersVisuals()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            PlayerComponentReferences player = _players[i].gameObject.GetComponent<PlayerComponentReferences>();
            player.TurretPosition.SetActive(true);
            player.SphereRenderer.enabled = true;
        }

        MakeAllPlayerNonKinematic();
    }

    public void DisableAllPlayersVisuals()
    {
        MakeAllPlayerKinematic();

        for (int i = 0; i < _playersAmount; i++)
        {
            PlayerComponentReferences player = _players[i].gameObject.GetComponent<PlayerComponentReferences>();
            player.TurretPosition.SetActive(false);
            player.SphereRenderer.enabled = false;
        }
    }

    public void DisablePlayerVisuals(int inputIndex)
    {
        PlayerComponentReferences player = _players[inputIndex].gameObject.GetComponent<PlayerComponentReferences>();
        player.TurretPosition.SetActive(false);
        player.SphereRenderer.enabled = false;
    }

    public void ClearPlayersList()
    {
        _players.ForEach(player => Destroy(player.gameObject));
        _players.Clear();

        _playersAmount = 0;
    }

    public void SetAllPlayersDefaultTurret()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            _players[i].gameObject.GetComponent<PlayerComponentReferences>().TurretController.SetToDefaultTurret();
        }
    }

    public void AssignAllPlayersNewCamera(Camera levelCamera)
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            PlayerComponentReferences player = _players[i].gameObject.GetComponent<PlayerComponentReferences>();
            player.Billboard.SetNewCamera(levelCamera);
            player.TurretRotationController.SetNewCamera(levelCamera);
        }
    }

    public int[] GetPlayerPointsInt()
    {
        int[] points = new int[_playersAmount];
        
        for (int i = 0; i < _playersAmount; i++)
        {
            points[i] = _players[i].gameObject.GetComponent<PlayerComponentReferences>().Points.GetPointsInt();
        }

        return points;
    }

    public void AddPointsToPlayer(int index)
    {
        _players[index].gameObject.GetComponent<PlayerComponentReferences>().Points.AddPoints();
    }

    public int GetLastPlayerStandingIndex()
    {
        return _players.FindIndex(
            player => player.gameObject.GetComponent<PlayerComponentReferences>().TurretPosition.activeInHierarchy);
    }

    public void EnablePlayersJoin() => _playerInputManager.EnableJoining();
    public void DisablePlayersJoin() => _playerInputManager.DisableJoining();
    public void SubscribeToNewPlayersEvent() => _playerInputManager.onPlayerJoined += AddPlayer;
    public void UnsubscribeToNewPlayersEvent() => _playerInputManager.onPlayerJoined -= AddPlayer;
    #endregion
}
