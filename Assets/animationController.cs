using UnityEngine;

public class animationController : MonoBehaviour
{
    public Animator animator;
    public float[] stopAt = { 0.1f, 0.5f, 0.9f };
    [SerializeField] GameObject[] Poses;

    private bool isPaused = false;
    private int stopIndex = 0;
    private string currentStateName = "";
    [SerializeField]GameObject particleEffect;

    public void resume()
    {
        if (isPaused)
        {
            animator.speed = 1f;
            isPaused = false;

            if (Poses.Length > stopIndex && Poses[stopIndex])
                Destroy(Poses[stopIndex], 2.5f);

            stopIndex++;

            if (Poses.Length > stopIndex && Poses[stopIndex])
                Poses[stopIndex].SetActive(true);

            Debug.Log("Resumed Animator");
        }
    }

    void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        // Detect new state entry and reset stop index
        if (currentStateName != state.fullPathHash.ToString())
        {
            currentStateName = state.fullPathHash.ToString();
            stopIndex = 0;
            isPaused = false;
            animator.speed = 1f;
        }

        if (!isPaused && stopIndex < stopAt.Length && state.normalizedTime >= stopAt[stopIndex])
        {
            animator.speed = 0f;
            isPaused = true;
            Debug.Log($"Paused Animator at {stopAt[stopIndex]} in state {currentStateName}");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            resume();
        }
    }
    public void setparticleEffecttrue()
    {
        particleEffect.SetActive(true);
    }
    public void setparticleEffectFalse()
    {
        particleEffect.SetActive(false);
    }
}
