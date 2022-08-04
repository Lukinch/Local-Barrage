
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalPlayersManager : MonoBehaviour
{
    [SerializeField] private int maxNumerOfPlayers;
    [SerializeField] private PlayerInputManager playerInputManager;

    private int playersAmount;
    private List<PlayerInput> players = new List<PlayerInput>();
    private List<GameObject> playersMenuUI = new List<GameObject>();
    private List<GameObject> playersLiveUI = new List<GameObject>();
    private List<GameObject> playersShield = new List<GameObject>();
    private List<KeepInPlacePosition> playersKeepInPlaceControllers = new List<KeepInPlacePosition>();
    private List<PlayerTurretController> playersTurretControllers = new List<PlayerTurretController>();

    public static GlobalPlayersManager Instance;
    public int PlayersAmount => playersAmount;
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
    }

    private void OnEnable() => SubscribeToNewPlayersEvent();
    private void OnDisable() => UnsubscribeToNewPlayersEvent();

    private void AddPlayer(PlayerInput playerInput)
    {
        if (players.Count == maxNumerOfPlayers) return;

        GameObject playerObject = playerInput.gameObject;

        playersLiveUI.Add(playerObject.GetComponentInChildren<Billboard>().gameObject);
        playersMenuUI.Add(playerObject.GetComponentInChildren<UIUnitMainMenuManager>().gameObject);
        playersShield.Add(playerObject.GetComponentInChildren<PlayerShieldCollision>().gameObject);
        playersKeepInPlaceControllers.Add(playerObject.GetComponentInChildren<KeepInPlacePosition>());
        playersTurretControllers.Add(playerObject.GetComponentInChildren<PlayerTurretController>());

        SwitchPlayerActionMap(playerInput, "UI");
        DisablePlayerGameplayComponents(PlayersAmount);

        players.Add(playerInput);

        playersAmount++;

        OnNewPlayerAdded?.Invoke(playerInput);
    }

    public void EnableAllPlayers()
    {
        players.ForEach(player => player.gameObject.SetActive(true));
    }
    public void DisableAllPlayers()
    {
        players.ForEach(player => player.gameObject.SetActive(false));
    }
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
        playersKeepInPlaceControllers[index].enabled = true;
        playersTurretControllers[index].enabled = true;
        playersLiveUI[index].SetActive(true);
        playersShield[index].SetActive(true);
        playersMenuUI[index].SetActive(false);
    }
    public void DisablePlayerGameplayComponents(int index)
    {
        playersKeepInPlaceControllers[index].enabled = false;
        playersTurretControllers[index].enabled = false;
        playersLiveUI[index].SetActive(false);
        playersShield[index].SetActive(false);
        playersMenuUI[index].SetActive(true);
    }
    public void EnableAllPlayersGameplayComponents()
    {
        for (int i = 0; i < players.Count; i++)
        {
            EnablePlayerGameplayComponents(i);
        }
    }
    public void DisableAllPlayersGameplayComponents()
    {
        for (int i = 0; i < players.Count; i++)
        {
            DisablePlayerGameplayComponents(i);
        }
    }
    public void ClearPlayersList()
    {
        players.Clear();
        playersMenuUI.Clear();
        playersLiveUI.Clear();
        playersShield.Clear();
        playersKeepInPlaceControllers.Clear();

        playersAmount = 0;
    }
    public void SetPlayersDefaultTurret()
    {
        playersTurretControllers.ForEach(controller => controller.SetToDefaultTurret());
    }
    public void AssignAllPlayersNewCamera(Camera levelCamera)
    {
        for (int i = 0; i < playersAmount; i++)
        {
            players[i].gameObject.GetComponentInChildren<PlayerTurretRotationController>().SetNewCamera(levelCamera);
            players[i].gameObject.GetComponentInChildren<Billboard>().SetNewCamera(levelCamera);
            players[i].gameObject.GetComponentInChildren<TurretBase>().GetComponentInChildren<Billboard>().SetNewCamera(levelCamera);
        }
    }
    public void StopPlayersMovement()
    {
        players.ForEach(player => player.gameObject.GetComponent<PlayerMoveController>().StopMovement());
    }
    public void EnablePlayersJoin() => playerInputManager.EnableJoining();
    public void DisablePlayersJoin() => playerInputManager.DisableJoining();
    public void SubscribeToNewPlayersEvent() => playerInputManager.onPlayerJoined += AddPlayer;
    public void UnsubscribeToNewPlayersEvent() => playerInputManager.onPlayerJoined -= AddPlayer;
}
