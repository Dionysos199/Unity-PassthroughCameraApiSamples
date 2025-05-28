using UnityEngine;

public class AnimatiorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string triggerName;

    public void TriggerAnimation()
    {
        Debug.Log("trigger animation");
        if (animator != null && !string.IsNullOrEmpty(triggerName))
        {
            animator.SetTrigger(triggerName);
        }
        else
        {
            Debug.LogWarning("Animator or triggerName not assigned.");
        }
    }
}
