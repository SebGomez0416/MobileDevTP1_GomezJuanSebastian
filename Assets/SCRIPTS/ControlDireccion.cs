using UnityEngine;

public class ControlDireccion : MonoBehaviour 
{
	private float Giro = 0;
	[SerializeField]private bool Habilitado = true;
	private CarController carController;
	private  int playerID;
	private string inputName = "Horizontal";

	void Start () 
	{
		carController = GetComponent<CarController>();
		playerID = GetComponent<Player>().IdPlayer;
		inputName += playerID;
		Debug.Log(inputName);
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
		Giro = InputManager.instance.GetAxis(inputName);
		carController.SetGiro(Giro);
	}

	public float GetGiro()
	{
		return Giro;
	}
	
}
