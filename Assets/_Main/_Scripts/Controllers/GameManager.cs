using System;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        throw new NotImplementedException();
    }
}