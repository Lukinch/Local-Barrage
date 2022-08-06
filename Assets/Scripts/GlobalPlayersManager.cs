
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
    private List<PlayerPoints> playersPoints = new List<PlayerPoints>();
    private List<GameObject> playersMenuUI = new List<GameObject>();
    private List<GameObject> playersLiveUI = new List<GameObject>();
    private List<GameObject> playersVisuals = new List<GameObject>();
    private List<GameObject> playersCollidersContainer = new List<GameObject>();
    private List<KeepInPlacePosition> playersKeepInPlaceControllers = new List<KeepInPlacePosition>();
    private List<PlayerTurretController> playersTurretControllers = new List<PlayerTurretController>();
    private List<Billboard> playersBillboards = new List<Billboard>();
    private List<PlayerTurretRotationController> playersTurretRotations = new List<PlayerTurretRotationController>();

    public static GlobalPlayersManager Instance;
    public int PlayersAmount => playersAmount;
    public int MaxAmountOfPointsPerPlayer => maxAmountOfPointsPerPlayer;
    public List<PlayerPoints> PlayersPoints => playersPoints;
    public List<PlayerInput> GetPlayerInputs { get => players; }
    public event Action<PlayerInput> OnNewPlayerAdded;
    public event Action OnGameStarted;

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

        SwitchPlayerActionMap(playerInput, "UI");

        playersLiveUI.Add(playerObject.GetComponentInChildren<UIPlayerLiveUI>().gameObject);
        playersMenuUI.Add(playerObject.GetComponentInChildren<UIUnitMainMenuManager>().gameObject);
        playersCollidersContainer.Add(playerObject.GetComponentInChildren<PlayerColliders>().gameObject);
        playersVisuals.Add(playerObject.GetComponentInChildren<PlayerVisuals>().gameObject);
        playersKeepInPlaceControllers.Add(playerObject.GetComponentInChildren<KeepInPlacePosition>());
        playersTurretControllers.Add(playerObject.GetComponentInChildren<PlayerTurretController>());
        playersPoints.Add(playerObject.GetComponent<PlayerPoints>());
        playersBillboards.Add(playerObject.GetComponentInChildren<Billboard>());
        playersTurretRotations.Add(playerObject.GetComponentInChildren<PlayerTurretRotationController>());

        DisablePlayerGameplayComponents(PlayersAmount);


        players.Add(playerInput);

        playersAmount++;

        OnNewPlayerAdded?.Invoke(playerInput);
    }

    public void EnableAllPlayersVisualsAndUI()
    {
        playersMenuUI.ForEach(ui => ui.gameObject.SetActive(false));
        playersLiveUI.ForEach(ui => ui.gameObject.SetActive(true));
        playersVisuals.ForEach(visual => visual.gameObject.SetActive(true));
    }
    public void DisableAllPlayersVisualsAndUI()
    {
        playersMenuUI.ForEach(ui => ui.gameObject.SetActive(false));
        playersLiveUI.ForEach(ui => ui.gameObject.SetActive(false));
        playersVisuals.ForEach(visual => visual.gameObject.SetActive(false));
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
        playersCollidersContainer[index].SetActive(true);
        playersMenuUI[index].SetActive(false);
    }
    public void DisablePlayerGameplayComponents(int index)
    {
        playersKeepInPlaceControllers[index].enabled = false;
        playersTurretControllers[index].enabled = false;
        playersLiveUI[index].SetActive(false);
        playersCollidersContainer[index].SetActive(false);
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
        players.ForEach(player => Destroy(player.gameObject));

        players.Clear();
        playersPoints.Clear();
        playersMenuUI.Clear();
        playersLiveUI.Clear();
        playersCollidersContainer.Clear();
        playersVisuals.Clear();
        playersKeepInPlaceControllers.Clear();
        playersTurretControllers.Clear();
        playersBillboards.Clear();
        playersTurretRotations.Clear();

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
            playersBillboards[i].SetNewCamera(levelCamera);
            playersTurretRotations[i].SetNewCamera(levelCamera);
        }
    }
    public void StopPlayersMovement()
    {
        players.ForEach(
            player => player
                        .gameObject
                        .GetComponent<PlayerMoveController>()
                        .StopMovement()
        );
    }
    public int[] GetPlayerPointsInt()
    {
        int[] points = new int[playersAmount];
        
        for (int i = 0; i < playersAmount; i++)
        {
            points[i] = playersPoints[i].GetPointsInt();
        }

        return points;
    }
    public void EnablePlayersJoin() => playerInputManager.EnableJoining();
    public void DisablePlayersJoin() => playerInputManager.DisableJoining();
    public void SubscribeToNewPlayersEvent() => playerInputManager.onPlayerJoined += AddPlayer;
    public void UnsubscribeToNewPlayersEvent() => playerInputManager.onPlayerJoined -= AddPlayer;
    public void GameStarted() => OnGameStarted?.Invoke();
}
