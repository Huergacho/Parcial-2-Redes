using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeteorSpawner : MonoBehaviourPun
{
    private List<GameObject> _meteorsPool;

    [SerializeField] private GameObject meteorPref;
    [SerializeField] private float _spawntime;

    [SerializeField] private int poolSize = 20;

    [SerializeField] private Transform[] spawnPoints;
    private float currTime;
    private int lastPoint = 0;

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
            return;
        }

        _meteorsPool = new List<GameObject>();
        //_meteorsPool = new GameObject[poolSize];
    }

   private void Start()
    {
        ResetTimer();
        currTime += 5f;
        for (int i = 0; i < poolSize; i++)
        {
          RefillSpool();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currTime<=Time.time)
        {
            ReleaseMeteor();
            
            ResetTimer();
            if (_meteorsPool.Count<=4)
            {
                RefillSpool();
            }
            
        }
    }

    void ReleaseMeteor()
    {
        var spawn = GetSpawnPoint();

        var meteor = _meteorsPool[_meteorsPool.Count - 1];
        
        meteor.transform.position = spawnPoints[spawn].position;
        meteor.SetActive(true);
        meteor.SetActive(true);
        _meteorsPool.RemoveAt(_meteorsPool.Count - 1);
        RefillSpool();

    }
    
    void ResetTimer()
    {
        currTime = _spawntime + Time.time;
    }

    int GetSpawnPoint()
    {
        var spawn = Random.Range(0, spawnPoints.Length - 1);
        if (spawn == lastPoint)
        {
            GetSpawnPoint();
        }

        return spawn;

    }

    void RefillSpool()
    {
        var meteor  = PhotonNetwork.Instantiate(meteorPref.name, Vector3.zero, Quaternion.identity);
        meteor.SetActive(false);
        _meteorsPool.Add(meteor);
    }


}
