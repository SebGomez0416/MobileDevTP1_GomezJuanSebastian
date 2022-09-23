using UnityEngine;

public class Menu : MonoBehaviour
{

    [SerializeField] private GameObject screenCredits;
    [SerializeField] private GameObject screenDifficulty;

    public void ExitScreen(bool state)
    {
        screenCredits.SetActive(state);
    }
    
    public void Exit()
    {
        Application.Quit();
    }

    public void SinglePlayer()
    {
        DatosPartida.instance.Players = 1;
        screenDifficulty.SetActive(true);
    }
    
    public void MultiPlayer()
    {
        DatosPartida.instance.Players = 2;
        screenDifficulty.SetActive(true);
    }

    
    
}
