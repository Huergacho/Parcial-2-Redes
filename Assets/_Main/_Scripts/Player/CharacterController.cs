using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;

public class CharacterController : MonoBehaviourPun
{
    [SerializeField] private KeyCode jumpInput;
    
    

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
        }
       // SetChatStatus(false);
    }

    private void Start()
    {
        RequestManager.Instance.RPCMaster("RequestConnectPlayer", PhotonNetwork.LocalPlayer);
        PhotonNetwork.Instantiate("VoiceObject", Vector3.zero, Quaternion.identity);
        PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled = false;
        // _lifeCheckCD = Time.time + updateLifecd;
    }
    private void Update()
    {
        Move();
        Jump();
        Attack();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled = true;
        } else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled = false;
        }
        
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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            RequestManager.Instance.RPCMaster("RequestAttack",PhotonNetwork.LocalPlayer);
        }
    }
    
}