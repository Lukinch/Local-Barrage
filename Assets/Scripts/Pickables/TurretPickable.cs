using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPickable : Pickable
{
    [SerializeField] bool _shouldDisappear;
    [SerializeField] private GameObject _turretPrefab;

    public static event Action<Transform> OnTurretDestroyed;

    protected override void ImplementEffect(PlayerPickableCollision player)
    {
        if (player.TurretController.HasActiveTurret()) return;

        TurretBase tBase = _turretPrefab.GetComponent<TurretBase>();

        if (player.TurretController.ContainsTurret(tBase.TurretName))
        {
            player.TurretController.OnWeaponPickedUp(tBase.TurretName);

            if (!_shouldDisappear) return;
            OnPicked();
        }
        else
        {
            player.TurretController.OnNewWeaponPickedUp(_turretPrefab);

            if (!_shouldDisappear) return;
            OnPicked();
        }
    }

    protected override void NotifyDestruction()
    {
        OnTurretDestroyed?.Invoke(transform.parent);
    }
}
