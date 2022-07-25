using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickable : Pickable
{
    [SerializeField] private float healAmount;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPickableCollision player = other.GetComponent<PlayerPickableCollision>();

        if (player != null)
        {
            if (!player.PlayerDamageController.HealthCanBeHealed()) return;

            player.PlayerDamageController.RestoreHealth(healAmount);
        }
    }
}
