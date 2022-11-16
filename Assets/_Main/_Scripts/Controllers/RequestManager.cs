using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Serialization;

public class RequestManager : MonoBehaviourPunCallbacks
{
    private static RequestManager _instance;
    Dictionary<Player, CharacterModel> _dicChars = new Dictionary<Player, CharacterModel>();
    Dictionary<CharacterModel, Player> _dicPlayer = new Dictionary<CharacterModel, Player>();
    [SerializeField] private GameObject[] characterPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private HudManager winHud;
    private GameObject currentWinHud;
    public static RequestManager Instance => _instance;
    private int playersAlive;
    private bool canRestart;

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }
    #region Requests
    [PunRPC]
    public void RequestConnectPlayer(Player client)
    {
        if (client == PhotonNetwork.MasterClient)
        {
            return;
        }
        GameObject obj = PhotonNetwork.Instantiate(characterPrefab[client.ActorNumber -2].name, spawnPoints[client.ActorNumber -2].position, Quaternion.identity);
        var character = obj.GetComponent<CharacterModel>();
        character.AssignStats(spawnPoints[client.ActorNumber -2]);
        playersAlive++;
        _dicChars[client] = character;
        _dicPlayer[character] = client;
    }
    [PunRPC]
    public void RequestMove(Player client, Vector3 dir)
    {
        FilterPlayer(client).Move(dir);
    }

    [PunRPC]
    public void RequestJump(Player client)
    {
        FilterPlayer(client).Jump();
    }

    [PunRPC]
    public void RequestAttack(Player client)
    {
        FilterPlayer(client).Attack();
    }
    #endregion
    
    #region Utilities
    public void RPCMaster(string name, params object[] p)
    {
        photonView.RPC(name, PhotonNetwork.MasterClient, p);
    }

    private CharacterModel FilterPlayer(Player client)
    {
        if (_dicChars.ContainsKey(client))
        {
            var character = _dicChars[client];
            return character;
        }
        else
        {
            return null;
        }
    }    
    private Player FilterClient(CharacterModel client)
    {
        if (_dicPlayer.ContainsKey(client))
        {
            var character = _dicPlayer[client];
            return character;
        }
        else
        {
            return null;
        }
    }

    [PunRPC]
    public void RequestChat(CharacterModel client, bool status)
    {
        winHud.photonView.RPC("ShowChat",FilterClient(client),status);
    }
    

    #endregion
    
    #region Photon Methods
   
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_dicChars.ContainsKey(otherPlayer))
            {
                var character = _dicChars[otherPlayer];
                RemovePlayer(otherPlayer);
                PhotonNetwork.Destroy(character.gameObject);
            }
        }
    }

    #endregion
    
    #region Remove Methods

    public void RemoveModel(CharacterModel model)
    {
        if (_dicPlayer.ContainsKey(model))
        {
            var player = _dicPlayer[model];
            _dicChars.Remove(player);
            _dicPlayer.Remove(model);
        }
    }
    public void RemovePlayer(Player player)
    {
        if (_dicChars.ContainsKey(player))
        {
            var character = _dicChars[player];
            _dicChars.Remove(player);
            _dicPlayer.Remove(character);
        }
    }


    #endregion
    
    #region GetMethods

    public Player GetClientFromModel(CharacterModel model)
    {
        if (_dicPlayer.ContainsKey(model))
        {
            return _dicPlayer[model];
        }
        return null;
    }
    public CharacterModel[] GetAllModels()
    {
        var characters = new CharacterModel[_dicChars.Count];
        int count = 0;
        foreach (var item in _dicChars)
        {
            characters[count] = item.Value;
            count++;
        }
        return characters;
    }

    #endregion
    public void PlayerDie()
    {
        playersAlive--;
        //winHud.photonView.RPC(nameof(winHud.ShowChat),FilterClient(model),true);
        if (playersAlive == 1)
        {
            CheckForWinner();
            StartCoroutine(RestartGameRoutine());
        }
        
    }
    private void CheckForWinner()
    {
        foreach (var model in GetAllModels())
        {
            if (!model.IsDead && PhotonNetwork.IsMasterClient)
            {
                winHud.photonView.RPC(nameof(winHud.WinScreen),RpcTarget.All,GetClientFromModel(model).NickName,true);
                break;
            }
        }
    }
    // public void RequestWinner()
    // {
    //     foreach (var model in GetAllModels())
    //     {
    //         if (!model.IsDead)
    //         { 
    //             winHud.WinScreen(GetClientFromModel(model).NickName);
    //             break;
    //         } 
    //     }
    // }

    IEnumerator RestartGameRoutine()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
            RestartGame();
        }
    }
    private void RestartGame()
    {
        StopCoroutine(RestartGameRoutine());
        winHud.ShowChat(false);
        winHud.photonView.RPC(nameof(winHud.WinScreen),RpcTarget.All,"",false);
        playersAlive = 0;
        foreach (var model in GetAllModels())
        {
            playersAlive++;
            model.Respawn();
        }
    }

}
