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

    public void ChangeScreen()
    {
        screenDifficulty.SetActive(true);
    }

    
    
}
