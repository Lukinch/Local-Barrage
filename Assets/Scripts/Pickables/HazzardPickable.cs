using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazzardPickable : Pickable
{
    [SerializeField] bool _shouldDisappear;
    [SerializeField] private float _damageAmount;

    protected override void ImplementEffect(PlayerPickableCollision player)
    {
        if (player.PlayerStatsController.IsShieldActive())
        {
            player.PlayerStatsController.PickableDamageShield(_damageAmount);
        }
        else
        {
            player.PlayerStatsController.PickableDamageHull(_damageAmount);
        }
    }

    protected override void NotifyDestruction() {}
}
