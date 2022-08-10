using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableExtension : Selectable
{
    public event Action<TransitionType> OnTransitionTriggerd;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);

        switch (state)
        {
            case SelectionState.Normal:
                OnTransitionTriggerd?.Invoke(TransitionType.Normal);
                break;
            case SelectionState.Highlighted:
                OnTransitionTriggerd?.Invoke(TransitionType.Highlighted);
                break;
            case SelectionState.Pressed:
                OnTransitionTriggerd?.Invoke(TransitionType.Pressed);
                break;
            case SelectionState.Selected:
                OnTransitionTriggerd?.Invoke(TransitionType.Selected);
                break;
            case SelectionState.Disabled:
                OnTransitionTriggerd?.Invoke(TransitionType.Disabled);
                break;
        }
        
    }
}

public enum TransitionType
{
    Normal,
    Highlighted,
    Pressed,
    Selected,
    Disabled
}
