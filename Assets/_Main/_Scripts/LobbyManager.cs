using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI currentPlayerText;
    [SerializeField] private TextMeshProUGUI nickNamePrefab;
    [SerializeField] private Transform nickNameContent;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI startButtonText;
    [SerializeField] private int minPlayers;
    [SerializeField] private int maxPlayers;
    [SerializeField] private float tickCooldown = 1.5f;
   
    public TMP_InputField roomInputField;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TextMeshProUGUI roomName;
    public RoomItem roomItemPrefab;
    private List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;
    private bool isInRoom;
    private List<TextMeshProUGUI> _nickNames = new List<TextMeshProUGUI>();
    private float nextTick;
    [SerializeField] public TMP_Dropdown playerNumber;
    [SerializeField] private List<String> numberPlayerOptions = new List<string>(){"2","3","4"};
    private void Start()
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        maxPlayers = minPlayers;
        if (playerNumber != null)
        {
            playerNumber.AddOptions(numberPlayerOptions);
        }
        PhotonNetwork.JoinLobby();
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.interactable = false;
            startButtonText.text = "Waiting players to connect...";
        }
    }

    private void Update()
    {
        if (isInRoom)
        {
            CheckForAllPlayersToJoin();
        }
    }

    void CheckForAllPlayersToJoin()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayers +1 ||PhotonNetwork.CurrentRoom.PlayerCount >= minPlayers +1 )
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                startButtonText.text = "Waiting to host...";
                return;
            }

            startButton.interactable = true;
            startButtonText.text = "Start Game";
        }
    }
    public void OnClickCreate()
    {
        if (roomName.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = (byte)maxPlayers });
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        isInRoom = true;
        roomName.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        currentPlayerText.text = MaxPlayersText();
        InstanceNickNames();

    }

    void InstanceNickNames()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            var newNick = Instantiate(nickNamePrefab, nickNameContent);
            newNick.text = player.NickName;
            _nickNames.Add(newNick);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomInfo)
    {
        if (Time.time >= nextTick)
        {
            UpdateRoomList(roomInfo);
            nextTick = tickCooldown;
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        for (int i = 0; i < roomItemsList.Count; i++)
        {
            var index = roomItemsList[i];
            Destroy(index.gameObject);
        }
        roomItemsList.Clear();
        foreach (var room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }

    public void UpdatePlayersWhenJoin(Player player)
    {
        var newNick = Instantiate(nickNamePrefab, nickNameContent);
        newNick.text = player.NickName;
        _nickNames.Add(newNick);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        currentPlayerText.text = MaxPlayersText();
        UpdatePlayersWhenJoin(newPlayer);
    }

    private String MaxPlayersText()
    {
      return  "CurrentPlayer " + (PhotonNetwork.CurrentRoom.PlayerCount -1) + "/" +
            PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    public void OnClickStart()
    {
        photonView.RPC(nameof(StartGame),RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Game");

    }

    public override void OnDisconnected(DisconnectCause cause)
    {

    }

    public override void OnLeftRoom()
    {
        isInRoom = false;
        ClearPlayers();
    }

    private void ClearPlayers()
    {
        for (int i = 0; i < _nickNames.Count; i++)
        {
            var nicks = _nickNames[i];
            Destroy(nicks);
        }
        _nickNames.Clear();
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    private void UpdatePlayersWhenLeft(Player leavePlayer)
    {
        for (int i = 0; i < _nickNames.Count; i++)
        {
            if (_nickNames[i].text == leavePlayer.NickName)
            {
                var name = _nickNames[i];
                _nickNames.Remove(name);
                Destroy(name);
                break;
            }
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayersWhenLeft(otherPlayer);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    
    public void SetMaxNumberChange(int i)
    {    
        maxPlayers = i+minPlayers;
    }
    public void OnClickToLeave()
    {
        PhotonNetwork.LeaveRoom();
    }
}