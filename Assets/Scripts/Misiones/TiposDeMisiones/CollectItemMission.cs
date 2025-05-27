using UnityEngine;
using Photon.Pun;

public class CollectItemMission : MonoBehaviourPun
{
    public MissionData missionData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            PlayerMissions playerMissions = other.GetComponent<PlayerMissions>();
            if (playerMissions != null && !missionData.isCompleted)
            {
                playerMissions.CompleteMission(missionData);
                photonView.RPC("DestroyItem", RpcTarget.AllBuffered); 
            }
        }
    }

    [PunRPC]
    void DestroyItem()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject); // Destruye el objeto en todos los clientes.
        }
    }
}