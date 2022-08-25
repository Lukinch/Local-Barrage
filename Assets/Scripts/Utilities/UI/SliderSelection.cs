
using UnityEngine;
using UnityEngine.UI;

public class SliderSelection : MonoBehaviour
{
    [Header("Selection Settings")]
    [SerializeField] private Image _sliderHandle;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _normalColor;

    public void SetSelectedColor()
    {
        _sliderHandle.color = _selectedColor;
    }
    public void SetDeselectedColor()
    {
        _sliderHandle.color = _normalColor;
    }
}
