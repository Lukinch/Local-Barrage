
using UnityEngine;

[CreateAssetMenu(menuName = "GameplaySettingsSO")]
public class GameplaySettingsSO : ScriptableObject
{
    [Range(2, 5)] public int TimeToStartNewGame = 3;
    [Range(2, 5)] public int TimeToStartLevel = 5;
    [Range(2, 5)] public int TimeTillNextLevel = 5;
    [Range(5, 25)] public int AmountOfPointsPerWin = 25;
    [Range(25, 100)] public int AmountOfPointsToWin = 50;
    [Range(1, 3)] public int AmountOfDummies = 3;
}
