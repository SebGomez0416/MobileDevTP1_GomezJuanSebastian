using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public abstract class GMState
    {
        public abstract void Update(GameManager gm);
    }

    public class GMStateTutorial:GMState
    {
        public override void Update(GameManager gm)
        {
            if (InputManager.instance.GetAxis("Vertical0") >= 0.5f) 
                gm.players[0].Seleccionado = true;

            if (InputManager.instance.GetAxis("Vertical1") >= 0.5f)
                gm.players[1].Seleccionado = true;
        }
    }
    
    public class GMStateJugando:GMState
    {
        public override void Update(GameManager gm)
        {
            if ( gm.TiempoDeJuego <= 0) 
                gm.FinalizarCarrera();

            if ( gm.ConteoRedresivo) {
                gm.ConteoParaInicion -= T.GetDT();
                if ( gm.ConteoParaInicion < 0) {
                    gm.EmpezarCarrera();
                    gm.ConteoRedresivo = false;
                }
            }
            else
            {
                if ( gm.isPause) return;
                gm.TiempoDeJuego -= T.GetDT();
            }
                
            if ( gm.ConteoRedresivo) 
            {
                if ( gm.ConteoParaInicion > 1) 
                    gm.ConteoInicio.text =  gm.ConteoParaInicion.ToString("0");
                    
                else  gm.ConteoInicio.text = "GO";
            }

            gm.ConteoInicio.gameObject.SetActive( gm.ConteoRedresivo);
            gm.TiempoDeJuegoText.text =  gm.TiempoDeJuego.ToString("00");
        }
    }
    
    public class GMStateFinalizado:GMState
    {
        public override void Update(GameManager gm)
        {
            gm.TiempEspMuestraPts -= Time.deltaTime;
            if (gm.TiempEspMuestraPts <= 0)
            {
                if (gm.players.Length == 1)
                {
                    DatosPartida.instance.ScoreP1 = gm.players[0].Dinero;
                    SceneManager.LoadScene("GameOverSinglePlayer");
                }
                else
                {
                    DatosPartida.instance.ScoreP1 = gm.players[0].Dinero;
                    DatosPartida.instance.ScoreP2 = gm.players[1].Dinero;
                    SceneManager.LoadScene("GameOverMultiplayer");
                }
            }
        }
    }
    

    public static GameManager Instancia;

    [SerializeField]private float TiempoDeJuego = 60;
    
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

    public bool isPause;
    public Player[] Players => this.players;

    private GMStateTutorial stateTutorial = new GMStateTutorial();
    private GMStateJugando stateJugando = new GMStateJugando();
    private GMStateFinalizado stateFinalizado = new GMStateFinalizado();
    private GMState currenState = null;
    
    void Awake() {
        GameManager.Instancia = this;
        isPause = false;
    }

    private void Start()
    { 
        DatosPartida.instance.Init();
        DatosPartida.instance.cantidadPlayers = players.Length;
        IniciarTutorial();
    }

    private void OnEnable()
    {
        UI_Buttons.OnPause += Pause;
    }

    private void OnDisable()
    {
        UI_Buttons.OnPause -= Pause;
    }
    private void Update()
    {
        if(currenState != null)
             currenState.Update(this);
        
         TiempoDeJuegoText.transform.parent.gameObject.SetActive(currenState == stateJugando && !ConteoRedresivo);
    }

    private void Pause()
    {
        isPause = !isPause;

        if (isPause)
        {
            FrenarCoche?.Invoke();
            HabilitarCoche?.Invoke(false);
        }
        else
        {
            HabilitarCoche?.Invoke(true);
            RestaurarVelCoche?.Invoke();
        }
      
        
    }
    private void IniciarTutorial()
    {
        foreach (var p in players)
            p.CambiarATutorial();
        
        currenState = stateTutorial;
    }
    private void EmpezarCarrera()
    {
        RestaurarVelCoche?.Invoke();
        HabilitarCoche?.Invoke(true);
    }

    void FinalizarCarrera() 
    {
        currenState = stateFinalizado;
        TiempoDeJuego = 0;

        FrenarCoche?.Invoke();
        foreach (var p in players)
           p.ContrDesc.FinDelJuego();
    }
    void CambiarACarrera() {

        currenState = stateJugando;
        
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
