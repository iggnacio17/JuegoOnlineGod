using UnityEngine;

public class AccesorieController : MonoBehaviour
{
    public GameObject accesoriePrefab; 
    public string huesoNombre; 
    public Vector3 offsetPosition = Vector3.zero; 
    public Vector3 offsetRotation = Vector3.zero; 

    public void Pressed(PlayerController player)
    {
        if (player == null)
        {
            Debug.LogWarning("No se recibió un PlayerController al presionar.");
            return;
        }

        Transform huesoTransform = FindChildByName(player.transform, huesoNombre);

        if (huesoTransform == null)
        {
            Debug.LogWarning($"No se encontró el hueso '{huesoNombre}' en {player.name}");
            return;
        }

        GameObject newAccesorie = Instantiate(accesoriePrefab, huesoTransform);
        newAccesorie.transform.localPosition = offsetPosition; 
        newAccesorie.transform.localRotation = Quaternion.Euler(offsetRotation); 

        Debug.Log($"Accesorio {accesoriePrefab.name} instanciado en {huesoTransform.name} del jugador {player.name}");
    }

    private Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.name == name)
                return child;
        }
        return null;
    }
}
