
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettingsToggle : MonoBehaviour
{
    [SerializeField] private GraphicsSettingsSO _graphicsSettingsSO;
    [SerializeField] private Toggle _toggle;

    private void Start()
    {
        LoadGraphicsSettings();
    }

    private void LoadGraphicsSettings()
    {
        UpdateToggleValue(_graphicsSettingsSO.Enable60FpsCap);
    }

    private void UpdateToggleValue(bool enabled)
    {
        _toggle.isOn = enabled;
    }

    public void OnToggleChanged(bool enabled)
    {
        _graphicsSettingsSO.Enable60FpsCap = enabled;
    }
}
