
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretController : MonoBehaviour
{
    [SerializeField] private Transform turretSpawnPosition;
    [SerializeField] private float newTurretDuration = 5f;

    private List<TurretBase> availableTurrets;
    private GameObject currentEnabledTurret;
    private Coroutine countdownCoroutine;
    private bool hasActiveTurret;
    private float currentTimer;

    public float BuffDuration { get => newTurretDuration; }
    public float CurrentTimer { get => currentTimer; }
    public event Action OnBuffStarted;

    public event Action<TurretHoldFire> OnHoldTurretPicked;
    public event Action<TurretChargeFire> OnChargeTurretPicked;
    public event Action<TurretTapFire> OnTapTurretPicked;

    private void Awake()
    {
        availableTurrets = new List<TurretBase>();
        availableTurrets.Add(GetComponentInChildren<TurretBase>());

        currentEnabledTurret = availableTurrets[0].gameObject;

        currentTimer = newTurretDuration;
        hasActiveTurret = false;
    }

    private void Start()
    {
        OnHoldTurretPicked?.Invoke(currentEnabledTurret.GetComponent<TurretHoldFire>());
    }

    private IEnumerator StartBuffCountDown()
    {
        OnBuffStarted?.Invoke();
        while (currentTimer > 0)
        {
            yield return new WaitForSeconds(newTurretDuration);
            currentTimer = 0;
        }

        SetToDefaultTurret();
    }

    public void SetToDefaultTurret()
    {
        countdownCoroutine = null;

        currentEnabledTurret.SetActive(false);

        currentEnabledTurret = availableTurrets[0].gameObject;
        OnHoldTurretPicked?.Invoke(currentEnabledTurret.GetComponent<TurretHoldFire>());

        currentEnabledTurret.SetActive(true);

        hasActiveTurret = false;
        currentTimer = newTurretDuration;
    }

    public void OnWeaponPickedUp(string turretName)
    {
        if (hasActiveTurret) return;

        hasActiveTurret = true;

        GameObject newTurret = availableTurrets.Find(x => x.TurretName == turretName).gameObject;

        currentEnabledTurret.SetActive(false);

        currentEnabledTurret = newTurret;

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
        currentEnabledTurret.SetActive(true);

        countdownCoroutine = StartCoroutine(nameof(StartBuffCountDown));
    }

    public void OnNewWeaponPickedUp(GameObject turret)
    {
        if (hasActiveTurret) return;
        
        hasActiveTurret = true;

        GameObject newTurret = Instantiate(turret, turretSpawnPosition);

        availableTurrets.Add(newTurret.GetComponent<TurretBase>());

        currentEnabledTurret.SetActive(false);

        currentEnabledTurret = newTurret;

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

        currentEnabledTurret.SetActive(true);

        countdownCoroutine = StartCoroutine(nameof(StartBuffCountDown));
    }

    public bool ContainsTurret(string turretName)
    {
        foreach (TurretBase turretBase in availableTurrets)
        {
            if (turretBase.TurretName.Equals(turretName)) return true;
        }
        
        return false;
    }

    public bool HasActiveTurret()
    {
        return hasActiveTurret;
    }
}
