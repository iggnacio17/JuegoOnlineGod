using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxFrameRate;
    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = maxFrameRate;
        //ALA
    }
}
