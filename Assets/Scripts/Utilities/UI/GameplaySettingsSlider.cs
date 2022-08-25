using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplaySettingsSlider : MonoBehaviour
{
    [SerializeField] private GameplaySettingsSO _gameplaySettings;
    [SerializeField] private GameplaySetting _settingToUpdate;
    [SerializeField] private Slider _sliderToUpdate;
    [SerializeField] private TextMeshProUGUI _textToUpdate;

    private void Start()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        switch (_settingToUpdate)
        {
            case GameplaySetting.TimeToStartNewGame:
                UpdateSliderValueAndText(_gameplaySettings.TimeToStartNewGame);
                break;
            case GameplaySetting.TimeToStartLevel:
                UpdateSliderValueAndText(_gameplaySettings.TimeToStartLevel);
                break;
            case GameplaySetting.TimeTillNextLevel:
                UpdateSliderValueAndText(_gameplaySettings.TimeTillNextLevel);
                break;
            case GameplaySetting.AmountOfPointsPerWin:
                UpdateSliderValueAndText(_gameplaySettings.AmountOfPointsPerWin);
                break;
            case GameplaySetting.AmountOfPointsToWin:
                UpdateSliderValueAndText(_gameplaySettings.AmountOfPointsToWin);
                break;
            case GameplaySetting.AmountOfDummies:
                UpdateSliderValueAndText(_gameplaySettings.AmountOfDummies);
                break;
        }
    }

    private void UpdateSliderValueAndText(int value)
    {
        _sliderToUpdate.value = value;
        _textToUpdate.text = $"{value}";
    }

    private void OnTimeStartNewGameChanged(int value)
    {
        _gameplaySettings.TimeToStartNewGame = value;
        _textToUpdate.text = $"{value}";
    }
    private void OnTimeToStartLevelChanged(int value)
    {
        _gameplaySettings.TimeToStartLevel = value;
        _textToUpdate.text = $"{value}";
    }
    private void OnTimeTillNextLevelChanged(int value)
    {
        _gameplaySettings.TimeTillNextLevel = value;
        _textToUpdate.text = $"{value}";
    }
    private void OnAmountOfPointsPerWinChanged(int value)
    {
        _gameplaySettings.AmountOfPointsPerWin = value;
        _textToUpdate.text = $"{value}";
    }
    private void OnAmountOfPointsToWinChanged(int value)
    {
        _gameplaySettings.AmountOfPointsToWin = value;
        _textToUpdate.text = $"{value}";
    }
    private void OnAmountOfDummiesChanged(int value)
    {
        _gameplaySettings.AmountOfDummies = value;
        _textToUpdate.text = $"{value}";
    }

    public void OnSliderValueChanged(float value)
    {
        int intValue = Mathf.RoundToInt(value);
        switch (_settingToUpdate)
        {
            case GameplaySetting.TimeToStartNewGame:
                OnTimeStartNewGameChanged(intValue);
                break;
            case GameplaySetting.TimeToStartLevel:
                OnTimeToStartLevelChanged(intValue);
                break;
            case GameplaySetting.TimeTillNextLevel:
                OnTimeTillNextLevelChanged(intValue);
                break;
            case GameplaySetting.AmountOfPointsPerWin:
                OnAmountOfPointsPerWinChanged(intValue);
                break;
            case GameplaySetting.AmountOfPointsToWin:
                OnAmountOfPointsToWinChanged(intValue);
                break;
            case GameplaySetting.AmountOfDummies:
                OnAmountOfDummiesChanged(intValue);
                break;
        }
    }

    enum GameplaySetting
    {
        TimeToStartNewGame,
        TimeToStartLevel,
        TimeTillNextLevel,
        AmountOfPointsPerWin,
        AmountOfPointsToWin,
        AmountOfDummies
    }
}
