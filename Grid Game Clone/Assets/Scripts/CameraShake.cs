using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	private float shakeDuration = 0f;
	public float shakeMagnitude = 0.7f;
	private float dampingSpeed = 1.0f;
	private Vector3 initialPosition;
	
	// Use this for initialization
	void Start ()
	{
		initialPosition = transform.localPosition;
	}
	
	
	// Update is called once per frame
	void Update () {
		if (shakeDuration > 0)
		{
			transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
			shakeDuration -= Time.deltaTime * dampingSpeed;
		}
		else
		{
			shakeDuration = 0f;
			transform.localPosition = initialPosition;
		}
	}
	
	public void ShakeCamera(float shakeDuration) {
		this.shakeDuration = shakeDuration;
	}
}
