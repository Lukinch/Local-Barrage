
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class LevelPickablesManager : MonoBehaviour
{
    [SerializeField] private List<Transform> weaponsLocations;
    [SerializeField] private List<Transform> statsLocations;
    [SerializeField] private List<GameObject> weaponPrefabs;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private int weaponsAtATime = 3;
    [SerializeField] private int healsAtATime = 2;
    [SerializeField] private int shieldsAtATime = 2;
    [SerializeField] private float timeToStartSpawning = 3f;
    [SerializeField] private float timeBetweenSpawns = 3f;

    private int currentAmountOfHeals;
    private int currentAmountOfShields;
    private int currentAmountOfWeapons;
    private Transform[] statsLocationsBeingUsed;
    private Transform[] weaponsLocationsBeingUsed;
    private Queue<Processes> queuedStatsProcesses;
    private Queue<Processes> queuedWeaponsProcesses;
    private bool isStatSpawningTakingPlace;
    private bool isWeaponSpawningTakingPlace;

    private void Awake()
    {
        InitializeQueuesAndArrays();

        FixAmountOfStatsAtATime();

        HealthPickable.OnHealthDestroyed += OnNewHealRequested;
        ShieldPickable.OnShieldDestroyed += OnNewShieldRequested;
        TurretPickable.OnTurretDestroyed += OnNewWeaponRequested;
    }

    private void Start()
    {
        StartCoroutine(nameof(WaitForSpawn));
    }

    private void Update()
    {
        if (queuedStatsProcesses.Count > 0)
        {
            if (isStatSpawningTakingPlace) return;
            StartCoroutine(nameof(WaitForNextStatSpawn), queuedStatsProcesses.Dequeue());
        }

        if (queuedWeaponsProcesses.Count > 0)
        {
            if (isWeaponSpawningTakingPlace) return;
            queuedWeaponsProcesses.Dequeue();
            StartCoroutine(nameof(WaitForNextWeaponSpawn));
        }
    }

    private void InitializeQueuesAndArrays()
    {
        queuedStatsProcesses = new Queue<Processes>();
        queuedWeaponsProcesses = new Queue<Processes>();

        statsLocationsBeingUsed = new Transform[statsLocations.Count];
        weaponsLocationsBeingUsed = new Transform[weaponsLocations.Count];
    }

    private void FixAmountOfStatsAtATime()
    {
        if (statsLocations.Count % 2 == 0)
        {
            int total = healsAtATime + shieldsAtATime;
            if (total >= statsLocations.Count)
            {
                healsAtATime = shieldsAtATime = statsLocations.Count / 2;
            }
        }
        else
        {
            int total = healsAtATime + shieldsAtATime;
            if (total >= statsLocations.Count)
            {
                healsAtATime = shieldsAtATime = (statsLocations.Count - 1) / 2;
            }
        }
    }

    private void OnNewHealRequested(Transform parent)
    {
        currentAmountOfHeals--;
        int index = Array.FindIndex(
            statsLocationsBeingUsed,
            location => location == parent);

        statsLocationsBeingUsed[index] = null;

        queuedStatsProcesses.Enqueue(Processes.HealSpawn);
    }

    private void OnNewShieldRequested(Transform parent)
    {
        currentAmountOfShields--;
        int index = Array.FindIndex(
            statsLocationsBeingUsed,
            location => location == parent);

        statsLocationsBeingUsed[index] = null;
        
        queuedStatsProcesses.Enqueue(Processes.ShieldSpawn);
    }

    private void OnNewWeaponRequested(Transform parent)
    {
        currentAmountOfWeapons--;
        int index = Array.FindIndex(
            weaponsLocationsBeingUsed,
            location => location == parent);

        weaponsLocationsBeingUsed[index] = null;
        
        queuedWeaponsProcesses.Enqueue(Processes.WeaponSpawn);
    }

    private IEnumerator WaitForNextStatSpawn(Processes process)
    {
        isStatSpawningTakingPlace = true;
        yield return new WaitForSeconds(timeBetweenSpawns);

        switch (process)
        {
            case Processes.HealSpawn:
                SpawnHealth();
                break;
            case Processes.ShieldSpawn:
                SpawnShield();
                break;
        }

        isStatSpawningTakingPlace = false;
    }

    private IEnumerator WaitForNextWeaponSpawn()
    {
        isWeaponSpawningTakingPlace = true;
        yield return new WaitForSeconds(timeBetweenSpawns);

        SpawnWeapon();

        isWeaponSpawningTakingPlace = false;
    }

    private void SpawnHealth()
    {
        if (currentAmountOfHeals >= healsAtATime) return;
        
        int index = Random.Range(0, statsLocations.Count - 1);

        while (statsLocationsBeingUsed[index] != null)
        {
            index = Random.Range(0, statsLocations.Count - 1);
        }

        var heal = Instantiate(healthPrefab, statsLocations[index]);
        statsLocationsBeingUsed[index] = statsLocations[index];
        currentAmountOfHeals++;
    }

    private void SpawnShield()
    {
        if (currentAmountOfShields >= shieldsAtATime) return;
        
        int index = Random.Range(0, statsLocations.Count - 1);

        while (statsLocationsBeingUsed[index] != null)
        {
            index = Random.Range(0, statsLocations.Count - 1);
        }

        var shield = Instantiate(shieldPrefab, statsLocations[index]);
        statsLocationsBeingUsed[index] = statsLocations[index];
        currentAmountOfShields++;
    }

    private void SpawnWeapon()
    {
        if (currentAmountOfWeapons >= weaponsAtATime) return;
        
        int index = Random.Range(0, weaponsLocations.Count - 1);

        while (weaponsLocationsBeingUsed[index] != null)
        {
            index = Random.Range(0, weaponsLocations.Count - 1);
        }

        var weapon = Instantiate(
            weaponPrefabs[UnityEngine.Random.Range(0, weaponPrefabs.Count)],
            weaponsLocations[index]);
        weaponsLocationsBeingUsed[index] = weaponsLocations[index];
        currentAmountOfWeapons++;
    }

    #region Initialize
    private void InitializeSpawning()
    {
        InitializeHeals();
        InitializeShields();
        InitializeWeapons();
    }

    private void InitializeHeals()
    {
        for (int i = 0; i < healsAtATime; i++)
        {
            int index = Random.Range(0, statsLocations.Count - 1);

            while (statsLocationsBeingUsed[index] != null)
            {
                index = Random.Range(0, statsLocations.Count - 1);
            }

            var heal = Instantiate(healthPrefab, statsLocations[index]);
            statsLocationsBeingUsed[index] = statsLocations[index];
            currentAmountOfHeals++;
        }
    }

    private void InitializeShields()
    {
        for (int i = 0; i < shieldsAtATime; i++)
        {
            int index = Random.Range(0, statsLocations.Count - 1);
            while (statsLocationsBeingUsed[index] != null)
            {
                index = Random.Range(0, statsLocations.Count - 1);
            }

            var shield = Instantiate(shieldPrefab, statsLocations[index]);
            statsLocationsBeingUsed[index] = statsLocations[index];
            currentAmountOfShields++;
        }
    }

    private void InitializeWeapons()
    {
        for (int i = 0; i < weaponsAtATime; i++)
        {
            int index = Random.Range(0, weaponsLocations.Count - 1);
            while (weaponsLocationsBeingUsed[index] != null)
            {
                index = Random.Range(0, weaponsLocations.Count - 1);
            }

            var weapon = Instantiate(
                weaponPrefabs[UnityEngine.Random.Range(0, weaponPrefabs.Count)],
                weaponsLocations[index]);
            weaponsLocationsBeingUsed[index] = weaponsLocations[index];
            currentAmountOfWeapons++;
        }
    }

    private IEnumerator WaitForSpawn()
    {
        yield return new WaitForSeconds(timeToStartSpawning);
        InitializeSpawning();
    }
    #endregion
}

enum Processes
{
    HealSpawn,
    ShieldSpawn,
    WeaponSpawn
}
