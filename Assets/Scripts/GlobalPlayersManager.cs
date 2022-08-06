
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalPlayersManager : MonoBehaviour
{
    [SerializeField] private int maxNumerOfPlayers;
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private int maxAmountOfPointsPerPlayer = 50;

    private int playersAmount;
    private List<PlayerInput> players = new List<PlayerInput>();

    public static GlobalPlayersManager Instance;
    public int PlayersAmount => playersAmount;
    public int MaxAmountOfPointsPerPlayer => maxAmountOfPointsPerPlayer;
    public List<PlayerInput> GetPlayerInputs { get => players; }
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

        playersAmount = 0;
    }

    private void OnEnable() => SubscribeToNewPlayersEvent();
    private void OnDisable() => UnsubscribeToNewPlayersEvent();

    private void AddPlayer(PlayerInput playerInput)
    {
        if (playersAmount == maxNumerOfPlayers) return;

        GameObject playerObject = playerInput.gameObject;

        players.Add(playerInput);

        DisablePlayerGameplayComponents(PlayersAmount);
        EnablePlayerMenuUI(PlayersAmount);

        SwitchPlayerActionMap(playerInput, "UI");

        playersAmount++;

        OnNewPlayerAdded?.Invoke(playerInput);
    }

    private void EnableAllPlayersRigidBodies()
    {
        for (int i = 0; i < playersAmount; i++)
        {
            players[i].gameObject.GetComponent<PlayerComponentReferences>().MoveController.EnableRigidBody();
        }
    }

    private void DisableAllPlayersRigidBodies()
    {
        for (int i = 0; i < playersAmount; i++)
        {
            players[i].gameObject.GetComponent<PlayerComponentReferences>().MoveController.DisableRigidBody();
        }
    }

    #region Public Methods
    public void SwitchPlayerActionMap(PlayerInput player ,string actionMap)
    {
        player.SwitchCurrentActionMap(actionMap);
    }

    public void SwitchAllPlayersActionMap(string actionMap)
    {
        players.ForEach(player => player.SwitchCurrentActionMap(actionMap));
    }

    public void EnablePlayerGameplayComponents(int index)
    {
        PlayerComponentReferences player = players[index].gameObject.GetComponent<PlayerComponentReferences>();
        player.TurretController.enabled = true;
        player.KeepInPlacePosition.enabled = true;
        player.LiveUI.SetActive(true);
        player.PlayerColliders.SetActive(true);
    }

    public void DisablePlayerGameplayComponents(int index)
    {
        PlayerComponentReferences player = players[index].gameObject.GetComponent<PlayerComponentReferences>();
        player.TurretController.enabled = false;
        player.KeepInPlacePosition.enabled = false;
        player.LiveUI.SetActive(false);
        player.PlayerColliders.SetActive(false);
    }


    public void EnableAllPlayersGameplayComponents()
    {
        for (int i = 0; i < playersAmount; i++)
        {
            EnablePlayerGameplayComponents(i);
        }
    }

    public void DisableAllPlayersGameplayComponents()
    {
        for (int i = 0; i < playersAmount; i++)
        {
            DisablePlayerGameplayComponents(i);
        }
    }

    public void DisableAllPlayersUIs()
    {
        for (int i = 0; i < playersAmount; i++)
        {
            PlayerComponentReferences player = players[i].gameObject.GetComponent<PlayerComponentReferences>();
            player.LiveUI.SetActive(false);
            player.MenuUI.SetActive(false);
        }
    }

    public void EnablePlayerMenuUI(int index)
    {
        players[index].gameObject.GetComponent<PlayerComponentReferences>().MenuUI.SetActive(true);
    }

    public void EnableAllPlayersVisuals()
    {
        for (int i = 0; i < playersAmount; i++)
        {
            players[i].gameObject.GetComponent<PlayerComponentReferences>().Visuals.SetActive(true);
        }

        EnableAllPlayersRigidBodies();
    }

    public void DisableAllPlayersVisuals()
    {
        DisableAllPlayersRigidBodies();

        for (int i = 0; i < playersAmount; i++)
        {
            players[i].gameObject.GetComponent<PlayerComponentReferences>().Visuals.SetActive(false);
        }
    }

    public void DisablePlayerVisuals(int inputIndex)
    {
        players[inputIndex].gameObject.GetComponent<PlayerComponentReferences>().Visuals.SetActive(false);
    }

    public void ClearPlayersList()
    {
        players.ForEach(player => Destroy(player.gameObject));
        players.Clear();

        playersAmount = 0;
    }

    public void SetAllPlayersDefaultTurret()
    {
        for (int i = 0; i < playersAmount; i++)
        {
            players[i].gameObject.GetComponent<PlayerComponentReferences>().TurretController.SetToDefaultTurret();
        }
    }

    public void AssignAllPlayersNewCamera(Camera levelCamera)
    {
        for (int i = 0; i < playersAmount; i++)
        {
            PlayerComponentReferences player = players[i].gameObject.GetComponent<PlayerComponentReferences>();
            player.Billboard.SetNewCamera(levelCamera);
            player.TurretRotationController.SetNewCamera(levelCamera);
        }
    }

    public int[] GetPlayerPointsInt()
    {
        int[] points = new int[playersAmount];
        
        for (int i = 0; i < playersAmount; i++)
        {
            points[i] = players[i].gameObject.GetComponent<PlayerComponentReferences>().Points.GetPointsInt();
        }

        return points;
    }

    public void AddPointsToPlayer(int index)
    {
        players[index].gameObject.GetComponent<PlayerComponentReferences>().Points.AddPoints();
    }

    public int GetLastPlayerStandingIndex()
    {
        return players.FindIndex(
            player => player.gameObject.GetComponent<PlayerComponentReferences>().Visuals.activeInHierarchy);
    }

    public void EnablePlayersJoin() => playerInputManager.EnableJoining();
    public void DisablePlayersJoin() => playerInputManager.DisableJoining();
    public void SubscribeToNewPlayersEvent() => playerInputManager.onPlayerJoined += AddPlayer;
    public void UnsubscribeToNewPlayersEvent() => playerInputManager.onPlayerJoined -= AddPlayer;
    #endregion
}
