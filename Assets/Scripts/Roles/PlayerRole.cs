using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerRole : MonoBehaviourPunCallbacks
{
    public int role = 0; //0: sin rol, 1: persona, 2: asesino

    private int maxPlayers => PhotonNetwork.CurrentRoom.MaxPlayers;

    private void Start()
    {
        if (!photonView.IsMine) return;
        Invoke(nameof(RequestMyRoleFromMaster), 1f);
    }

    void RequestMyRoleFromMaster()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RequestMyRole", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        else
        {
            TryAssignRoles();
        }
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayers)
        {
            TryAssignRoles();
        }
    }

    [PunRPC]
    public void RPC_SetRole(int roleValue)
    {
        role = roleValue;

        switch (role)
        {
            case 1:
                Debug.Log($"{photonView.Owner.NickName} es una PERSONA");
                GetComponent<PlayerController>().ChangePlayerColor(Color.blue);
                break;
            case 2:
                Debug.Log($"{photonView.Owner.NickName} es el ASESINO");
                GetComponent<PlayerController>().ChangePlayerColor(Color.red);
                break;
            default:
                Debug.Log($"{photonView.Owner.NickName} no tiene rol asignado");
                break;
        }
    }

    [PunRPC]
    public void RequestMyRole(int actorNumber)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(actorNumber.ToString(), out object roleObj))
        {
            foreach (var roleObjMono in FindObjectsOfType<PlayerRole>())
            {
                if (roleObjMono.photonView.Owner.ActorNumber == actorNumber)
                {
                    roleObjMono.photonView.RPC("RPC_SetRole", roleObjMono.photonView.Owner, (int)roleObj);
                    break;
                }
            }
        }
    }

    void TryAssignRoles()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("RolesAssigned")) return;
        if (PhotonNetwork.CurrentRoom.PlayerCount < maxPlayers) return;

        AssignRoles();
    }

    void AssignRoles()
    {
        Player[] players = PhotonNetwork.PlayerList;
        ExitGames.Client.Photon.Hashtable roleData = new ExitGames.Client.Photon.Hashtable();

        int asesinoIndex = Random.Range(0, players.Length);

        for (int i = 0; i < players.Length; i++)
        {
            int assignedRole = (i == asesinoIndex) ? 2 : 1;
            roleData[players[i].ActorNumber.ToString()] = assignedRole;
        }

        roleData["RolesAssigned"] = true;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roleData);

        foreach (var pr in FindObjectsOfType<PlayerRole>())
        {
            int actor = pr.photonView.Owner.ActorNumber;
            int roleInt = (int)roleData[actor.ToString()];
            pr.photonView.RPC("RPC_SetRole", pr.photonView.Owner, roleInt);
        }
    }
}
