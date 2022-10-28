using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RoomItem : MonoBehaviour
{
  public TextMeshProUGUI roomName;

  private void Start()
  {
    
  }

  public void SetRoomName(string name)
  {
    roomName.text = name;
  }

  public void OnClickItem()
  {
    LobbyManager.Instance.JoinRoom(roomName.text);
  }
}