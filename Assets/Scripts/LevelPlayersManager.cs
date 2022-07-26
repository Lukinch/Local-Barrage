using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelPlayersManager : MonoBehaviour
{
    [SerializeField] private Camera levelCamera;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<PlayerInput> playerInputs;
    public List<PlayerInput> PlayerInputs { get => playerInputs; }

    private GlobalPlayersManager globalPlayerManager;

    public event Action<PlayerInput> OnPlayerAdded;

    private void Awake()
    {
        levelCamera = Camera.main;
    }

    private void Start()
    {
        globalPlayerManager = FindObjectOfType<GlobalPlayersManager>();

        List<PlayerInput> playerManagerPlayers = globalPlayerManager.GetPlayerInputs;

        if (playerManagerPlayers.Count > 0)
        {
            playerInputs = playerManagerPlayers;
            AssignAllPlayersNewCamera();
            SpawnPlayers();
        }
        globalPlayerManager.OnNewPlayerAdded += AddNewPlayer;
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
}
