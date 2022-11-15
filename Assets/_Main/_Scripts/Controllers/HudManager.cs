using TMPro;
using UnityEngine;
using Photon.Pun;
public class HudManager : MonoBehaviourPun
{
    [SerializeField] private GameObject winnerTextContainer;
    [SerializeField] private TextMeshProUGUI winnerText;
    [PunRPC]
    public void WinScreen(string text)
    {
        winnerTextContainer.SetActive(true);
        winnerText.text = text;
    }
}