using Photon.Pun;
using UnityEngine;

public class Meteors : MonoBehaviourPun
{
    private float _lifeSpan = 3f;

    private float _currtime;

    void Start()
    {
        _currtime = _lifeSpan + Time.time;
    }

    void Update()
    {
        if (_currtime<=Time.time)
        {
            PhotonNetwork.Destroy(photonView.gameObject);
        }
    }
}
