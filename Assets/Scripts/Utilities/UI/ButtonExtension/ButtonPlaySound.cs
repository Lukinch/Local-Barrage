using System;
using UnityEngine;

public class ButtonPlaySound : MonoBehaviour
{
    private ButtonEventEmitter _buttonEventEmitter;

    public bool ShouldPlaySounds { get; set; }

    private void Awake()
    {
        _buttonEventEmitter = GetComponent<ButtonEventEmitter>();

        _buttonEventEmitter.OnButtonPointerEntered += PlayHighlightedSound;
        _buttonEventEmitter.OnButtonSelected += PlaySelectedSound;

        _buttonEventEmitter.
        OnButtonPressed += PlayPressedSound;

        ShouldPlaySounds = true;
    }

    private void PlayHighlightedSound()
    {
        if (!ShouldPlaySounds) return;
        UiSfxManager.Instance.PlayHighlightButtonClip();
    }

    private void PlaySelectedSound()
    {
        if (!ShouldPlaySounds) return;
        if (!UiSfxManager.Instance.ShouldPlaySelectedSounds) return;
        UiSfxManager.Instance.PlayHighlightButtonClip();
    }

    private void PlayPressedSound()
    {
        if (!ShouldPlaySounds) return;
        UiSfxManager.Instance.PlayPressButtonClip();
    }
}