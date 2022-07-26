using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPickable : Pickable
{
    [SerializeField] private GameObject turretPrefab;

    public static event Action<Transform> OnTurretDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPickableCollision player = other.GetComponent<PlayerPickableCollision>();

        if (player == null) return;
        if (player.TurretController.HasActiveTurret()) return;

        TurretBase tBase = turretPrefab.GetComponent<TurretBase>();

        if (player.TurretController.ContainsTurret(tBase.TurretName))
        {
            player.TurretController.OnWeaponPickedUp(tBase.TurretName);
            OnPicked();
        }
        else
        {
            player.TurretController.OnNewWeaponPickedUp(turretPrefab);
            OnPicked();
        }
    }

    protected override void OnPicked()
    {
        if (pickupSfx)
        {
            CreateSFX(pickupSfx, transform.position, 3f, 0f);
            StartCoroutine(nameof(WaitForEvent), pickupSfx.length);
        }

        if (pickupVfx)
        {
            Instantiate(pickupVfx, transform.position, Quaternion.identity);
        }

        if (!pickupSfx)
        {
            OnTurretDestroyed?.Invoke(transform.parent);
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitForEvent(float seconds)
    {
        yield return new WaitForSeconds(seconds - 0.1f);
        OnTurretDestroyed?.Invoke(transform.parent);
    }
}
