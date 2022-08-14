using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _menuClip;
    [SerializeField] private AudioClip[] _levelClips;
    [SerializeField] private AudioClip _nextLevelClip;
    [SerializeField] private AudioClip _playerWonClip;

    private AudioClip _currentClip;

    public static BackgroundMusicManager Instance;

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

        PlayMenuTheme();
    }

    public void PlayMenuTheme()
    {
        _audioSource.clip = _menuClip;
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void PlayRandomLevelTheme()
    {
        int index = Random.Range(0, _levelClips.Length);
        while (_currentClip == _levelClips[index])
        {
            index = Random.Range(0, _levelClips.Length);
        }
        _audioSource.clip = _levelClips[index];
        _audioSource.loop = true;
        _audioSource.Play();
        _currentClip = _audioSource.clip;
    }

    public void PauseMusic()
    {
        _audioSource.Pause();
    }
    public void ResumeMusic()
    {
        _audioSource.Play();
    }
    public void StopMusic()
    {
        _audioSource.Stop();
    }

    public void PlayNextLevelClip()
    {
        StopMusic();
        _audioSource.clip = _nextLevelClip;
        _audioSource.Play();
    }

    public void PlayPlayerWonClip()
    {
        StopMusic();
        _audioSource.clip = _playerWonClip;
        _audioSource.Play();
    }
}
