using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTapFire : TurretBase
{
    [Header("This turret specific")]
    [SerializeField] private List<Transform> firingPoints;
    [Space(10)]
    [Tooltip("Scriptable Object containing the turret stats")]
    [SerializeField] private TurretTapFireStatsSO turretTapFireStatsSO;

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
        isReloading = false;
        currentReloadTime = 0;
        
        turretFiringController.onFireInstantPerformed -= Fire;
    }

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
            FireProjectile(firingPoint, turretTapFireStatsSO.damagePerShot, turretTapFireStatsSO.projectileForce);
            yield return new WaitForSeconds(turretTapFireStatsSO.timeBetweenShots);
        }

        StartCoroutine(nameof(WaitForRealod));
    }

    private IEnumerator WaitForRealod()
    {
        while (isReloading && currentReloadTime <= turretTapFireStatsSO.reloadTime)
        {
            currentReloadTime += 0.02f;

            OnReloadTimeChanged?.Invoke(currentReloadTime / turretTapFireStatsSO.reloadTime);

            if (currentReloadTime >= turretTapFireStatsSO.reloadTime)
            {
                currentReloadTime = 0;
                OnReloadTimeChanged?.Invoke(0);
                isReloading = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
