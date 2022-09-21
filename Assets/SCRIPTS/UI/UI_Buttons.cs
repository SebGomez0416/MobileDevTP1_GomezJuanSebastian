using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Buttons : MonoBehaviour
{
    public void Reiniciar()
    {
        SceneManager.LoadScene(DatosPartida.instance.cantidadPlayers == 1 ? "SinglePlayer" : "Multiplayer");
    }
    
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
    
    public void Pausa()
    {
       //codear pausa
    }

    public void Mute()
    {
        //codear
    }

    public void Salir()
    {
        Application.Quit();
    }
    
}
