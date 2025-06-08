using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Mission")]
public class Mission : ScriptableObject
{
    public int missionId;
    public string missionTitle;
    public bool isCompleted;
}
