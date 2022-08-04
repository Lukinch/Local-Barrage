
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelPickablesManager : MonoBehaviour
{
    [SerializeField] private LevelObjectiveScreenController levelObjectiveScreenController;
    [Header("Locations")]
    [SerializeField] private Transform[] turretLocations;
    [SerializeField] private Transform[] statsLocations;
    [Header("Prefabs")]
    [SerializeField] private GameObject[] turretPrefabs;
    [SerializeField] private GameObject[] statsPrefabs;
    [Header("Settings")]
    [SerializeField] private int turretsAtATime;
    [SerializeField] private int statsAtATime;
    [SerializeField] private float timeToStartSpawning;
    [SerializeField] private float timeBetweenSpawns;

    private Transform[] statsLocationsBeingUsed;
    private Transform[] turretsLocationsBeingUsed;
    private Queue<Processes> queuedStatsProcesses;
    private Queue<Processes> queuedTurretsProcesses;
    [SerializeField] private int currentAmountOfTurrets;
    [SerializeField] private int currentAmountOfStats;
    private bool isStatSpawningTakingPlace;
    private bool isTurretSpawningTakingPlace;

    private void Awake()
    {
        InitializeQueuesAndArrays();

        HealthPickable.OnHealthDestroyed += OnStatRequested;
        ShieldPickable.OnShieldDestroyed += OnStatRequested;
        TurretPickable.OnTurretDestroyed += OnWeaponRequested;

        levelObjectiveScreenController.OnObjectiveShown += InitializeSpawn;
    }

    private void Update()
    {
        if (queuedStatsProcesses.Count > 0)
        {
            if (currentAmountOfStats >= statsAtATime) return;
            if (isStatSpawningTakingPlace) return;
            queuedStatsProcesses.Dequeue();
            StartCoroutine(nameof(WaitForNextStatSpawn));
        }

        if (queuedTurretsProcesses.Count > 0)
        {
            if (currentAmountOfTurrets >= turretsAtATime) return;
            if (isTurretSpawningTakingPlace) return;
            queuedTurretsProcesses.Dequeue();
            StartCoroutine(nameof(WaitForNextWeaponSpawn));
        }
    }

    private void InitializeQueuesAndArrays()
    {
        queuedStatsProcesses = new Queue<Processes>();
        queuedTurretsProcesses = new Queue<Processes>();

        statsLocationsBeingUsed = new Transform[statsLocations.Length];
        turretsLocationsBeingUsed = new Transform[turretLocations.Length];
    }

    private void InitializeSpawn()
    {
        StartCoroutine(nameof(WaitForSpawn));
    }

    private IEnumerator WaitForSpawn()
    {
        yield return new WaitForSeconds(timeToStartSpawning);
        Initialize();
    }

    private void Initialize()
    {
        SpawnMultiplePrefabs(statsAtATime, statsLocations, statsLocationsBeingUsed, statsPrefabs, ref currentAmountOfStats);
        SpawnMultiplePrefabs(turretsAtATime, turretLocations, turretsLocationsBeingUsed, turretPrefabs, ref currentAmountOfTurrets);
    }

    private void SpawnSinglePrefab(Transform[] locations, Transform[] locationsUsed, GameObject[] prefabs, ref int trackAmount)
    {
        int index = Random.Range(0, locations.Length);
        while (locationsUsed[index] != null)
        {
            index = Random.Range(0, locations.Length);
        }

        int prebaIndex = Random.Range(0, prefabs.Length);
        Instantiate(prefabs[prebaIndex], locations[index]);
        locationsUsed[index] = locations[index];
        trackAmount++;
    }

    private void SpawnMultiplePrefabs(int limitAmount, Transform[] locations, Transform[] locationsUsed, GameObject[] prefabs, ref int trackAmount)
    {
        for (int i = 0; i < limitAmount; i++)
        {
            int index = Random.Range(0, locations.Length);
            while (locationsUsed[index] != null)
            {
                index = Random.Range(0, locations.Length);
            }

            int prebaIndex = Random.Range(0, prefabs.Length);
            Instantiate(prefabs[prebaIndex], locations[index]);
            locationsUsed[index] = locations[index];
            trackAmount++;
        }
    }

    private void OnStatRequested(Transform pickableParent)
    {
        currentAmountOfStats--;
        int index = Array.FindIndex(
            statsLocationsBeingUsed,
            location => location == pickableParent);

        statsLocationsBeingUsed[index] = null;
        
        queuedStatsProcesses.Enqueue(Processes.StatSpawn);
    }

    private void OnWeaponRequested(Transform pickableParent)
    {
        currentAmountOfTurrets--;
        int index = Array.FindIndex(
            turretsLocationsBeingUsed,
            location => location == pickableParent);

        turretsLocationsBeingUsed[index] = null;
        
        queuedTurretsProcesses.Enqueue(Processes.WeaponSpawn);
    }

    private IEnumerator WaitForNextStatSpawn()
    {
        isStatSpawningTakingPlace = true;
        yield return new WaitForSeconds(timeBetweenSpawns);

        SpawnSinglePrefab(statsLocations, statsLocationsBeingUsed, statsPrefabs, ref currentAmountOfStats);

        isStatSpawningTakingPlace = false;
    }

    private IEnumerator WaitForNextWeaponSpawn()
    {
        isTurretSpawningTakingPlace = true;
        yield return new WaitForSeconds(timeBetweenSpawns);

        SpawnSinglePrefab(turretLocations, turretsLocationsBeingUsed, turretPrefabs, ref currentAmountOfTurrets);

        isTurretSpawningTakingPlace = false;
    }
}

enum Processes
{
    StatSpawn,
    WeaponSpawn
}
