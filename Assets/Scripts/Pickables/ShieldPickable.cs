using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickable : Pickable
{
    [SerializeField] private float shieldAmount;

    public static event Action<Transform> OnShieldDestroyed;

    protected override void ImplementEffect(PlayerPickableCollision player)
    {
        if (!player.PlayerStatsController.ShieldCanBeHealed()) return;

        player.PlayerStatsController.RestoreShield(shieldAmount);
        OnPicked();
    }

    protected override void NotifyDestruction()
    {
        OnShieldDestroyed?.Invoke(transform.parent);
    }
}
