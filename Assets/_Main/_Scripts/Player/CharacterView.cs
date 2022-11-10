using System;
using Photon.Pun;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        
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
        _animator.SetBool("IsGettingHit",state);
    }


}