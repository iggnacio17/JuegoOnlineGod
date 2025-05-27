using Photon.Pun;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public MissionData missionToComplete;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            PlayerMissions playerMissions = other.GetComponent<PlayerMissions>();
            if (playerMissions != null && !missionToComplete.isCompleted)
            {
                playerMissions.CompleteMission(missionToComplete);
                Debug.Log($"Misi�n completada: {missionToComplete.missionName}");
            }
        }
    }
}