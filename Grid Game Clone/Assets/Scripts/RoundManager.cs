using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{

	public int MaxScore;
	public Transform[] PlayerSpawnPoints;
	public GameObject[] PlayerObjects;
	public Text EndText;
	public Text[] ScoreText;
	
	private AudioSource aso;
	public AudioClip hitSound, dingSound;
	

	private PlayerMovement[] playerScripts = new PlayerMovement[2];
	private bool betweenRounds, keepScoreVisible;
	private int[] Scores = new int[2];
	private bool menuOpen = true;

	
	// Use this for initialization
	void Start ()
	{
		playerScripts[0] = PlayerObjects[0].GetComponent<PlayerMovement>();
		playerScripts[1] = PlayerObjects[1].GetComponent<PlayerMovement>();

		PlayerObjects[0].transform.position = PlayerSpawnPoints[0].position;
		PlayerObjects[1].transform.position = PlayerSpawnPoints[1].position;

		aso = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			ResetGame();
		}

		if (keepScoreVisible)
		{
			ScoreText[0].enabled = true;
			ScoreText[1].enabled = true;
		}
		
		if (menuOpen)
		{
			playerScripts[0].enabled = false;
			playerScripts[1].enabled = false;
			if (Input.anyKeyDown)
			{
				menuOpen = false;
				playerScripts[0].enabled = true;
				playerScripts[1].enabled = true;
				aso.PlayOneShot(dingSound);
				SceneManager.UnloadSceneAsync("Menu");
			}
		}
		
		ScoreText[0].text = Scores[0].ToString();
		ScoreText[1].text = Scores[1].ToString();
		
		if (!betweenRounds)
		{
			if (playerScripts[0].hitSomething || playerScripts[1].hitSomething)
			{
				if (playerScripts[0].hitSomething && playerScripts[1].hitSomething)
				{
					Scores[0]++;
					Scores[1]++;
					PlayerObjects[0].GetComponent<FlashSprite>().FlashObject(true);
					ScoreText[0].GetComponent<FlashText>().FlashObject(true);
					PlayerObjects[1].GetComponent<FlashSprite>().FlashObject(true);
					ScoreText[1].GetComponent<FlashText>().FlashObject(true);
				}
				else if (playerScripts[0].hitSomething)
				{
					Scores[1]++;
					PlayerObjects[0].GetComponent<FlashSprite>().FlashObject(true);
					ScoreText[1].GetComponent<FlashText>().FlashObject(true);
				}
				else if (playerScripts[1].hitSomething)
				{
					Scores[0]++;
					PlayerObjects[1].GetComponent<FlashSprite>().FlashObject(true);
					ScoreText[0].GetComponent<FlashText>().FlashObject(true);
				}

				ScoreText[0].enabled = true;
				ScoreText[1].enabled = true;
				
				aso.PlayOneShot(hitSound);

				StartCoroutine(NewRound());
			}
		}
	}

	IEnumerator NewRound()
	{
		GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
		Destroy(walls[0]);
		Destroy(walls[1]);
		
		betweenRounds = true;
		playerScripts[0].enabled = false;
		playerScripts[1].enabled = false;
		
		
		
		yield return new WaitForSeconds(2.5f);

		walls = GameObject.FindGameObjectsWithTag("Wall");
		foreach (var wall in walls)
		{
			Destroy(wall);
		}
		

		PlayerObjects[0].transform.position = PlayerSpawnPoints[0].position;
		PlayerObjects[0].transform.rotation = playerScripts[0].startRotation;
		
		PlayerObjects[1].transform.position = PlayerSpawnPoints[1].position;
		PlayerObjects[1].transform.rotation = playerScripts[1].startRotation;
		
		PlayerObjects[0].GetComponent<FlashSprite>().FlashObject(false);
		ScoreText[0].GetComponent<FlashText>().FlashObject(false);
		PlayerObjects[1].GetComponent<FlashSprite>().FlashObject(false);
		ScoreText[1].GetComponent<FlashText>().FlashObject(false);

		if (Scores[0] >= MaxScore || Scores[1] >= MaxScore)
		{
			EndText.enabled = true;
			
			ScoreText[0].gameObject.transform.position = EndText.transform.position + new Vector3(-85f, 0f, 0f);
			ScoreText[1].gameObject.transform.position = EndText.transform.position + new Vector3(85f, 0f, 0f);

			keepScoreVisible = true;
			aso.PlayOneShot(dingSound);
		}
		else
		{
			ScoreText[0].enabled = false;
			ScoreText[1].enabled = false;
			
			yield return new WaitForSeconds(1f);

			playerScripts[0].enabled = true;
			playerScripts[1].enabled = true;


			playerScripts[0].hitSomething = false;
			playerScripts[1].hitSomething = false;
			betweenRounds = false;
		}
	}

	void ResetGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
	}
}
