
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretController : MonoBehaviour
{
    [SerializeField] private Transform _turretSpawnPosition;
    [SerializeField] private float _newTurretDuration = 5f;

    private List<TurretBase> _availableTurrets;
    private GameObject _currentEnabledTurret;
    private Coroutine _countdownCoroutine;
    private bool _hasActiveTurret;
    private float _currentTimer;

    public float BuffDuration { get => _newTurretDuration; }
    public float CurrentTimer { get => _currentTimer; }
    public event Action OnBuffStarted;

    public event Action<TurretHoldFire> OnHoldTurretPicked;
    public event Action<TurretChargeFire> OnChargeTurretPicked;
    public event Action<TurretTapFire> OnTapTurretPicked;

    private void Awake()
    {
        _availableTurrets = new List<TurretBase>();
        _availableTurrets.Add(GetComponentInChildren<TurretBase>());

        _currentEnabledTurret = _availableTurrets[0].gameObject;

        _currentTimer = _newTurretDuration;
        _hasActiveTurret = false;
    }

    private void Start()
    {
        OnHoldTurretPicked?.Invoke(_currentEnabledTurret.GetComponent<TurretHoldFire>());
    }

    private IEnumerator StartBuffCountDown()
    {
        OnBuffStarted?.Invoke();
        while (_currentTimer > 0)
        {
            yield return new WaitForSeconds(_newTurretDuration);
            _currentTimer = 0;
        }

        SetToDefaultTurret();
    }

    public void SetToDefaultTurret()
    {
        _countdownCoroutine = null;

        _currentEnabledTurret.SetActive(false);

        _currentEnabledTurret = _availableTurrets[0].gameObject;
        OnHoldTurretPicked?.Invoke(_currentEnabledTurret.GetComponent<TurretHoldFire>());

        _currentEnabledTurret.SetActive(true);

        _hasActiveTurret = false;
        _currentTimer = _newTurretDuration;
    }

    public void OnWeaponPickedUp(string turretName)
    {
        if (_hasActiveTurret) return;

        _hasActiveTurret = true;

        GameObject newTurret = _availableTurrets.Find(x => x.TurretName == turretName).gameObject;

        _currentEnabledTurret.SetActive(false);

        _currentEnabledTurret = newTurret;

        TurretType type = newTurret.GetComponent<TurretBase>().TurretType;
        switch (type)
        {
            case TurretType.Hold:
                OnHoldTurretPicked?.Invoke(newTurret.GetComponent<TurretHoldFire>());
                break;
            case TurretType.Charge:
                OnChargeTurretPicked?.Invoke(newTurret.GetComponent<TurretChargeFire>());
                break;
            case TurretType.Tap:
                OnTapTurretPicked?.Invoke(newTurret.GetComponent<TurretTapFire>());
                break;
        }
        _currentEnabledTurret.SetActive(true);

        _countdownCoroutine = StartCoroutine(nameof(StartBuffCountDown));
    }

    public void OnNewWeaponPickedUp(GameObject turret)
    {
        if (_hasActiveTurret) return;
        
        _hasActiveTurret = true;

        GameObject newTurret = Instantiate(turret, _turretSpawnPosition);

        _availableTurrets.Add(newTurret.GetComponent<TurretBase>());

        _currentEnabledTurret.SetActive(false);

        _currentEnabledTurret = newTurret;

        TurretType type = newTurret.GetComponent<TurretBase>().TurretType;
        switch (type)
        {
            case TurretType.Hold:
                OnHoldTurretPicked?.Invoke(newTurret.GetComponent<TurretHoldFire>());
                break;
            case TurretType.Charge:
                OnChargeTurretPicked?.Invoke(newTurret.GetComponent<TurretChargeFire>());
                break;
            case TurretType.Tap:
                OnTapTurretPicked?.Invoke(newTurret.GetComponent<TurretTapFire>());
                break;
        }

        _currentEnabledTurret.SetActive(true);

        _countdownCoroutine = StartCoroutine(nameof(StartBuffCountDown));
    }

    public bool ContainsTurret(string turretName)
    {
        foreach (TurretBase turretBase in _availableTurrets)
        {
            if (turretBase.TurretName.Equals(turretName)) return true;
        }
        
        return false;
    }

    public bool HasActiveTurret()
    {
        return _hasActiveTurret;
    }
}
