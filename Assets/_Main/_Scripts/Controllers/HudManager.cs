using System;
using TMPro;
using UnityEngine;
using Photon.Pun;
public class HudManager : MonoBehaviourPun
{
    [SerializeField] private GameObject winnerTextContainer;
    [SerializeField] private TextMeshProUGUI winnerText;
  //  [SerializeField] private GameObject chat;

    private void Awake()
    {
        winnerTextContainer.SetActive(false);
    //    chat.SetActive(false);
    }

    [PunRPC]
    public void WinScreen(string text, bool status)
    {
        winnerTextContainer.SetActive(status);
        winnerText.text = text;
    }
    [PunRPC]
    public void ShowChat(bool status)
    {
       // chat.SetActive(status);
    }
    
}