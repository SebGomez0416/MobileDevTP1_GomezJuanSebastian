using UnityEngine;

public class ControlDireccion : MonoBehaviour 
{
	public enum TipoInput {AWSD, Arrows}
	[SerializeField]private TipoInput InputAct = TipoInput.AWSD;

	private float Giro = 0;
	[SerializeField]private bool Habilitado = true;
	private CarController carController;

	void Start () 
	{
		carController = GetComponent<CarController>();
	}

	private void OnEnable()
	{
		GameManager.HabilitarCoche += SetHabilitado;
	}

	private void OnDisable()
	{
		GameManager.HabilitarCoche -= SetHabilitado;
	}

	private void SetHabilitado(bool habilitar)
	{
		Habilitado = habilitar;
	}

	void Update () 
	{
		switch(InputAct)
		{
            case TipoInput.AWSD:
                if (Habilitado) {
                    if (Input.GetKey(KeyCode.A)) {
						Giro = -1;
                    }
                    else if (Input.GetKey(KeyCode.D)) {
						Giro = 1;
                    }
                    else {
						Giro = 0;
					}
                }
                break;
            case TipoInput.Arrows:
                if (Habilitado) {
                    if (Input.GetKey(KeyCode.LeftArrow)) {
						Giro = -1;
					}
                    else if (Input.GetKey(KeyCode.RightArrow)) {
						Giro = 1;
					}
                    else {
						Giro = 0;
					}
                }
                break;
        }

		carController.SetGiro(Giro);
	}

	public float GetGiro()
	{
		return Giro;
	}
	
}
