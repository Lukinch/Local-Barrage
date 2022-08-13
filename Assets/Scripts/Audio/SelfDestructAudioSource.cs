
using UnityEngine;

public class SelfDestructAudioSource : MonoBehaviour
{
    public void DestroySelf(float timeDelay)
    {
        Destroy(gameObject, timeDelay);
    }
}
