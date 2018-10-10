using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;

	private Vector2 direction;
	private Vector3 lastPosition;
	private float moveTimer;
	private bool hitSomething = false;
	
	public float MoveSpeed;
	public int PlayerNumber;
	public GameObject TrailObject;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();

		if (PlayerNumber == 1)
		{
			direction = Vector2.down;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		moveTimer += Time.deltaTime;
		
		if (PlayerNumber == 1)
		{
			changeDirectionP1();
		}

		if (moveTimer >= MoveSpeed)
		{
			lastPosition = transform.position;
			rb.position += direction;
			Instantiate(TrailObject, lastPosition, transform.rotation);
			moveTimer = 0;
		}
	}

	void changeDirectionP1()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			direction = Vector2.up;
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			direction = Vector2.left;
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			direction = Vector2.down;
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			direction = Vector2.right;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		hitSomething = true;
	}
}
