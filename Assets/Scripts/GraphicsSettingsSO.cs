
using UnityEngine;

[CreateAssetMenu(menuName = "GraphicsSettingsSO")]
public class GraphicsSettingsSO : ScriptableObject
{
    [SerializeField] private bool _enable60FpsCap = true;

    public bool Enable60FpsCap
    {
        get => _enable60FpsCap;
        set
        {
            _enable60FpsCap = value;
            int framesAmount = GetIntFromBoolFramerate(value);
            Application.targetFrameRate = framesAmount;
            PlayerPrefs.SetInt("Enable60FpsCap", framesAmount);
        }
    }

    private void OnEnable()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        LoadGraphicsSettings();
    }

    private void LoadGraphicsSettings()
    {
        if (!PlayerPrefs.HasKey("Enable60FpsCap")) return;

        Enable60FpsCap = GetBoolFromIntFramerate(PlayerPrefs.GetInt("Enable60FpsCap"));
    }

    private int GetIntFromBoolFramerate(bool value)
    {
        return value == true ? 60 : -1;
    }

    private bool GetBoolFromIntFramerate(int value)
    {
        return value == 60 ? true : false;
    }
}
