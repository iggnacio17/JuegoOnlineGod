using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class PlayerMissions : MonoBehaviourPunCallbacks
{
    public List<MissionData> allMissions;
    public List<MissionData> currentMissions = new List<MissionData>();
    public int completedMissions = 0;

    private void Start()
    {
        if (photonView.IsMine)
        {
            AssignRandomMissions();
        }
    }

    void AssignRandomMissions()
    {
        currentMissions.Clear();

        for (int i = 0; i < 3; i++)
        {
            if (allMissions.Count > 0)
            {
                int randomIndex = Random.Range(0, allMissions.Count);
                currentMissions.Add(allMissions[randomIndex]);
                allMissions.RemoveAt(randomIndex);
            }
        }

        UpdateMissionsUI();
    }

    public void CompleteMission(MissionData mission)
    {
        if (photonView.IsMine && currentMissions.Contains(mission))
        {
            mission.isCompleted = true;
            completedMissions++;

            photonView.RPC("UpdateMissionProgress", RpcTarget.All, currentMissions.IndexOf(mission));

            if (completedMissions >= 3)
            {
                Debug.Log("¡Todas las misiones completadas!");
            }
        }
    }

    [PunRPC]
    void UpdateMissionProgress(int missionIndex)
    {
        if (missionIndex < currentMissions.Count)
        {
            currentMissions[missionIndex].isCompleted = true;
        }
        UpdateMissionsUI();
    }

    void UpdateMissionsUI()
    {
        foreach (var mission in currentMissions)
        {
            Debug.Log($"{mission.missionName}: {(mission.isCompleted ? "✔️" : "❌")}");
        }
    }
}