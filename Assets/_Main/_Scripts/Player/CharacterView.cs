using System;
using System.Collections;
using Photon.Pun;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    //[SerializeField]private AnimatorController[] _animatorList;
    private Animator _animator;
    private Coroutine _getHitCoroutine = null;
    private Coroutine _attackCoroutine = null;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void AttackAnimation(bool state, float cooldown)
    {
        
        _animator.SetBool("IsAttacking",state);
        if (_attackCoroutine == null || state)
        {
            _getHitCoroutine = StartCoroutine(AttackCooldown(cooldown));
        }
    }

    public void MoveAnimation(bool state)
    {
        _animator.SetBool("IsMoving",state);
    }

    public void GetHitAnimation(bool state)
    {
        _animator.SetBool("IsGettingHit",state);
        if (_getHitCoroutine == null || state)
        {
            _getHitCoroutine = StartCoroutine(ResetAnimation());
        }
    }
    public IEnumerator AttackCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        AttackAnimation(false,cooldown);
        _attackCoroutine = null;
    }
    public IEnumerator ResetAnimation()
    {
        print("Arranco Corutina de Hit");
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        GetHitAnimation(false);
        _getHitCoroutine = null;
    }

    public void DieAnimation(bool state)
    {
        _animator.SetBool("IsDead",state);
    }
    
}