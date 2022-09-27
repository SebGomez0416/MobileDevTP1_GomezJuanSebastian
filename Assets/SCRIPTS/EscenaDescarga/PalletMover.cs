using UnityEngine;

public class PalletMover : ManejoPallets {
   
    public ManejoPallets Desde, Hasta;
    bool segundoCompleto = false;
    private bool isPause;
    [SerializeField] private string playerID;

    private void Awake()
    {
        isPause = false;
    }

    private void OnEnable()
    {
        UI_Buttons.OnPause += Pause;
    }

    private void OnDisable()
    {
        UI_Buttons.OnPause -= Pause;
    }

    private void Pause()
    {
        isPause = !isPause;
    }

    private void Update()
    {
      if (isPause) return;
        
      if (!Tenencia() && Desde.Tenencia() && Left()) 
           PrimerPaso();
                
      if (Tenencia() && Down()) 
           SegundoPaso();
                
      if (segundoCompleto && Tenencia() && Right()) 
           TercerPaso();
      
    }

    private bool Up()
    {
        return InputManager.instance.GetAxis("Vertical" + playerID) > 0.5f;
    }
    private bool Down()
    {
        return InputManager.instance.GetAxis("Vertical" + playerID) < -0.5f;
    }
    private bool Left()
    {
        return InputManager.instance.GetAxis("Horizontal" + playerID) < -0.5f;
    }
    private bool Right()
    {
        return InputManager.instance.GetAxis("Horizontal" + playerID) > 0.5f;
    }

    void PrimerPaso() {
        Desde.Dar(this);
        segundoCompleto = false;
    }
    void SegundoPaso() {
        base.Pallets[0].transform.position = transform.position;
        segundoCompleto = true;
    }
    void TercerPaso() {
        Dar(Hasta);
        segundoCompleto = false;
    }

    public override void Dar(ManejoPallets receptor) {
        if (Tenencia()) {
            if (receptor.Recibir(Pallets[0])) {
                Pallets.RemoveAt(0);
            }
        }
    }
    public override bool Recibir(Pallet pallet) {
        if (!Tenencia()) {
            pallet.Portador = this.gameObject;
            base.Recibir(pallet);
            return true;
        }
        else
            return false;
    }
}
