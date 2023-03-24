using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class AnimatorController : MonoBehaviour
{
   private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayIdle()
    {
        _animator.SetBool("Idle",true);
    }

    public void PlayRun()
    {
        _animator.SetBool("Run",true);
        _animator.SetBool("Attack", false);
    }

    public void StopRun()
    {
        _animator.SetBool("Run", false);
    }

    public void PlayAttack()
    {
        _animator.SetBool("Attack", true);
    }

    public void StopAttack()
    {
        _animator.SetBool("Attack", false);
    }
}
