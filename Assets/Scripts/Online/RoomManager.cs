using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Transform spawnPoint;
    public GameObject roomCam;
    public GameObject playerCam;
    public static RoomManager instance;

    private List<GameObject> allPlayers = new List<GameObject>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Instanciando jugador...");
            roomCam.SetActive(false);

            GameObject player = PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity);
            allPlayers.Add(player);
        }
        else
        {
            Debug.LogWarning("No estás en una sala, no se instanció el jugador.");
        }
    }
}
