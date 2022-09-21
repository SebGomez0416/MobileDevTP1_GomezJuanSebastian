using UnityEngine;
using TMPro;

public class UI_EscenaFinal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ganador;
    [SerializeField] private TextMeshProUGUI player1;
    [SerializeField] private TextMeshProUGUI player2;

    private void Start()
    {
        Ganador();
    }

    private void Ganador()
    {
        player1.text = "" + DatosPartida.instance.ScoreP1;
        player2.text = "" + DatosPartida.instance.ScoreP2;
        
        int p1= DatosPartida.instance.ScoreP1;
        int p2= DatosPartida.instance.ScoreP2;

        if (p1 > p2)
            ganador.text = "Ganador\n" + "P1";
        else if (p2 > p1)
            ganador.text = "Ganador\n" + "P2";
        else if (p2 == p1)
            ganador.text = "Empate" ;
    }

   
    

}
