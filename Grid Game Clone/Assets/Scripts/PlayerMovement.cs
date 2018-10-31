using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;

	private Vector2 direction;
	private Vector3 lastPosition;
	private Quaternion newRotation;
	private float moveTimer;
	
	[HideInInspector] public bool hitSomething;
	[HideInInspector] public Quaternion startRotation;
	public float MoveSpeed;
	public int PlayerNumber;
	public GameObject TrailObject;
	public GameObject Confetti;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		startRotation = transform.rotation;

	}

	private void OnEnable()
	{
		if (PlayerNumber == 1)
		{
			direction = Vector2.down;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
		}
		else if (PlayerNumber == 2)
		{
			direction = Vector2.up;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		moveTimer += Time.deltaTime;
		
		if (PlayerNumber == 1)
		{
			ChangeDirectionP1();
		}
		else if (PlayerNumber == 2)
		{
			ChangeDirectionP2();
		}

		if (moveTimer >= MoveSpeed)
		{
			lastPosition = transform.position;
			rb.position += direction;
			transform.rotation = newRotation;
			Instantiate(TrailObject, lastPosition, transform.rotation);
			moveTimer = 0;
		}
	}

	void ChangeDirectionP1()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			direction = Vector2.up;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			direction = Vector2.left;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			direction = Vector2.down;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			direction = Vector2.right;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -90);		
		}
	}
	
	void ChangeDirectionP2()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			direction = Vector2.up;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);

		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			direction = Vector2.left;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			direction = Vector2.down;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			direction = Vector2.right;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -90);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		hitSomething = true;
		Instantiate(Confetti, transform.position, transform.rotation);
	}
}
