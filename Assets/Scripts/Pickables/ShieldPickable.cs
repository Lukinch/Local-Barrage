using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickable : Pickable
{
    [SerializeField] private float shieldAmount;

    public static event Action<Transform> OnShieldDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPickableCollision player = other.GetComponent<PlayerPickableCollision>();

        if (player != null)
        {
            if (!player.PlayerDamageController.ShieldCanBeHealed()) return;

            player.PlayerDamageController.RestoreShield(shieldAmount);
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
            OnShieldDestroyed?.Invoke(transform.parent);
            Destroy(gameObject);
        }
    }

    private IEnumerator WaitForEvent(float seconds)
    {
        yield return new WaitForSeconds(seconds - 0.1f);
        OnShieldDestroyed?.Invoke(transform.parent);
    }
}
