using System;
using UnityEngine;
public class Frenado : MonoBehaviour 
{
	[SerializeField]private float VelEntrada = 0;
	[SerializeField]private bool Frenando = false;
	
	private string TagDeposito = "Deposito";
	private int Contador = 0;
	private int CantMensajes = 10;
	private float TiempFrenado = 0.5f;
	private float Tempo = 0f;
	private Vector3 Destino;

	private Rigidbody rb;
	private ControlDireccion _controlDireccion;
	private CarController _carController;
	
	
	
	private void Start ()
	{
		rb = GetComponent<Rigidbody>();
		_controlDireccion = GetComponent<ControlDireccion>();
		_carController = GetComponent<CarController>();
		Frenar();
	}

	private void OnEnable()
	{
		GameManager.FrenarCoche += Frenar;
		GameManager.RestaurarVelCoche += RestaurarVel;
	}

	private void OnDisable()
	{
		GameManager.FrenarCoche -= Frenar;
		GameManager.RestaurarVelCoche -= RestaurarVel;
	}

	private void FixedUpdate ()
	{
		if(Frenando)
		{
			Tempo += T.GetFDT();
			if(Tempo >= (TiempFrenado / CantMensajes) * Contador)
			{
				Contador++;
			}
		}
	}
	
	private void Frenar()
	{
		Frenando = true;
		rb.velocity = Vector3.zero;
		rb.constraints = RigidbodyConstraints.FreezePositionZ;
		_controlDireccion.enabled = false;
		_carController.SetAcel(0f);
		
		Tempo = 0;
		Contador = 0;
	}
	
	public void RestaurarVel()
	{
		rb.constraints = RigidbodyConstraints.None;
		_controlDireccion.enabled = true;
		_carController.SetAcel(1f);
		Frenando = false;
		Tempo = 0;
		Contador = 0;
	}
	
	private void OnTriggerEnter(Collider other) 
	{
		if(other.tag == TagDeposito)
		{
			Deposito2 dep = other.GetComponent<Deposito2>();
			if(dep.Vacio)
			{	
				if(this.GetComponent<Player>().ConBolasas())
				{
					dep.Entrar(this.GetComponent<Player>());
					Destino = other.transform.position;
					transform.forward = Destino - transform.position;
					Frenar();
				}				
			}
		}
	}

}
