using Photon.Pun;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPun
{
    public PlayerController playerCon;
    public GameObject cam;
    public GameObject playerModel;

    void Start()
    {
        if (photonView.IsMine)
        {
            IsLocalPlayer();
        }
        else
        {
            cam.SetActive(false); 
        }
    }

    public void IsLocalPlayer()
    {
        playerCon.enabled = true;
        cam.SetActive(true);

        SetLayerRecursively(playerModel, LayerMask.NameToLayer("IgnoreLocal"));
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
