using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickable : Pickable
{
    [SerializeField] bool shouldDisappear;
    [SerializeField] private float shieldAmount;

    public static event Action<Transform> OnShieldDestroyed;

    protected override void ImplementEffect(PlayerPickableCollision player)
    {
        if (!player.PlayerStatsController.ShieldCanBeHealed()) return;

        player.PlayerStatsController.RestoreShield(shieldAmount);

        if (!shouldDisappear) return;
        OnPicked();
    }

    protected override void NotifyDestruction()
    {
        OnShieldDestroyed?.Invoke(transform.parent);
    }
}
