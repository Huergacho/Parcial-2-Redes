using System;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager _instance;
    public static GameManager Instace => _instance;

    private int playersAlive;

    private void Start()
    {
        _instance = this;
    }

    public void PlayerDie()
    {
        playersAlive--;
    }

    private void CheckForWin()
    {
        if (playersAlive <= 1)
        {
            
        }
    }
}