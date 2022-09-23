using System;
using UnityEngine;
using System.Collections;

public class CarCamera : MonoBehaviour
{
	public Transform target = null;
	public float height = 1f;
	public float positionDamping = 3f;
	public float velocityDamping = 3f;
	public float distance = 4f;
	public LayerMask ignoreLayers = -1;
	
	public float LejaniaZ = 1;

	private RaycastHit hit = new RaycastHit();
	private Vector3 prevVelocity = Vector3.zero;
	private LayerMask raycastLayers = -1;
	private Vector3 currentVelocity = Vector3.zero;
	private bool isPause;
	private Rigidbody rb;
	
	void Start()
	{
		rb = target.GetComponentInParent<Rigidbody>();
		raycastLayers = ~ignoreLayers;
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


	void FixedUpdate()
	{
		if (isPause) return;
		currentVelocity = Vector3.Lerp(prevVelocity, rb.velocity, velocityDamping * Time.deltaTime);
		currentVelocity.y = 0;
		prevVelocity = currentVelocity;
	}
	
	void LateUpdate()
	{
		if (isPause) return;
		
		float speedFactor = Mathf.Clamp01(rb.velocity.magnitude / 70.0f);
		GetComponent<Camera>().fieldOfView = Mathf.Lerp(55, 72, speedFactor);
		float currentDistance = Mathf.Lerp(7.5f, 6.5f, speedFactor);
		
		currentVelocity = currentVelocity.normalized;
		
		Vector3 newTargetPosition = target.position + Vector3.up * height;
		Vector3 newPosition = newTargetPosition - (currentVelocity * currentDistance);
		newPosition.y = newTargetPosition.y;
		
		Vector3 targetDirection = newPosition - newTargetPosition;
		if(Physics.Raycast(newTargetPosition, targetDirection, out hit, currentDistance, raycastLayers))
			newPosition = hit.point;
		
		newPosition += transform.forward * LejaniaZ;//diferencia en z agregada por mi
		
		transform.position = newPosition;
		transform.LookAt(newTargetPosition);
		
		//rotacion agregada por mi
		Vector3 vAux = transform.rotation.eulerAngles;
		vAux.x = 20;
		transform.eulerAngles = vAux;
	}
}
