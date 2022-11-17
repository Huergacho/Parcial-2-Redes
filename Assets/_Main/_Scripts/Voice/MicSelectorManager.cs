using System.Collections.Generic;
using Photon.Voice;
using TMPro;
using UnityEngine;
using Recorder = Photon.Voice.Unity.Recorder;

public class MicSelectorManager : MonoBehaviour
{
    public TMP_Dropdown dropdow;
    public Recorder rec;

    private void Awake()
    {
        var micList = new List<string>(Microphone.devices);
        dropdow.AddOptions(micList);
    }

    public void SetMic(int i)
    {
        var mic = Microphone.devices[i];
        rec.MicrophoneDevice = new DeviceInfo(mic);
    }
}
