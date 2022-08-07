using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickable : Pickable
{
    [SerializeField] bool _shouldDisappear;
    [SerializeField] private float _healAmount;

    public static event Action<Transform> OnHealthDestroyed;

    protected override void ImplementEffect(PlayerPickableCollision player)
    {
        if (!player.PlayerStatsController.HealthCanBeHealed()) return;

        player.PlayerStatsController.RestoreHealth(_healAmount);

        if (!_shouldDisappear) return;
        OnPicked();
    }

    protected override void NotifyDestruction()
    {
        OnHealthDestroyed?.Invoke(transform.parent);
    }
}
