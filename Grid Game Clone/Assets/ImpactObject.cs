using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactObject : MonoBehaviour
{	
	public float moveSpeed;
	public int OwnerPlayerNumber;
	public GameObject WallExplodeParticle, BulletExplodeParticle, AudioPlayer;
	public AudioClip HitSound, DestroySound, PlayerHitSound;

	private float moveTimer;
	
	// Update is called once per frame
	void Update ()
	{
		if (moveTimer >= moveSpeed)
		{
			transform.position += transform.up;
			moveTimer = 0;
		}
		else
		{
			moveTimer += Time.deltaTime;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Bullet") && other.GetComponent<ImpactObject>().OwnerPlayerNumber != OwnerPlayerNumber)
		{
			Instantiate(BulletExplodeParticle, transform.position, transform.rotation);
			GameObject sound = Instantiate(AudioPlayer);
			sound.GetComponent<AudioSource>().PlayOneShot(HitSound);
			Destroy(gameObject);
		}
		else if (other.CompareTag("Player"))
		{
			if (other.GetComponent<PlayerMovement>().PlayerNumber != OwnerPlayerNumber)
			{
				Instantiate(BulletExplodeParticle, transform.position, transform.rotation);
				GameObject sound = Instantiate(AudioPlayer);
				sound.GetComponent<AudioSource>().PlayOneShot(PlayerHitSound);
				Destroy(gameObject);
			}
		}
		else if (other.CompareTag("Wall"))
		{
			Instantiate(WallExplodeParticle, other.transform.position, transform.rotation);
			//Instantiate(BulletExplodeParticle, transform.position, transform.rotation);
			GameObject sound = Instantiate(AudioPlayer);
			sound.GetComponent<AudioSource>().PlayOneShot(DestroySound);
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
		else
		{
			Instantiate(BulletExplodeParticle, transform.position, transform.rotation);
			GameObject sound = Instantiate(AudioPlayer);
			sound.GetComponent<AudioSource>().PlayOneShot(HitSound);
			Destroy(gameObject);
		}
	}
}
