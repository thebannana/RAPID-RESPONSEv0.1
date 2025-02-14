using UnityEngine;

public class SuspectModelAnimator : MonoBehaviour
{
    private Animator animator;
    private Suspect suspect;

    void Start()
    {
        animator = GetComponent<Animator>();
        suspect = GetComponentInParent<Suspect>(); // Get the Suspect script from the parent GameObject
    }

    void Update()
    {
        if (suspect != null)
        {
            UpdateAnimation();
        }
    }

    private void UpdateAnimation()
    {
        switch (suspect.currentState)
        {
            case Suspect.SuspectState.Idle:
                PlayIdleAnimation();
                break;
            case Suspect.SuspectState.Patroling:
                PlayPatrolAnimation();
                break;
            case Suspect.SuspectState.Concussed:
                PlayConcussedAnimation();
                break;
            case Suspect.SuspectState.Flee:
                PlayFleeAnimation();
                break;
            case Suspect.SuspectState.Surrendered:
                PlaySurrenderedAnimation();
                break;
            case Suspect.SuspectState.Attack:
                PlayAttackAnimation();
                break;
            case Suspect.SuspectState.Killed:
                PlayKilledAnimation();
                break;
            case Suspect.SuspectState.Arrested:
                PlayArrestedAnimation();
                break;
        }
    }

    private void PlayIdleAnimation()
    {
        animator.Play("IdleTerrorist");
    }

    private void PlayPatrolAnimation()
    {
        animator.Play("WalkingTerrorist");
    }

    private void PlayConcussedAnimation()
    {
        animator.Play("Concussed");
    }

    private void PlayFleeAnimation()
    {
        animator.Play("WalkingTerrorist");
    }

    private void PlaySurrenderedAnimation()
    {
        animator.Play("SurrenderingTerrorist");
    }

    private void PlayAttackAnimation()
    {
        animator.Play("ShootingTerrorist");
    }

    private void PlayKilledAnimation()
    {
        animator.Play("DeathTerrorist");
    }

    private void PlayArrestedAnimation()
    {
        animator.Play("ArrestedTerrorist");
    }
}
