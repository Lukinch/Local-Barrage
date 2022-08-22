using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private float _timeToWaitForSave;

    private float _defaultVolume = 0.70f;
    public float MasterVolume { get; set; }
    public float GameplayVolume { get; set; }
    public float UiVolume { get; set; }
    public float MusicVolume { get; set; }

    public static AudioSettingsManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        LoadVolumeData();
    }

    private void SaveVolumeData()
    {
        PlayerPrefs.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.SetFloat("GameplayVolume", GameplayVolume);
        PlayerPrefs.SetFloat("UIVolume", UiVolume);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        Debug.Log("Settings Saved");
    }

    private void LoadVolumeData()
    {
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            SetMixerVolume("Master", _defaultVolume);
            SetMixerVolume("Gameplay", _defaultVolume);
            SetMixerVolume("UI", _defaultVolume);
            SetMixerVolume("Music", _defaultVolume);
            SaveVolumeData();
            return;
        }

        SetMixerVolume("Master", PlayerPrefs.GetFloat("MasterVolume"));
        SetMixerVolume("Gameplay", PlayerPrefs.GetFloat("GameplayVolume"));
        SetMixerVolume("UI", PlayerPrefs.GetFloat("UIVolume"));
        SetMixerVolume("Music", PlayerPrefs.GetFloat("MusicVolume"));
    }

    private IEnumerator WaitForSave()
    {
        yield return new WaitForSeconds(_timeToWaitForSave);
        SaveVolumeData();
    }

    private void SetMixerVolume(string mixerName, float volume)
    {
        float normalized = Mathf.Log10(volume) * 20;
        _audioMixer.SetFloat(mixerName, normalized);

        if (mixerName == "Master") MasterVolume = volume;
        if (mixerName == "Gameplay") GameplayVolume = volume;
        if (mixerName == "UI") UiVolume = volume;
        if (mixerName == "Music") MusicVolume = volume;
    }

    public float GetMixerVolume(string mixerName)
    {
        if (mixerName == "Master") return MasterVolume;
        if (mixerName == "Gameplay") return GameplayVolume;
        if (mixerName == "UI") return UiVolume;
        if (mixerName == "Music") return MusicVolume;

        return _defaultVolume;
    }

    public void SetMixerVolumeFromSlider(string mixerName, float volume)
    {
        StopCoroutine(WaitForSave());
        SetMixerVolume(mixerName, volume);
        StartCoroutine(WaitForSave());
    }
}
