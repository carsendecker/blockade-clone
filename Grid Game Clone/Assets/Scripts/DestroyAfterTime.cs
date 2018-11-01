using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	private float timer;
	public float waitTime;
	

	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;
		if (timer >= waitTime)
		{
			Destroy(gameObject);
		}
	}
}
