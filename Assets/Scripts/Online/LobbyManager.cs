using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField createOrJoinRoom;
    [SerializeField] private int maxPlayer;
    bool isConnectedToMaster;
    private void Start()
    {
        //Acá me conecto apenas abro el juego
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        //Sirve para meterme al online
        base.OnConnectedToMaster();
        Debug.Log("Connected to server!");
        isConnectedToMaster = true;
        PhotonNetwork.JoinLobby();
    }
    public override void OnCreatedRoom()
    {
        //Cuando me meto al room
        //base.OnCreatedRoom();
        print("Room Creada");
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //print("Entré en la room, cargando DiscoBar...");
        PhotonNetwork.LoadLevel("DiscoBar");
    }
    public void EntrandoOCreandoRoom()
    {
        if (isConnectedToMaster)
        {
            //Creo o me meto a algún room existente
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = maxPlayer;
            roomOptions.IsVisible = true;
            PhotonNetwork.JoinOrCreateRoom(createOrJoinRoom.text, roomOptions, TypedLobby.Default);
            //print("Creando Room");
        }
        else
        {
            Debug.Log("Aun no está conectado, intenta de nuevo!");
        }
    }
}
