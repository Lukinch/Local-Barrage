using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPickable : Pickable
{
    [SerializeField] bool shouldDisappear;
    [SerializeField] private GameObject turretPrefab;

    public static event Action<Transform> OnTurretDestroyed;

    protected override void ImplementEffect(PlayerPickableCollision player)
    {
        if (player.TurretController.HasActiveTurret()) return;

        TurretBase tBase = turretPrefab.GetComponent<TurretBase>();

        if (player.TurretController.ContainsTurret(tBase.TurretName))
        {
            player.TurretController.OnWeaponPickedUp(tBase.TurretName);

            if (!shouldDisappear) return;
            OnPicked();
        }
        else
        {
            player.TurretController.OnNewWeaponPickedUp(turretPrefab);

            if (!shouldDisappear) return;
            OnPicked();
        }
    }

    protected override void NotifyDestruction()
    {
        OnTurretDestroyed?.Invoke(transform.parent);
    }
}
