
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelPickablesManager : MonoBehaviour
{
    [SerializeField] private LevelObjectiveScreenController _levelObjectiveScreenController;
    [Header("Locations")]
    [SerializeField] private Transform[] _turretLocations;
    [SerializeField] private Transform[] _statsLocations;
    [Header("Prefabs")]
    [SerializeField] private GameObject[] _turretPrefabs;
    [SerializeField] private GameObject[] _statsPrefabs;
    [Header("Settings")]
    [SerializeField] private int _turretsAtATime;
    [SerializeField] private int _statsAtATime;
    [SerializeField] private float _timeToStartSpawning;
    [SerializeField] private float timeBetweenSpawns;

    private Transform[] _statsLocationsBeingUsed;
    private Transform[] _turretsLocationsBeingUsed;
    private Queue<Processes> _queuedStatsProcesses;
    private Queue<Processes> _queuedTurretsProcesses;
    private int _currentAmountOfTurrets;
    private int _currentAmountOfStats;
    private bool _isStatSpawningTakingPlace;
    private bool _isTurretSpawningTakingPlace;

    private bool _shouldUpdateQueues;

    private void Awake()
    {
        InitializeQueuesAndArrays();

        HealthPickable.OnHealthDestroyed += OnStatRequested;
        ShieldPickable.OnShieldDestroyed += OnStatRequested;
        TurretPickable.OnTurretDestroyed += OnWeaponRequested;

        _levelObjectiveScreenController.OnObjectiveShown += InitializeSpawn;
    }
    private void OnDestroy()
    {
        HealthPickable.OnHealthDestroyed -= OnStatRequested;
        ShieldPickable.OnShieldDestroyed -= OnStatRequested;
        TurretPickable.OnTurretDestroyed -= OnWeaponRequested;

        _levelObjectiveScreenController.OnObjectiveShown -= InitializeSpawn;
    }

    private void Update()
    {
        if (!_shouldUpdateQueues) return;

        if (_queuedStatsProcesses.Count > 0)
        {
            if (_currentAmountOfStats >= _statsAtATime) return;
            if (_isStatSpawningTakingPlace) return;
            _queuedStatsProcesses.Dequeue();
            StartCoroutine(nameof(WaitForNextStatSpawn));
        }

        if (_queuedTurretsProcesses.Count > 0)
        {
            if (_currentAmountOfTurrets >= _turretsAtATime) return;
            if (_isTurretSpawningTakingPlace) return;
            _queuedTurretsProcesses.Dequeue();
            StartCoroutine(nameof(WaitForNextWeaponSpawn));
        }
    }

    private void InitializeQueuesAndArrays()
    {
        _queuedStatsProcesses = new Queue<Processes>();
        _queuedTurretsProcesses = new Queue<Processes>();

        _statsLocationsBeingUsed = new Transform[_statsLocations.Length];
        _turretsLocationsBeingUsed = new Transform[_turretLocations.Length];
    }

    private void InitializeSpawn()
    {
        StartCoroutine(nameof(WaitForSpawn));
    }

    private IEnumerator WaitForSpawn()
    {
        yield return new WaitForSeconds(_timeToStartSpawning);
        SpawnInitialPickables();
    }

    private void SpawnInitialPickables()
    {
        SpawnMultiplePrefabs(_statsAtATime, _statsLocations, _statsLocationsBeingUsed, _statsPrefabs, ref _currentAmountOfStats);
        SpawnMultiplePrefabs(_turretsAtATime, _turretLocations, _turretsLocationsBeingUsed, _turretPrefabs, ref _currentAmountOfTurrets);

        _shouldUpdateQueues = true;
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
        _currentAmountOfStats--;
        int index = Array.FindIndex(
            _statsLocationsBeingUsed,
            location => location == pickableParent);

        _statsLocationsBeingUsed[index] = null;
        
        _queuedStatsProcesses.Enqueue(Processes.StatSpawn);
    }

    private void OnWeaponRequested(Transform pickableParent)
    {
        _currentAmountOfTurrets--;
        int index = Array.FindIndex(
            _turretsLocationsBeingUsed,
            location => location == pickableParent);

        _turretsLocationsBeingUsed[index] = null;
        
        _queuedTurretsProcesses.Enqueue(Processes.WeaponSpawn);
    }

    private IEnumerator WaitForNextStatSpawn()
    {
        _isStatSpawningTakingPlace = true;
        yield return new WaitForSeconds(timeBetweenSpawns);

        SpawnSinglePrefab(_statsLocations, _statsLocationsBeingUsed, _statsPrefabs, ref _currentAmountOfStats);

        _isStatSpawningTakingPlace = false;
    }

    private IEnumerator WaitForNextWeaponSpawn()
    {
        _isTurretSpawningTakingPlace = true;
        yield return new WaitForSeconds(timeBetweenSpawns);

        SpawnSinglePrefab(_turretLocations, _turretsLocationsBeingUsed, _turretPrefabs, ref _currentAmountOfTurrets);

        _isTurretSpawningTakingPlace = false;
    }
}

enum Processes
{
    StatSpawn,
    WeaponSpawn
}
