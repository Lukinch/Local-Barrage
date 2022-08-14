using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicManager : MonoBehaviour
{
    void Start()
    {
        BackgroundMusicManager.Instance.PlayRandomLevelTheme();
    }
}
