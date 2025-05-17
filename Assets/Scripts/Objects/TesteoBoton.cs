using UnityEngine;

public class TesteoBoton : MonoBehaviour
{
    public Color color;

    public void Pressed(PlayerController player)
    {
        Debug.Log("Pressed ejecutado por: " + player.gameObject.name);
        if (player != null)
        {
            player.ChangePlayerColor(color);
        }
    }
}
