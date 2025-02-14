using UnityEngine;

public class CivilianModelAnimator : MonoBehaviour
{
    private Animator animator;
    private Civilian civilian;

    void Start()
    {
        animator = GetComponent<Animator>();
        civilian = GetComponentInParent<Civilian>(); // Get the Civilian script from the parent GameObject
    }

    void Update()
    {
        if (civilian != null)
        {
            UpdateAnimation();
        }
    }

    private void UpdateAnimation()
    {
        switch (civilian.currentState)
        {
            case Civilian.CivilianState.Idle:
                PlayIdleAnimation();
                break;
            case Civilian.CivilianState.Wandering:
                PlayWanderingAnimation();
                break;
            case Civilian.CivilianState.Detained:
                PlayDetainedAnimation();
                break;
        }
    }

    private void PlayIdleAnimation()
    {
        animator.Play("ScaredCivilian");
    }

    private void PlayWanderingAnimation()
    {
        animator.Play("WalkingCivilian");
    }

    private void PlayDetainedAnimation()
    {
        animator.Play("ArrestedCivilian");
    }
}
