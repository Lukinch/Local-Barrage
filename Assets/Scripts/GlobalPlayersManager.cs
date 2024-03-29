
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalPlayersManager : MonoBehaviour
{
    [Header("Settings Dependency")]
    [SerializeField] private GameplaySettingsSO _gameplaySettings;

    [SerializeField] private int _maxNumberOfPlayers;
    [SerializeField] private PlayerInputManager _playerInputManager;

    private float _maxAmountOfPointsToWin;

    private int _playersAmount;
    private List<PlayerInput> _players = new List<PlayerInput>();

    public static GlobalPlayersManager Instance;
    public int PlayersAmount => _playersAmount;
    public float MaxAmountOfPointsToWin => _maxAmountOfPointsToWin;
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

        _maxAmountOfPointsToWin = _gameplaySettings.AmountOfPointsToWin;
        _playersAmount = 0;
    }

    private void OnEnable() => SubscribeToNewPlayersEvent();
    private void OnDisable() => UnsubscribeToNewPlayersEvent();

    private void AddPlayer(PlayerInput playerInput)
    {
        if (_playersAmount == _maxNumberOfPlayers) return;

        _players.Add(playerInput);

        ChangePlayerName();

        _playersAmount++;

        if (_playersAmount < 2) OnFirstPlayerAdded?.Invoke(playerInput);
        else OnNewPlayerAdded?.Invoke(playerInput);
    }

    private void ChangePlayerName()
    {
        PlayerComponentReferences player = _players[_playersAmount].gameObject.GetComponent<PlayerComponentReferences>();
        player.MenuPlayerName.text = $"Player {_players.Count}";
        player.GameplayPlayerName.text = $"Player {_players.Count}";
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

    public void SwitchPlayerActionMap(PlayerInput player, string actionMap)
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
        for (int i = _players.Count - 1; i >= 0; i--)
        {
            Destroy(_players[i].gameObject);
        }
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

    public float[] GetPlayerPointsInt()
    {
        float[] points = new float[_playersAmount];

        for (int i = 0; i < _playersAmount; i++)
        {
            points[i] = _players[i].gameObject.GetComponent<PlayerComponentReferences>().Points.GetPointsInt();
        }

        return points;
    }

    public void AddPointsToPlayer(int index)
    {
        _players[index].gameObject.GetComponent<PlayerComponentReferences>().Points.AddPoints(_gameplaySettings.AmountOfPointsPerWin);
    }

    public int GetLastPlayerStandingIndex()
    {
        return _players.FindIndex(
            player => player.gameObject.GetComponent<PlayerComponentReferences>().TurretPosition.activeInHierarchy);
    }

    public void RemoveAllExtraPlayers()
    {
        for (int i = _players.Count - 1; i >= 1; i--)
        {
            Destroy(_players[i].gameObject);
            _players.RemoveAt(i);
        }

        _playersAmount = 1;
    }

    public void EnablePlayersJoin() => _playerInputManager.EnableJoining();
    public void DisablePlayersJoin() => _playerInputManager.DisableJoining();
    public void SubscribeToNewPlayersEvent() => _playerInputManager.onPlayerJoined += AddPlayer;
    public void UnsubscribeToNewPlayersEvent() => _playerInputManager.onPlayerJoined -= AddPlayer;
    #endregion
}
