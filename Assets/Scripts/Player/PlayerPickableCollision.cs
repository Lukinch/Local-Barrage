
using UnityEngine;

public class PlayerPickableCollision : MonoBehaviour
{
    [SerializeField] private PlayerTurretController _playerTurretController;
    [SerializeField] private PlayerStatsController _playerStatsController;

    public PlayerTurretController TurretController
    {
        get => _playerTurretController;
    }

    public PlayerStatsController PlayerStatsController
    {
        get => _playerStatsController;
    }
}
