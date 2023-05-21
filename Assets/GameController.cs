using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class GameController : MonoBehaviourPunCallbacks
{
    public GameObject[] spawns;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomPos = spawns[Random.Range(0, spawns.Length)].transform.localPosition;

        PhotonNetwork.Instantiate(player.name, randomPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
