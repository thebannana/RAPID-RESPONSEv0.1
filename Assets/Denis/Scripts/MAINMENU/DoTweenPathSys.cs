using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoTweenPathSys : MonoBehaviour
{
    [SerializeField] private float duration;

    public void MoveTo(Transform target)
    {
        transform.DOMove(target.position, duration);
    }
    public void LookAt(Transform target)
    {
        transform.DODynamicLookAt(target.position, duration);
    }

}
