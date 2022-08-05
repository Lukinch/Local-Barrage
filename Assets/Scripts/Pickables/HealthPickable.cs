using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickable : Pickable
{
    [SerializeField] bool shouldDisappear;
    [SerializeField] private float healAmount;

    public static event Action<Transform> OnHealthDestroyed;

    protected override void ImplementEffect(PlayerPickableCollision player)
    {
        if (!player.PlayerStatsController.HealthCanBeHealed()) return;

        player.PlayerStatsController.RestoreHealth(healAmount);

        if (!shouldDisappear) return;
        OnPicked();
    }

    protected override void NotifyDestruction()
    {
        OnHealthDestroyed?.Invoke(transform.parent);
    }
}
