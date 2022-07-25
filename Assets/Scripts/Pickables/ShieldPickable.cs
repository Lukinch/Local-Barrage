using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickable : Pickable
{
    [SerializeField] private float shieldAmount;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPickableCollision player = other.GetComponent<PlayerPickableCollision>();

        if (player != null)
        {
            if (!player.PlayerDamageController.ShieldCanBeHealed()) return;

            player.PlayerDamageController.RestoreShield(shieldAmount);
        }
    }
}
