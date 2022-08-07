
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

        GameObject playerObject = playerInput.gameObject;

        _players.Add(playerInput);

        DisablePlayerGameplayComponents(PlayersAmount);
        EnablePlayerMenuUI(PlayersAmount);

        SwitchPlayerActionMap(playerInput, "UI");

        _playersAmount++;

        OnNewPlayerAdded?.Invoke(playerInput);
    }

    private void EnableAllPlayersRigidBodies()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            _players[i].gameObject.GetComponent<PlayerComponentReferences>().MoveController.EnableRigidBody();
        }
    }

    private void DisableAllPlayersRigidBodies()
    {
        for (int i = 0; i < _playersAmount; i++)
        {
            _players[i].gameObject.GetComponent<PlayerComponentReferences>().MoveController.DisableRigidBody();
        }
    }

    #region Public Methods
    public void SwitchPlayerActionMap(PlayerInput player ,string actionMap)
    {
        player.SwitchCurrentActionMap(actionMap);
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
        player.LiveUI.SetActive(true);
        player.PlayerColliders.SetActive(true);
    }

    public void DisablePlayerGameplayComponents(int index)
    {
        PlayerComponentReferences player = _players[index].gameObject.GetComponent<PlayerComponentReferences>();
        player.TurretRotationController.enabled = false;
        player.TurretController.enabled = false;
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

        EnableAllPlayersRigidBodies();
    }

    public void DisableAllPlayersVisuals()
    {
        DisableAllPlayersRigidBodies();

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
