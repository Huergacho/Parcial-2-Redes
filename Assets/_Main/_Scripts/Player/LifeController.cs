﻿using System;
using Photon.Pun;
using Unity.Collections;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField] private int maxLife;
    [ReadOnly,SerializeField] int currentLife;
    public event Action OnDie;
    public event Action OnHit;

    private void Start()
    {
        currentLife = maxLife;
    }

    public void TakeDamage(int damage)
    {
        if (currentLife - damage <= 0)
        {
            OnDie?.Invoke();
        }
        currentLife -= damage;
        OnHit?.Invoke();
    }
}