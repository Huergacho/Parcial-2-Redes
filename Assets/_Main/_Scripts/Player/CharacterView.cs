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
    private Coroutine _getHitCoroutine;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void AttackAnimation(bool state)
    {
        _animator.SetBool("IsAttacking",state);
    }

    public void MoveAnimation(bool state)
    {
        _animator.SetBool("IsMoving",state);
    }

    public void GetHitAnimation(bool state)
    {
        print("Empiezo la de hit");
        _animator.SetBool("IsGettingHit",state);
        if (_getHitCoroutine == null)
        {
            _getHitCoroutine = StartCoroutine(ResetAnimation());
        }
    }

    public IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0).Length);
        GetHitAnimation(false);
        _getHitCoroutine = null;
    }

}