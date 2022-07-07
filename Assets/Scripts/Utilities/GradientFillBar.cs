using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientFillBar : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fillIamge;

    private void Update()
    {
        fillIamge.color = gradient.Evaluate(fillIamge.fillAmount);
    }
}
