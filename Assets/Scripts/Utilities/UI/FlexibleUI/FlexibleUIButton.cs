
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(ButtonEventEmitter))]
public class FlexibleUIButton : FlexibleUI
{
    private Image _image;
    private ButtonEventEmitter _button;
    private TextMeshProUGUI _text;
    private SelectableExtension _buttonSelectable;

    public override void Awake()
    {
        base.Awake();
        _buttonSelectable.OnTransitionTriggerd += OnSelectionChanged;
    }

    private void Start()
    {
        _button.OnButtonSelected += SetTextColorToSelected;
    }

    private void OnSelectionChanged(TransitionType transition)
    {
        switch (transition)
        {
            case TransitionType.Normal:
                ChangeTintsToNormal();
                break;
            case TransitionType.Highlighted:
                ChangeTintsToHighlighted();
                break;
            case TransitionType.Pressed:
                ChangeTintsToPressed();
                break;
            case TransitionType.Selected:
                ChangeTintsToSelected();
                break;
            case TransitionType.Disabled:
                ChangeTintsToDisabled();
                break;
        }
    }

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        _image = GetComponent<Image>();
        _button = GetComponent<ButtonEventEmitter>();
        _buttonSelectable = GetComponent<SelectableExtension>();
        _text = GetComponentInChildren<TextMeshProUGUI>();

        _button.transition = Selectable.Transition.ColorTint;
        _button.targetGraphic = _image;

        _button.colors = skinData.backgroundColors;
    }

    private void ChangeTintsToNormal()
    {
        _text.color = skinData.fontColors.normalColor;
    }
    private void ChangeTintsToHighlighted()
    {
        _text.color = skinData.fontColors.highlightedColor;
    }
    private void ChangeTintsToPressed()
    {
        _text.color = skinData.fontColors.pressedColor;
    }
    private void ChangeTintsToSelected()
    {
        _text.color = skinData.fontColors.selectedColor;
    }
    private void ChangeTintsToDisabled()
    {
        _text.color = skinData.fontColors.disabledColor;
    }

    private void SetTextColorToSelected()
    {
        ChangeTintsToSelected();
    }
}
