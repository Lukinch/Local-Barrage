using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretTapFire : TurretBase
{
    [Header("This turret specific")]
    [SerializeField] private List<Transform> firingPoints;
    [Space(10)]
    [Tooltip("Pause time between each firing point Fire action")]
    [SerializeField] private float timeBetweenShots = 0.2f;
    [Tooltip("Time till the next Fire action is available")]
    [SerializeField] private float reloadTime = 0.3f;

    private TurretFiringController turretFiringController;

    private bool isReloading;
    private float currentReloadTime;

    public event Action<float> OnReloadTimeChanged;
    
    private void OnEnable()
    {
        if (turretFiringController == null)
        {
            turretFiringController = GetComponentInParent<TurretFiringController>();
        }

        turretFiringController.onFireInstantPerformed += Fire;
    }
    
    private void OnDisable()
    {
        turretFiringController.onFireInstantPerformed -= Fire;
    }

    // public void OnFireInstant(InputAction.CallbackContext context)
    // {
    //     if (context.performed) Fire();
    // }

    protected override void Fire()
    {
        if (isReloading) return;

        isReloading = true;
        StartCoroutine(nameof(FireFromFirginPoints));
    }

    private IEnumerator FireFromFirginPoints()
    {
        foreach (Transform firingPoint in firingPoints)
        {
            FireProjectile(firingPoint);
            yield return new WaitForSeconds(timeBetweenShots);
        }

        StartCoroutine(nameof(WaitForRealod));
    }

    private IEnumerator WaitForRealod()
    {
        while (isReloading && currentReloadTime <= reloadTime)
        {
            currentReloadTime += 0.02f;

            OnReloadTimeChanged?.Invoke(currentReloadTime / reloadTime);

            if (currentReloadTime >= reloadTime)
            {
                currentReloadTime = 0;
                OnReloadTimeChanged?.Invoke(0);
                isReloading = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
