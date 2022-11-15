using Photon.Pun;
using UnityEngine;

public class UIManager : MonoBehaviourPun
{
    [SerializeField] private GameObject chatPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            print("Blah");
            chatPanel.SetActive(false);
        }   

       
    }

    public void SetActiveChat()
    {
        if(!photonView.IsMine) return;
        chatPanel.SetActive(true);
    }

}