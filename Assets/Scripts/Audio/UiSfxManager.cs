using UnityEngine;
using Random = UnityEngine.Random;

public class UiSfxManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _pauseMenuClip;
    [SerializeField] private AudioClip _buttonHighlightClip;
    [SerializeField] private AudioClip _buttonPressClip;

    public static UiSfxManager Instance;
    public bool ShouldPlayButtonsSounds { get; set; } = true;
    public bool ShouldPlaySelectedSounds { get; set; } = true;

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

    public void PlayHighlightButtonClip()
    {
        if (!ShouldPlayButtonsSounds) return;
        _audioSource.volume = 0.4f;
        _audioSource.PlayOneShot(_buttonHighlightClip);
    }

    public void PlayPressButtonClip()
    {
        if (!ShouldPlayButtonsSounds) return;
        _audioSource.volume = 0.4f;
        _audioSource.PlayOneShot(_buttonPressClip);
    }

    public void PlayPauseMenuClip()
    {
        _audioSource.clip = _pauseMenuClip;
        _audioSource.volume = 1f;
        _audioSource.Play();
    }
}