using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTapFire : TurretBase
{
    [Header("This turret specific")]
    [SerializeField] private List<Transform> _firingPoints;
    [Space(10)]
    [Tooltip("Scriptable Object containing the turret stats")]
    [SerializeField] private TurretTapFireStatsSO _turretTapFireStatsSO;

    private bool _isReloading;
    private float _currentReloadTime;

    public event Action<float> OnReloadTimeChanged;

    private void OnEnable()
    {
        if (turretFiringController == null)
        {
            turretFiringController = GetComponentInParent<TurretFiringController>();
        }

        turretFiringController.OnFireInstantPerformed += Fire;
    }

    private void OnDisable()
    {
        _isReloading = false;
        _currentReloadTime = 0;

        turretFiringController.OnFireInstantPerformed -= Fire;
    }

    protected override void Fire()
    {
        if (_isReloading) return;

        _isReloading = true;
        StartCoroutine(nameof(FireFromFiringPoints));
    }

    private IEnumerator FireFromFiringPoints()
    {
        foreach (Transform firingPoint in _firingPoints)
        {
            FireProjectile(
                firingPoint,
                _turretTapFireStatsSO.damagePerShot,
                _turretTapFireStatsSO.projectileForce);

            yield return new WaitForSeconds(_turretTapFireStatsSO.timeBetweenShots);
        }

        StartCoroutine(nameof(WaitForReload));
    }

    private IEnumerator WaitForReload()
    {
        while (_isReloading && _currentReloadTime <= _turretTapFireStatsSO.reloadTime)
        {
            _currentReloadTime += 0.02f;

            OnReloadTimeChanged?.Invoke(_currentReloadTime / _turretTapFireStatsSO.reloadTime);

            if (_currentReloadTime >= _turretTapFireStatsSO.reloadTime)
            {
                _currentReloadTime = 0;
                OnReloadTimeChanged?.Invoke(0);
                _isReloading = false;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
