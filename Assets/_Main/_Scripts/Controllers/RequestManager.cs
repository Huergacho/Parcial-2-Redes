using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RequestManager : MonoBehaviourPunCallbacks
{
    private static RequestManager _instance;
    Dictionary<Player, CharacterModel> _dicChars = new Dictionary<Player, CharacterModel>();
    Dictionary<CharacterModel, Player> _dicPlayer = new Dictionary<CharacterModel, Player>();
    [SerializeField] private GameObject[] characterPrefab;
    [SerializeField] private Transform[] spawnPoints;
    public static RequestManager Instance => _instance;

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

    // Update is called once per frame
    [PunRPC]
    public void RequestConnectPlayer(Player client)
    {
        if (client == PhotonNetwork.MasterClient)
        {
            return;
        }
        print(client.ActorNumber);
        GameObject obj = PhotonNetwork.Instantiate(characterPrefab[client.ActorNumber -2].name, spawnPoints[client.ActorNumber -2].position, Quaternion.identity);
        var character = obj.GetComponent<CharacterModel>();
        // var characterView = obj.GetComponent<CharacterView>();
        // characterView.ChangeAnimator(PhotonNetwork.LocalPlayer.ActorNumber);
        _dicChars[client] = character;
        _dicPlayer[character] = client;
    }
    public void RPCMaster(string name, params object[] p)
    {
        photonView.RPC(name, PhotonNetwork.MasterClient, p);
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

    [PunRPC]
    public void RequestDie(Player client)
    {
        FilterPlayer(client).Die();
    }

    public void RequestHit(Player client, int damage,Vector3 dir)
    {
        FilterPlayer(client).OnHitAction(damage,dir);
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
}
