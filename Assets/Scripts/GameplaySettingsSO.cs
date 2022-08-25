
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "GameplaySettingsSO")]
public class GameplaySettingsSO : ScriptableObject
{
    [SerializeField] private int _timeBeforeSave;
    [SerializeField][Range(2, 5)] private int _timeToStartNewGame = 3;
    [SerializeField][Range(2, 5)] private int _timeToStartLevel = 5;
    [SerializeField][Range(2, 5)] private int _timeTillNextLevel = 5;
    [SerializeField][Range(5, 25)] private int _amountOfPointsPerWin = 25;
    [SerializeField][Range(25, 100)] private int _amountOfPointsToWin = 50;
    [SerializeField][Range(1, 3)] private int _amountOfDummies = 3;

    private Coroutine _saveCoroutine;

    public int TimeToStartNewGame
    {
        get => _timeToStartNewGame;
        set
        {
            _timeToStartNewGame = value;
            PlayerPrefs.SetInt("TimeToStartNewGame", _timeToStartNewGame);
        }
    }
    public int TimeToStartLevel
    {
        get => _timeToStartLevel;
        set
        {
            _timeToStartLevel = value;
            PlayerPrefs.SetInt("TimeToStartLevel", _timeToStartLevel);
        }
    }
    public int TimeTillNextLevel
    {
        get => _timeTillNextLevel;
        set
        {
            _timeTillNextLevel = value;
            PlayerPrefs.SetInt("TimeTillNextLevel", _timeTillNextLevel);
        }
    }
    public int AmountOfPointsPerWin
    {
        get => _amountOfPointsPerWin;
        set
        {
            _amountOfPointsPerWin = value;
            PlayerPrefs.SetInt("AmountOfPointsPerWin", _amountOfPointsPerWin);
        }
    }
    public int AmountOfPointsToWin
    {
        get => _amountOfPointsToWin;
        set
        {
            _amountOfPointsToWin = value;
            PlayerPrefs.SetInt("AmountOfPointsToWin", _amountOfPointsToWin);
        }
    }
    public int AmountOfDummies
    {
        get => _amountOfDummies;
        set
        {
            _amountOfDummies = value;
            PlayerPrefs.SetInt("AmountOfDummies", _amountOfDummies);
        }
    }

    private void OnEnable()
    {
        LoadGameplaySettings();
    }

    private void LoadGameplaySettings()
    {
        if (!PlayerPrefs.HasKey("TimeToStartNewGame")) return;

        _timeToStartNewGame = PlayerPrefs.GetInt("TimeToStartNewGame");
        _timeToStartLevel = PlayerPrefs.GetInt("TimeToStartLevel");
        _timeTillNextLevel = PlayerPrefs.GetInt("TimeTillNextLevel");
        _amountOfPointsPerWin = PlayerPrefs.GetInt("AmountOfPointsPerWin");
        _amountOfPointsToWin = PlayerPrefs.GetInt("AmountOfPointsToWin");
        _amountOfDummies = PlayerPrefs.GetInt("AmountOfDummies");
    }
}
