using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Flexible UI Data")]
public class FlexibleUIData : ScriptableObject
{
    [Header("Background Colors")]
    public ColorBlock backgroundColors;
    [Space(10)]
    [Header("Font Colors")]
    public ColorBlock fontColors;
}
