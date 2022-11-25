using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ChatScript : MonoBehaviourPun
{
    public TextMeshProUGUI content;
    public TMP_InputField inputField;
    private string _command = "/m";
    
    // Start is called before the first frame update
    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Destroy(gameObject);
        }
    }

    public void ChatSendMessage()
    {
        var message = inputField.text;
        if (string.IsNullOrEmpty(message)||string.IsNullOrWhiteSpace(message)) return;
       
        string[] words = message.Split(' ');

        if (words[0] == _command && Char.IsDigit(words[1],0)) //checks if it is a number
        {
            var thisMuchMeteorits = Int32.Parse(words[1].Substring(0));
            
          RequestManager.Instance.RPCMaster("RequestMeteorito",thisMuchMeteorits);   
        }
        else
        {
            photonView.RPC(nameof(GetChatMessage), RpcTarget.All,PhotonNetwork.NickName,message);
        }
    }

    [PunRPC]
    public void GetChatMessage(string nameClient, string message)
    {
        string color;
        
        if (nameClient == PhotonNetwork.NickName)
        {
            color = "<color=green>";
        }
        else
        {
            color = "<color=yellow>";
        }
        content.text += color + nameClient + ": " + "</color>" + message +"\n";
    }
}
