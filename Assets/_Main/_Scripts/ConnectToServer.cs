using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField userNameInput;
    public TextMeshProUGUI status;
    public Button button;

    public void OnClickConnect()
    {
        if (userNameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = userNameInput.text;
            status.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        // button.interactable = false;
        // PhotonNetwork.JoinLobby();
        // status.text = "Connecting To Lobby";
        SceneManager.LoadScene("Lobby");
    }

    // public override void OnDisconnected(DisconnectCause cause)
    // {
    //     status.text = "Connection Failed" + "  "+  cause.ToString();
    // }
    //
    // public override void OnJoinedLobby()
    // {
    //     button.interactable = true;
    //     status.text = "Connected to Lobby";
    // }
    //
    // public override void OnLeftLobby()
    // {
    //     base.OnLeftLobby();
    //     status.text = "Lobby Failed";
    // }
}
