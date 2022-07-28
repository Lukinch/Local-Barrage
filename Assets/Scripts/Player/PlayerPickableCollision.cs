
using UnityEngine;

public class PlayerPickableCollision : MonoBehaviour
{
    [SerializeField] private PlayerTurretController playerTurretController;
    [SerializeField] private PlayerStatsController playerStatsController;

    public PlayerTurretController TurretController
    {
        get => playerTurretController;
    }

    public PlayerStatsController PlayerStatsController
    {
        get => playerStatsController;
    }
}
