
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalPlayersManager : MonoBehaviour
{
    public event Action<PlayerInput> OnNewPlayerAdded;

    public List<PlayerInput> GetPlayerInputs { get => players; }

    [SerializeField] private int maxNumerOfPlayers;
    [SerializeField] private List<PlayerInput> players;

    private PlayerInputManager playerInputManager;
    private GlobalPlayersManager Instance;

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

        playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable() => playerInputManager.onPlayerJoined += AddPlayer;
    private void OnDisable() => playerInputManager.onPlayerJoined -= AddPlayer;

    private void AddPlayer(PlayerInput playerInput)
    {
        if (players.Count == maxNumerOfPlayers) return;
        players.Add(playerInput);

        OnNewPlayerAdded?.Invoke(playerInput);
    }
}
