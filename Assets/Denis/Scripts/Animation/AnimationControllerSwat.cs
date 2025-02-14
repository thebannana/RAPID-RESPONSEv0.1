using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwatOfficerAnimator : MonoBehaviour
{
    private Animator animator;
    private Unit unit;

    void Start()
    {
        animator = GetComponent<Animator>();
        unit = GetComponentInParent<Unit>();

        if (unit == null)
        {
            Debug.LogError("Unit script not found in parent!");
        }
    }

    void Update()
    {
        if (unit != null)
        {
            UpdateAnimations();
        }
    }

    void UpdateAnimations()
    {
        switch (unit.currentState)
        {
            case Unit.OfficerState.Incapacitated:
                animator.Play("IncapacitatedOfficer");
                break;
            case Unit.OfficerState.Idle:
                animator.Play("IdleOfficer");
                break;
            case Unit.OfficerState.Walking:
                animator.Play("RunningOfficer");
                break;
            case Unit.OfficerState.WalkingToDoor:
                animator.Play("RunningOfficer");
                break;
            case Unit.OfficerState.OpeningClosingDoor:
                animator.Play("InteractingOfficer");
                break;
            case Unit.OfficerState.WalkingToDetain:
                animator.Play("RunningOfficer");
                break;
            case Unit.OfficerState.Detaining:
                animator.Play("InteractingOfficer");
                break;
            case Unit.OfficerState.WalkingToCollectEvidence:
                animator.Play("RunningOfficer");
                break;
            case Unit.OfficerState.CollectingEvidence:
                animator.Play("InteractingOfficer");
                break;
            case Unit.OfficerState.WalkingToArrest:
                animator.Play("RunningOfficer");
                break;
            case Unit.OfficerState.Arresting:
                animator.Play("InteractingOfficer");
                break;
            case Unit.OfficerState.AutonomousAttack:
                animator.Play("ShootingOfficer");
                break;
            case Unit.OfficerState.WalkingToAttack:
                animator.Play("RunningOfficer");
                break;
            case Unit.OfficerState.OrderedAttack:
                animator.Play("ShootingOfficer");
                break;
            case Unit.OfficerState.Throwing:
                animator.Play("IdleOfficer");
                break;
            default:
                animator.Play("IdleOfficer");
                break;
        }
    }
}
