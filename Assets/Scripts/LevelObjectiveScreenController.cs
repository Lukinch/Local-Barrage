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

    // (1 / timeToStartMatch) / 50
    private readonly float SUBTRACTION = 0.00666666666666666666666666666667f;

    public event Action OnObjectiveShown;

    private void Awake()
    {
        globalPlayersManager = FindObjectOfType<GlobalPlayersManager>();
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
            imageSize.y -= SUBTRACTION;
            background.rectTransform.localScale = imageSize;
            yield return new WaitForFixedUpdate();
        }
    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
