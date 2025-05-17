using Photon.Pun;
using UnityEngine;

public class RolePower : MonoBehaviourPun
{
    public GameObject uiAsassine;
    public GameObject uiPerson;
    private PlayerRole plaRole;

    private string prefabToSpawn;
    private bool uiInitialized = false;

    void Start()
    {
        plaRole = GetComponent<PlayerRole>();

        if (photonView.IsMine)
        {
            uiAsassine = GameObject.Find("CanvasAssasin");
            uiPerson = GameObject.Find("CanvasPerson");
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        if (plaRole.role == 0) // Persona
        {
            uiPerson.SetActive(false);
            uiAsassine.SetActive(false);
        }
        // Inicializa el UI solo una vez cuando el rol ya esté asignado
        if (!uiInitialized && plaRole.role != 0)
        {
            if (plaRole.role == 1) // Persona
            {
                uiPerson.SetActive(true);
                uiAsassine.SetActive(false);
            }
            else if (plaRole.role == 2) // Asesino
            {
                uiPerson.SetActive(false);
                uiAsassine.SetActive(true);
            }

            uiInitialized = true;
        }

        // Asignar prefab según el rol
        if (plaRole.role == 1)
        {
            prefabToSpawn = "CubeA";
        }
        else if (plaRole.role == 2)
        {
            prefabToSpawn = "SphereB";
        }
        else
        {
            prefabToSpawn = "";
        }

        // Instanciar objeto con tecla P
        if (Input.GetKeyDown(KeyCode.P) && !string.IsNullOrEmpty(prefabToSpawn))
        {
            Vector3 spawnPosition = transform.position + Vector3.forward * 2f;
            PhotonNetwork.Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
