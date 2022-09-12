using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instancia;

    [SerializeField]private float TiempoDeJuego = 60;

    public enum EstadoJuego { Tutorial, Jugando, Finalizado }
    [SerializeField]private EstadoJuego EstAct = EstadoJuego.Tutorial;
    [SerializeField]private Player[] players;
    
    private bool ConteoRedresivo = true;
    [SerializeField]private Rect ConteoPosEsc;
    [SerializeField]private float ConteoParaInicion = 3;
    [SerializeField]private Text ConteoInicio;
    [SerializeField]private Text TiempoDeJuegoText;

    [SerializeField]private float TiempEspMuestraPts = 3;

    [SerializeField]private Vector3[] PosCamionesCarrera = new Vector3[2];
    [SerializeField]private GameObject[] ObjsCalibracion;
    [SerializeField]private GameObject[] ObjsCarrera;
    private Vector3 PosCamion = Vector3.zero;

    public static event Action FrenarCoche;
    public static event Action RestaurarVelCoche;
    public static event Action <bool> HabilitarCoche;

    
    void Awake() {
        GameManager.Instancia = this;
    }

    private void Start()
    { 
        DatosPartida.instance.Init();
        DatosPartida.instance.cantidadPlayers = players.Length;
        IniciarTutorial();
    }
    public Player[] Players => this.players;

    private void Update()
    {
        
        switch (EstAct) {
            case EstadoJuego.Tutorial:

                if (Input.GetKeyDown(KeyCode.W)) 
                    players[0].Seleccionado = true;

                if (Input.GetKeyDown(KeyCode.UpArrow)) 
                    players[1].Seleccionado = true;
                break;

            case EstadoJuego.Jugando:

                if (TiempoDeJuego <= 0) 
                    FinalizarCarrera();

                if (ConteoRedresivo) {
                    ConteoParaInicion -= T.GetDT();
                    if (ConteoParaInicion < 0) {
                        EmpezarCarrera();
                        ConteoRedresivo = false;
                    }
                }
                else TiempoDeJuego -= T.GetDT();
                
                if (ConteoRedresivo) 
                {
                    if (ConteoParaInicion > 1) 
                        ConteoInicio.text = ConteoParaInicion.ToString("0");
                    
                    else ConteoInicio.text = "GO";
                }

                ConteoInicio.gameObject.SetActive(ConteoRedresivo);
                TiempoDeJuegoText.text = TiempoDeJuego.ToString("00");
                break;

            case EstadoJuego.Finalizado:

                TiempEspMuestraPts -= Time.deltaTime;
                if (TiempEspMuestraPts <= 0)
                {
                    if (players.Length == 1)
                    {
                        DatosPartida.instance.ScoreP1 = players[0].Dinero;
                        SceneManager.LoadScene("GameOverSinglePlayer");
                    }
                    else
                    {
                        DatosPartida.instance.ScoreP1 = players[0].Dinero;
                        DatosPartida.instance.ScoreP2 = players[1].Dinero;
                        SceneManager.LoadScene("GameOverMultiplayer");
                    }
                }
                break;
        }
        TiempoDeJuegoText.transform.parent.gameObject.SetActive(EstAct == EstadoJuego.Jugando && !ConteoRedresivo);
    }
    private void IniciarTutorial()
    {
        foreach (var p in players)
            p.CambiarATutorial();
    }
    private void EmpezarCarrera()
    {
        RestaurarVelCoche?.Invoke();
        HabilitarCoche?.Invoke(true);
    }

    void FinalizarCarrera() 
    {
        EstAct = GameManager.EstadoJuego.Finalizado;
        TiempoDeJuego = 0;

        FrenarCoche?.Invoke();
        foreach (var p in players)
           p.ContrDesc.FinDelJuego();
    }
    void CambiarACarrera() {

        EstAct = GameManager.EstadoJuego.Jugando;
        ConteoInicio.gameObject.SetActive(true);
        
        foreach (var p in players)
            p.CambiarAConduccion();

        for (int i = 0; i < ObjsCalibracion.Length; i++) 
            ObjsCalibracion[i].SetActive(false);

        foreach (var p in players)
            p.FinCalibrado = true;

        for (int i = 0; i < ObjsCarrera.Length; i++) 
            ObjsCarrera[i].SetActive(true);

        for (short i = 0; i < players.Length; i++)
            players[i].gameObject.transform.position = PosCamionesCarrera[i];

        TiempoDeJuegoText.transform.parent.gameObject.SetActive(false);
        ConteoInicio.gameObject.SetActive(false);
    }
    public void FinCalibracion(int playerID)
    {
        switch (playerID)
        {
            case 0:
                players[0].FinTuto = true;
                break;
            case 1:
                players[1].FinTuto = true;
                break;
        }

        if (players.Length == 1)
        {
            if ( players[0].FinTuto )
                CambiarACarrera();
        }
        else
        {
            if ( players[0].FinTuto &&  players[1].FinTuto)
                CambiarACarrera();
        }
    }

}
