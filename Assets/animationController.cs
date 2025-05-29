using UnityEngine;

public class animationController : MonoBehaviour
{
    public Animator animator;
    public float[] stopAt = { .3f, .5f, .8f }; // Define your pause times in seconds

    private bool isPaused = false;
    int stopIndex = 0;
    public void resume()
    {
        if (isPaused)
        {
            animator.speed = 1f;
            isPaused = false;
            stopIndex++;
            Debug.Log("Resumed Animator");
        }
    }
    void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (!isPaused && state.normalizedTime >= stopAt[stopIndex])
        {
            animator.speed = 0f;
            isPaused = true;
            Debug.Log("Paused Animator");
        }
    }
}
