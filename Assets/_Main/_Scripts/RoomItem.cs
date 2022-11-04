using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RoomItem : MonoBehaviour
{
  public TextMeshProUGUI roomName;
  private LobbyManager _manager;

  private void Start()
  {
   _manager = FindObjectOfType<LobbyManager>();
  }

  public void SetRoomName(string name)
  {
    roomName.text = name;
  }

  public void OnClickItem()
  {
    _manager.JoinRoom(roomName.text);
  }
}