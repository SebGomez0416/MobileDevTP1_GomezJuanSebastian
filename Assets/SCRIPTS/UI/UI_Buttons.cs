using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class UI_Buttons : MonoBehaviour
{
   
    [SerializeField] private GameObject backgroundPause;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject reiniciar;
    
    public static event Action OnPause;
    private bool isPause;

    private void Awake()
    {
        isPause = false;
    }

    private void OnEnable()
    {
        Player.IniciarCarrera += ActivarBotones;
        ControladorDeDescarga.finalizarCarga += DesactivarBotones;
    }

    private void OnDisable()
    {
        Player.IniciarCarrera -= ActivarBotones;
        ControladorDeDescarga.finalizarCarga -= DesactivarBotones;
    }

    private void ActivarBotones()
    {
        pause.SetActive(true);
        reiniciar.SetActive(true);
    }
    
    private void DesactivarBotones()
    {
        pause.SetActive(false);
        reiniciar.SetActive(false);
    }
    
    

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
       isPause = !isPause;
       OnPause?.Invoke();
       backgroundPause.SetActive(isPause);
       
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
