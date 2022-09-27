using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class UI_Buttons : MonoBehaviour
{
   
    [SerializeField] private GameObject backgroundPause;
    [SerializeField] private GameObject pause;

    public static event Action OnPause;
    public static event Action OnMute;
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
    }
    
    private void DesactivarBotones()
    {
        pause.SetActive(false);
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
       backgroundPause.SetActive(isPause);
       
       OnPause?.Invoke();
       
    }

    public void Mute()
    {
        OnMute?.Invoke();
    }

    public void Easy()
    {
        DatosPartida.instance.difficulty = DifficultyController.Difficulty.Easy;
        ChangeScene();
    }
    
    public void Normal()
    {
        DatosPartida.instance.difficulty = DifficultyController.Difficulty.Normal;
        ChangeScene();
    }
    
    public void Hard()
    {
        DatosPartida.instance.difficulty = DifficultyController.Difficulty.Hard;
        ChangeScene();
    }

    public void Salir()
    {
        Application.Quit();
    }

    private void ChangeScene()
    {
        switch (DatosPartida.instance.Players)
        {
            case 1:
                SceneManager.LoadScene("SinglePlayer");
                break;
            case 2:
                SceneManager.LoadScene("Multiplayer");
                break;
        }
    }
    
}
