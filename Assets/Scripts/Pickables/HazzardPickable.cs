using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazzardPickable : Pickable
{
    [SerializeField] private float damageAmount;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPickableCollision player = other.GetComponent<PlayerPickableCollision>();

        if (player != null)
        {
            if (player.PlayerDamageController.IsShieldActive())
            {
                player.PlayerDamageController.DamageShield(damageAmount);
            }
            else
            {
                player.PlayerDamageController.DamageHull(damageAmount);
            }
        }
    }
}
