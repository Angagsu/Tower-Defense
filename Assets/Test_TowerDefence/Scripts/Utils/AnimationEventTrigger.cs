using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
    public void AnimationTriggered()
    {
        gameObject.SetActive(false);
    }
}
