using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI currentPlayerText;
    [SerializeField] private TextMeshProUGUI nickNamePrefab;
    [SerializeField] private Transform nickNameContent;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI startButtonText;
    [SerializeField] private int maxPlayers;
    [SerializeField] private float tickCooldown = 1.5f;
    [SerializeField] public TMP_Dropdown playerNumber;
    [SerializeField] private List<String> numberPlayerOptions = new List<string>(){"2","3","4"};
     public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TextMeshProUGUI roomName;
    private bool _isInRoom;
    private List<TextMeshProUGUI> _nickNames = new List<TextMeshProUGUI>();
    private float _nextTick;


    private void Awake()
    {
        roomName.text = "Master Room";
    }

    private void Start()
    {
        PhotonNetwork.JoinLobby();
        if (PhotonNetwork.CountOfPlayers == 1)
        {
            lobbyPanel.SetActive(true);
            roomPanel.SetActive(false);
         
            playerNumber.AddOptions(numberPlayerOptions);
            PhotonNetwork.LocalPlayer.NickName = "Host";
            startButton.interactable = false;
            startButtonText.text = "Waiting players to connect...";
            return;
        }

        print("No soy el host");
        PhotonNetwork.JoinRoom(roomName.text);
    }

    private void Update()
    {
        if (_isInRoom)
        {
            CheckForAllPlayersToJoin();
        }
    }

    void CheckForAllPlayersToJoin()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= maxPlayers)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                startButtonText.text = "Waiting host to start...";
                return;
            }

            startButton.interactable = true;
            startButtonText.text = "Start Game";
        }
    }
    public void OnClickCreate()
    {
         PhotonNetwork.CreateRoom(roomName.text, new RoomOptions() { MaxPlayers = (byte)maxPlayers });
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        _isInRoom = true;
        roomName.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        currentPlayerText.text = "CurrentPlayer :" + (PhotonNetwork.CurrentRoom.PlayerCount-1) + "/" +
                                 PhotonNetwork.CurrentRoom.MaxPlayers;
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
    public void UpdatePlayersWhenJoin(Player player)
    {
        var newNick = Instantiate(nickNamePrefab, nickNameContent);
        newNick.text = player.NickName;
        _nickNames.Add(newNick);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        currentPlayerText.text = "CurrentPlayer :" + (PhotonNetwork.CurrentRoom.PlayerCount-1) + "/" +
                                PhotonNetwork.CurrentRoom.MaxPlayers;
        UpdatePlayersWhenJoin(newPlayer);
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
        _isInRoom = false;
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
        maxPlayers = i+2;
    }
    
    public void OnClickToLeave()
    {
        PhotonNetwork.LeaveRoom();
    }
}