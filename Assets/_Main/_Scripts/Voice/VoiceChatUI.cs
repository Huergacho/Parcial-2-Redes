using System.Collections.Generic;
using Photon.Realtime;
using Photon.Voice.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoiceChatUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    private Dictionary<Speaker, Player> _dictionary = new Dictionary<Speaker, Player>();
    [SerializeField] private Image _image;
    public void AddSpeaker(Speaker speaker, Player player)
    {
        _dictionary[speaker] = player;
    }
    private void Update()
    {
        string voiceChat = "";

        foreach (var item in _dictionary)
        {
            var speaker = item.Key;
            if (speaker.IsPlaying)
            {
                print(item.Value.NickName + " si");
                voiceChat+=item.Value.NickName+"\n";
            }
        }
        text.text = voiceChat;
        if (voiceChat == "")
        {
            _image.enabled = false;
        }
        else
        {
            _image.enabled = true;
        }
    }
}
