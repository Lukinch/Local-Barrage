using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickable : Pickable
{
    [SerializeField] bool _shouldDisappear;
    [SerializeField] private float _shieldAmount;

    public static event Action<Transform> OnShieldDestroyed;

    protected override void ImplementEffect(PlayerPickableCollision player)
    {
        if (!player.PlayerStatsController.ShieldCanBeHealed()) return;

        player.PlayerStatsController.RestoreShield(_shieldAmount);

        if (!_shouldDisappear) return;
        OnPicked();
    }

    protected override void NotifyDestruction()
    {
        OnShieldDestroyed?.Invoke(transform.parent);
    }
}
