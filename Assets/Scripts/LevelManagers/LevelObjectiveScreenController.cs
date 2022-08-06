using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelObjectiveScreenController : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI countdown;
    [SerializeField] private int timeToStartMatch;

    private int currentTime;
    private Vector3 imageSize;
    private GlobalPlayersManager globalPlayersManager;

    private float amountToSubstract;

    public event Action OnObjectiveShown;

    private void Awake()
    {
        globalPlayersManager = FindObjectOfType<GlobalPlayersManager>();

        /// 1 - Full image fill amount
        /// 50 - WaitForFixedUpdate calls per second
        amountToSubstract = (1f / (float)timeToStartMatch) / 50f;
    }

    private void OnEnable()
    {
        currentTime = timeToStartMatch;
        countdown.text = $"{currentTime}";
        imageSize = background.rectTransform.localScale;
    }

    private void Start()
    {
        StartCoroutine(nameof(WaitForMatchToStart));
        StartCoroutine(nameof(ShrinkBackground));
    }

    private IEnumerator WaitForMatchToStart()
    {
        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1);
            currentTime--;
            countdown.text = $"{currentTime}";
        }

        DisableGameObject();
        OnObjectiveShown?.Invoke();
    }

    private IEnumerator ShrinkBackground()
    {
        while (currentTime > 0)
        {
            imageSize.y -= amountToSubstract;
            background.rectTransform.localScale = imageSize;
            yield return new WaitForFixedUpdate();
        }
    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
