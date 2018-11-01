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
	public Text EndText, ResetText;
	public Image EndBackPanel;
	public Text[] ScoreText;
	public GameObject[] Maps;
	
	private AudioSource aso;
	public AudioClip hitSound, dingSound, selectSound;
	

	private PlayerMovement[] playerScripts = new PlayerMovement[2];
	private bool betweenRounds, keepScoreVisible;
	private int[] Scores = new int[2];
	private bool menuOpen = true;
	private int currentMap = 0;

	
	// Use this for initialization
	void Start ()
	{
		playerScripts[0] = PlayerObjects[0].GetComponent<PlayerMovement>();
		playerScripts[1] = PlayerObjects[1].GetComponent<PlayerMovement>();

		PlayerObjects[0].transform.position = PlayerSpawnPoints[0].position;
		PlayerObjects[1].transform.position = PlayerSpawnPoints[1].position;

		aso = GetComponent<AudioSource>();

		SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);

		currentMap = Random.Range(0, Maps.Length);
		Maps[currentMap].SetActive(true);

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
			if (Input.GetKeyDown(KeyCode.Space))
			{
				menuOpen = false;
				playerScripts[0].enabled = true;
				playerScripts[1].enabled = true;
				aso.PlayOneShot(dingSound);
				SceneManager.UnloadSceneAsync("Menu");
			}

			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				Maps[currentMap].SetActive(false);
				currentMap = 0;
				Maps[currentMap].SetActive(true);
				aso.PlayOneShot(selectSound);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				Maps[currentMap].SetActive(false);
				currentMap = 1;
				Maps[currentMap].SetActive(true);
				aso.PlayOneShot(selectSound);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				Maps[currentMap].SetActive(false);
				currentMap = 2;
				Maps[currentMap].SetActive(true);
				aso.PlayOneShot(selectSound);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				Maps[currentMap].SetActive(false);
				currentMap = 3;
				Maps[currentMap].SetActive(true);
				aso.PlayOneShot(selectSound);
			}
		}
		
		
		ScoreText[0].text = Scores[0].ToString();
		ScoreText[1].text = Scores[1].ToString();

		if (betweenRounds) return;
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

		//If a player reaches the max score to win
		if (Scores[0] >= MaxScore || Scores[1] >= MaxScore)
		{
			EndText.enabled = true;
			ResetText.enabled = true;
			EndBackPanel.enabled = true;
			
			ScoreText[0].gameObject.transform.position = EndText.transform.position + new Vector3(-85f, 0f, 0f);
			ScoreText[1].gameObject.transform.position = EndText.transform.position + new Vector3(85f, 0f, 0f);

			keepScoreVisible = true;
			aso.PlayOneShot(dingSound);
		}
		//Otherwise start a new round
		else
		{
			ScoreText[0].enabled = false;
			ScoreText[1].enabled = false;
			
			yield return new WaitForSeconds(1f);

			playerScripts[0].enabled = true;
			playerScripts[1].enabled = true;

			playerScripts[0].hitSomething = false;
			playerScripts[1].hitSomething = false;
			playerScripts[0].BulletCount = playerScripts[0].MaxBullets;
			playerScripts[1].BulletCount = playerScripts[1].MaxBullets;
			betweenRounds = false;
		}
	}

	void ResetGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		//SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
	}
}
