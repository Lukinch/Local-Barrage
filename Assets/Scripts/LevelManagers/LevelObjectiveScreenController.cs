using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelObjectiveScreenController : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _countdown;
    [SerializeField] private int _timeToStartMatch;

    private int _currentTime;
    private Vector3 _imageSize;
    private GlobalPlayersManager _globalPlayersManager;

    private float _amountToSubstract;

    public event Action OnObjectiveShown;

    private void Awake()
    {
        _globalPlayersManager = FindObjectOfType<GlobalPlayersManager>();

        /// 1 - Full image fill amount
        /// 50 - WaitForFixedUpdate calls per second
        _amountToSubstract = (1f / (float)_timeToStartMatch) / 50f;
    }

    private void OnEnable()
    {
        _currentTime = _timeToStartMatch;
        _countdown.text = $"{_currentTime}";
        _imageSize = _background.rectTransform.localScale;
    }

    private void Start()
    {
        StartCoroutine(nameof(WaitForMatchToStart));
        StartCoroutine(nameof(ShrinkBackground));
    }

    private IEnumerator WaitForMatchToStart()
    {
        while (_currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            _currentTime--;
            _countdown.text = $"{_currentTime}";
        }

        DisableGameObject();
        OnObjectiveShown?.Invoke();
    }

    private IEnumerator ShrinkBackground()
    {
        while (_currentTime > 0)
        {
            _imageSize.y -= _amountToSubstract;
            _background.rectTransform.localScale = _imageSize;
            yield return new WaitForFixedUpdate();
        }
    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
