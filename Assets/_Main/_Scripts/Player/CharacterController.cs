using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CharacterController : MonoBehaviourPun
{
    private void Start()
    {
        RequestManager.Instance.RPCMaster("RequestConnectPlayer", PhotonNetwork.LocalPlayer);
    }
    private void Update()
    {
        Move();
    }

    private void Move()
    {
        var h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = new Vector3(h, 0, 0).normalized;
        if (dir != Vector3.zero)
        {
            RequestManager.Instance.RPCMaster("RequestMove", PhotonNetwork.LocalPlayer, dir);
        }
    }

    private void Jump()
    {
        var v = Input.GetAxisRaw("Vertical");
        if (v != 0)
        {
            RequestManager.Instance.RPCMaster("RequestJump",PhotonNetwork.LocalPlayer);
        }
    }
}