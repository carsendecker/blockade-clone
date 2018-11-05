using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
	
	public int PlayerNumber;
	[HideInInspector] public bool hitSomething;
	[HideInInspector] public Quaternion startRotation;
	public float MoveSpeed;
	public int MaxBullets;
	[HideInInspector] public int BulletCount;
	public GameObject TrailObject;
	public GameObject Confetti;
	public GameObject MainCam;
	public GameObject Bullet;
	public GameObject[] BulletUI;
	
	private Vector3 direction;
	private Vector3 lastPosition;
	private Quaternion newRotation;
	private AudioSource aso;
	private float moveTimer;
	private bool stopped;
	private Color playerColor;

	// Use this for initialization
	void Start ()
	{
		aso = GetComponent<AudioSource>();
		startRotation = transform.rotation;
		BulletCount = MaxBullets;
		playerColor = GetComponent<SpriteRenderer>().color;

		GameObject.Find("P" + PlayerNumber + " Img").GetComponent<Image>().color = playerColor;
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
		foreach (var element in BulletUI)
		{
			element.SetActive(true);
		}

		moveTimer = 0;
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
			transform.rotation = newRotation;
			transform.position += direction;
			GameObject newWall = Instantiate(TrailObject, lastPosition, transform.rotation);
			newWall.GetComponent<SpriteRenderer>().color = playerColor;
			moveTimer = 0;
		}
	}

	void ChangeDirectionP1()
	{
		if (Input.GetKeyDown(KeyCode.W) && direction != Vector3.down)
		{
			direction = Vector2.up;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
		}
		else if (Input.GetKeyDown(KeyCode.A) && direction != Vector3.right)
		{
			direction = Vector2.left;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
		}
		else if (Input.GetKeyDown(KeyCode.S) && direction != Vector3.up)
		{
			direction = Vector2.down;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
		}
		else if (Input.GetKeyDown(KeyCode.D) && direction != Vector3.left)
		{
			direction = Vector2.right;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -90);		
		}

		if (Input.GetKeyDown(KeyCode.E) && BulletCount > 0 && !stopped)
		{
			BulletCount--;
			GameObject newBullet = Instantiate(Bullet, transform.position + transform.up, transform.rotation);
			newBullet.GetComponent<ImpactObject>().OwnerPlayerNumber = PlayerNumber;
			aso.Play();
			BulletUI[BulletCount].SetActive(false);
		}
	}
	
	void ChangeDirectionP2()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector3.down)
		{
			direction = Vector2.up;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector3.right)
		{
			direction = Vector2.left;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector3.up)
		{
			direction = Vector2.down;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector3.left)
		{
			direction = Vector2.right;
			newRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -90);
		}
		
		if (Input.GetKeyDown(KeyCode.RightShift) && BulletCount > 0 && !stopped)
		{
			BulletCount--;
			GameObject newBullet = Instantiate(Bullet, transform.position + transform.up, transform.rotation);
			newBullet.GetComponent<ImpactObject>().OwnerPlayerNumber = PlayerNumber;
			aso.Play();
			BulletUI[BulletCount].SetActive(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Bullet"))
		{
			if (other.GetComponent<ImpactObject>().OwnerPlayerNumber != PlayerNumber && !stopped)
			{
				StartCoroutine(StopPlayer(1.4f));
			}
		}
		else
		{
			hitSomething = true;
			MainCam.GetComponent<CameraShake>().ShakeCamera(0.1f);
			Instantiate(Confetti, transform.position, transform.rotation);
		}
	
	}

	IEnumerator StopPlayer(float stopTime)
	{
		stopped = true;
		Color originalColor = GetComponent<SpriteRenderer>().color;
		GetComponent<SpriteRenderer>().color = Color.yellow;
		float originalSpeed = MoveSpeed;
		MoveSpeed = 1000;
		
		yield return new WaitForSeconds(stopTime);

		stopped = false;
		MoveSpeed = originalSpeed;
		GetComponent<SpriteRenderer>().color = originalColor;
	}
}
