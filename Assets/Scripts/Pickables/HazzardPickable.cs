using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazzardPickable : Pickable
{
    [SerializeField] bool shouldDisappear;
    [SerializeField] private float damageAmount;

    protected override void ImplementEffect(PlayerPickableCollision player)
    {
        if (player.PlayerStatsController.IsShieldActive())
        {
            player.PlayerStatsController.PickableDamageShield(damageAmount);
        }
        else
        {
            player.PlayerStatsController.PickableDamageHull(damageAmount);
        }
    }

    protected override void NotifyDestruction() {}
}
