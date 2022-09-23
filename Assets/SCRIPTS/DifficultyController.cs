using UnityEngine;
public class DifficultyController : MonoBehaviour
{
    public enum Difficulty {Easy, Normal, Hard }

    public GameObject[] boxes;
    public GameObject[] wallBoxes;


    private void Start()
    {
        
        SetDifficulty();
    }

    private void SetDifficulty()
    {
        switch (DatosPartida.instance.difficulty)
        {
           
            case Difficulty.Normal:
                for (short i = 0; i < boxes.Length; i++)
                    boxes[i].SetActive(true);
                    
                for (short i = 0; i < wallBoxes.Length-2 ; i++)
                    wallBoxes[i].SetActive(true);
                
                break;
            
            case Difficulty.Hard:
                for (short i = 0; i < boxes.Length; i++)
                    boxes[i].SetActive(true);
                    
                for (short i = 0; i < wallBoxes.Length ; i++)
                    wallBoxes[i].SetActive(true);

                break;
        }
    }
}
