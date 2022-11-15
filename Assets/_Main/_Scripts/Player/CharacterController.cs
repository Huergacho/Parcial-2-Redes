using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CharacterController : MonoBehaviourPun
{
    [SerializeField] private KeyCode jumpInput;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        RequestManager.Instance.RPCMaster("RequestConnectPlayer", PhotonNetwork.LocalPlayer);
    }
    private void Update()
    {
        Move();
        Jump();
        Attack();
    }

    private void Move()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(h, v, 0).normalized;
        if (dir != Vector3.zero)
        {
            RequestManager.Instance.RPCMaster("RequestMove", PhotonNetwork.LocalPlayer, dir);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(jumpInput))
        {
            RequestManager.Instance.RPCMaster("RequestJump",PhotonNetwork.LocalPlayer);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RequestManager.Instance.RPCMaster("RequestAttack",PhotonNetwork.LocalPlayer);
        }
    }
    
}