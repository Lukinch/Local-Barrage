using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTapFire : TurretBase
{
    #region Serialized Fieds
    [Header("This turret specific")]
    [SerializeField] private List<Transform> firingPoints;
    [Space(10)]
    [Tooltip("Pause time between each firing point Fire action")]
    [SerializeField] private float timeBetweenShots = 0.2f;
    [Tooltip("Time till the next Fire action is available")]
    [SerializeField] private float reloadTime = 0.3f;
    #endregion

    #region Private variables
    private bool isReloading;
    private float currentReloadTime;
    #endregion

    #region Public variables and Actions
    public event Action<float> OnReloadTimeChanged;
    #endregion

    #region Initialize
    private void OnEnable() => SubscribeToInputs();
    private void OnDisable()
    {
        DisableAllInputs();
        StopAllCoroutines();
    }
    #endregion

    #region Subscription Handling
    private void SubscribeToInputs()
    {
        inputReader.FireInstantEvent += OnFireInstantPerformed;
    }

    private void DisableAllInputs()
    {
        inputReader.FireInstantEvent -= OnFireInstantPerformed;
    }
    #endregion

    #region Actions calling
    private void OnFireInstantPerformed() => Fire();
    #endregion

    #region Tap To Fire Logic
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


    #endregion
}
