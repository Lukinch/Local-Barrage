using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPickable : Pickable
{
    [SerializeField] private GameObject turretPrefab;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPickableCollision player = other.GetComponent<PlayerPickableCollision>();

        if (player == null) return;
        if (player.TurretController.HasActiveTurret()) return;

        TurretBase tBase = turretPrefab.GetComponent<TurretBase>();

        if (player.TurretController.ContainsTurret(tBase.TurretName))
        {
            player.TurretController.OnWeaponPickedUp(tBase.TurretName);
            //OnPicked();
        }
        else
        {
            player.TurretController.OnNewWeaponPickedUp(turretPrefab);
            //OnPicked();
        }

    }
}
