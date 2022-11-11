using System;
using Photon.Pun;
using Unity.Collections;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    private int maxLife; 
    int currentLife;
    public event Action OnDie;
    public event Action OnHit;

    private void Start()
    {
        currentLife = maxLife;
    }

    public void AssignMaxLife(int data)
    {
        maxLife = data;
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