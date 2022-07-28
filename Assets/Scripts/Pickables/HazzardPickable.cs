using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazzardPickable : Pickable
{
    [SerializeField] private float damageAmount;

    protected override void ImplementEffect(PlayerPickableCollision player)
    {
        if (player.PlayerStatsController.IsShieldActive())
        {
            player.PlayerStatsController.DamageShield(damageAmount);
        }
        else
        {
            player.PlayerStatsController.DamageHull(damageAmount);
        }
    }

    protected override void NotifyDestruction() {}
}
