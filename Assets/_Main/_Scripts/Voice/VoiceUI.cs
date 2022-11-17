using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceUI : MonoBehaviourPun
{
    private MicUI _micUI;
    public Speaker speaker;
    private void Start()
    {
        if (photonView.IsMine)
        {
            _micUI = FindObjectOfType<MicUI>();
        }
        else
        {
            FindObjectOfType<VoiceChatUI>().AddSpeaker(speaker,photonView.Owner);
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            var status = PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled;
            _micUI.Show(status);
        }
    }
}
