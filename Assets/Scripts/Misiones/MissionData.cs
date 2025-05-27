using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "Missions/New Mission")]
public class MissionData : ScriptableObject
{
    public string missionName;
    public string description;
    public bool isCompleted;
}