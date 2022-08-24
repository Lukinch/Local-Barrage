using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    [Header("Selection Settings")]
    [SerializeField] private Image _sliderHandle;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _normalColor;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private Slider _sliderToUpdate;
    [SerializeField] private TextMeshProUGUI _textToUpdate;

    private float _maxSliderValue = 1;

    private void Start()
    {
        _sliderToUpdate.value = AudioSettingsManager.Instance.GetMixerVolume(_audioMixerGroup.name);

        UpdateText();
    }

    private void UpdateText()
    {
        float percentage = (_sliderToUpdate.value / _maxSliderValue) * 100;
        string toText = percentage.ToString("0");
        _textToUpdate.text = $"{toText}%";
    }

    public void SetVolume(float value)
    {
        AudioSettingsManager.Instance.SetMixerVolumeFromSlider(_audioMixerGroup.name, value);
        UpdateText();
    }

    public void SetSelectedColor()
    {
        _sliderHandle.color = _selectedColor;
    }
    public void SetDeselectedColor()
    {
        _sliderHandle.color = _normalColor;
    }
}
