using System;
using Photon.Pun;
using Unity.Collections;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField] private int maxLife;
    [ReadOnly,SerializeField] int currentLife;

    private void Start()
    {
        currentLife = maxLife;
    }

    public void TakeDamage(int damage)
    {
        if (currentLife - damage <= 0)
        {
            RequestManager.Instance.RPCMaster("RequestDie",PhotonNetwork.LocalPlayer);
        }
        currentLife -= damage;
        RequestManager.Instance.RPCMaster("RequestHit",PhotonNetwork.LocalPlayer,damage);
    }
}