using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientFillBar : MonoBehaviour
{
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fillIamge;

    private void Update()
    {
        _fillIamge.color = _gradient.Evaluate(_fillIamge.fillAmount);
    }
}
