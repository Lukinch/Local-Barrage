using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickable : Pickable
{
    [SerializeField] private float healAmount;

    public static event Action<Transform> OnHealthDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPickableCollision player = other.GetComponent<PlayerPickableCollision>();

        if (player != null)
        {
            if (!player.PlayerDamageController.HealthCanBeHealed()) return;

            player.PlayerDamageController.RestoreHealth(healAmount);
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
            OnHealthDestroyed?.Invoke(transform.parent);
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitForEvent(float seconds)
    {
        yield return new WaitForSeconds(seconds - 0.1f);
        OnHealthDestroyed?.Invoke(transform.parent);
    }
}
