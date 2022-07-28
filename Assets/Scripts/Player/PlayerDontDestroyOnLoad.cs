
using UnityEngine;

public class PlayerDontDestroyOnLoad : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
