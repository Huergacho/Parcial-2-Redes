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
            chatPanel.SetActive(false);
        }   

       
    }

    public void SetActiveChat()
    {
        chatPanel.SetActive(true);
    }

}