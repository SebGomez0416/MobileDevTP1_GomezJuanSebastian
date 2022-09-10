using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instancia;

    [SerializeField]private float TiempoDeJuego = 60;

    public enum EstadoJuego { Tutorial, Jugando, Finalizado }
    [SerializeField]private EstadoJuego EstAct = EstadoJuego.Tutorial;

    [SerializeField]private Player player1;
    [SerializeField]private Player player2;

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
        IniciarTutorial();
    }
    public Player Player1 { get; private set; }
    public Player Player2 { get; private set; }

    private void Update()
    {
        
        switch (EstAct) {
            case EstadoJuego.Tutorial:

                if (Input.GetKeyDown(KeyCode.W)) 
                    player1.Seleccionado = true;

                if (Input.GetKeyDown(KeyCode.UpArrow)) 
                    player2.Seleccionado = true;
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
                    SceneManager.LoadScene("PtsFinal");
                break;
        }
        TiempoDeJuegoText.transform.parent.gameObject.SetActive(EstAct == EstadoJuego.Jugando && !ConteoRedresivo);
    }
    private void IniciarTutorial()
    {
        player1.CambiarATutorial();
        player2.CambiarATutorial();
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
        
        if (player1.Dinero > player2.Dinero) {
            
            if (player1.LadoActual == Visualizacion.Lado.Der)
                DatosPartida.LadoGanadaor = DatosPartida.Lados.Der;
            else
                DatosPartida.LadoGanadaor = DatosPartida.Lados.Izq;
            
            DatosPartida.PtsGanador = player1.Dinero;
            DatosPartida.PtsPerdedor = player2.Dinero;
        }
        else 
        {
            if (player2.LadoActual == Visualizacion.Lado.Der)
                DatosPartida.LadoGanadaor = DatosPartida.Lados.Der;
            else
                DatosPartida.LadoGanadaor = DatosPartida.Lados.Izq;

         
            DatosPartida.PtsGanador = player2.Dinero;
            DatosPartida.PtsPerdedor = player1.Dinero;
        }

        FrenarCoche?.Invoke();
        player1.ContrDesc.FinDelJuego();
        player2.ContrDesc.FinDelJuego();
    }
    void CambiarACarrera() {

        EstAct = GameManager.EstadoJuego.Jugando;
        ConteoInicio.gameObject.SetActive(true);
        
        player1.CambiarAConduccion();
        player2.CambiarAConduccion();
            
        for (int i = 0; i < ObjsCalibracion.Length; i++) 
            ObjsCalibracion[i].SetActive(false);
        
        player1.FinCalibrado = true;
        player2.FinCalibrado = true;

        for (int i = 0; i < ObjsCarrera.Length; i++) 
            ObjsCarrera[i].SetActive(true);

        player1.gameObject.transform.position = PosCamionesCarrera[0];
        player2.gameObject.transform.position = PosCamionesCarrera[1];

        TiempoDeJuegoText.transform.parent.gameObject.SetActive(false);
        ConteoInicio.gameObject.SetActive(false);
    }
    public void FinCalibracion(int playerID)
    {
        if (playerID == 0) 
            player1.FinTuto = true;

        if (playerID == 1) 
            player2.FinTuto = true;

        if (player1.FinTuto && player2.FinTuto)
            CambiarACarrera();
    }

}
