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
	public Text EndText, ResetText, MapText, SuddenDeathText;
	public Image EndBackPanel;
	public Transform[] PlayerSpawnPoints;
	public GameObject[] PlayerObjects;
	public Text[] ScoreText;
	public GameObject[] Maps;
	
	private AudioSource aso, musicAso;
	public AudioClip hitSound, dingSound, selectSound, suddenDeathSound;
	private PlayerMovement[] playerScripts = new PlayerMovement[2];
	private bool betweenRounds, keepScoreVisible;
	private int[] Scores = new int[2];
	private bool menuOpen = true;
	private int currentMap = 0;
	private int suddenDeathAmount;
	private bool suddenDeathMode;
	private GameObject[] terrain;

	
	// Use this for initialization
	void Start ()
	{
		playerScripts[0] = PlayerObjects[0].GetComponent<PlayerMovement>();
		playerScripts[1] = PlayerObjects[1].GetComponent<PlayerMovement>();

		PlayerObjects[0].transform.position = PlayerSpawnPoints[0].position;
		PlayerObjects[1].transform.position = PlayerSpawnPoints[1].position;

		aso = GetComponent<AudioSource>();
		musicAso = GameObject.Find("Music").GetComponent<AudioSource>();

		SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);

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
				terrain = GameObject.FindGameObjectsWithTag("Terrain");
				SceneManager.UnloadSceneAsync("Menu");

				if (suddenDeathMode)
				{
					BeginSuddenDeath();
					SuddenDeathText.enabled = false;
				}
			}

			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				Maps[currentMap].SetActive(false);
				currentMap = 0;
				Maps[currentMap].SetActive(true);
				aso.PlayOneShot(selectSound);
				MapText.text = "Map 1";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				Maps[currentMap].SetActive(false);
				currentMap = 1;
				Maps[currentMap].SetActive(true);
				aso.PlayOneShot(selectSound);
				MapText.text = "Map 2";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				Maps[currentMap].SetActive(false);
				currentMap = 2;
				Maps[currentMap].SetActive(true);
				aso.PlayOneShot(selectSound);
				MapText.text = "Map 3";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				Maps[currentMap].SetActive(false);
				currentMap = 3;
				Maps[currentMap].SetActive(true);
				aso.PlayOneShot(selectSound);
				MapText.text = "Map 4";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				Maps[currentMap].SetActive(false);
				currentMap = 4;
				Maps[currentMap].SetActive(true);
				aso.PlayOneShot(selectSound);
				MapText.text = "Map 5";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				Maps[currentMap].SetActive(false);
				currentMap = 5;
				Maps[currentMap].SetActive(true);
				aso.PlayOneShot(selectSound);
				MapText.text = "Map 6";
			}
			else if (Input.GetKeyDown(KeyCode.Alpha7))
			{
				Maps[currentMap].SetActive(false);
				int previousMap = currentMap;
				while (currentMap == previousMap)
				{
					currentMap = Random.Range(0, Maps.Length);
				}
				Maps[currentMap].SetActive(true);
				aso.PlayOneShot(selectSound);
				MapText.text = "Map Randomized";
			}

			if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				if (!suddenDeathMode)
				{
					suddenDeathMode = true;
					SuddenDeathText.enabled = true;
					aso.PlayOneShot(suddenDeathSound);
				}
				else if (suddenDeathMode)
				{
					suddenDeathMode = false;
					SuddenDeathText.enabled = false;
					aso.PlayOneShot(selectSound);
				}
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

	//There's a lot in this coroutine :O
	IEnumerator NewRound()
	{	
		betweenRounds = true;
		playerScripts[0].enabled = false;
		playerScripts[1].enabled = false;
		yield return new WaitForSeconds(2.5f);

		GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
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
			//In the event of a tie
			if (Scores[0] == Scores[1])
			{
				if (!suddenDeathMode)
				{
					EndText.color = new Color(1f, 0.22f, 0.28f);
					EndText.text = "SUDDEN\nDEATH";
				}
				else
				{
					EndText.color = new Color(1f, 0.97f, 0.04f);
					EndText.text = "SUDDEN-ER\nDEATH";
				}
				
				EndText.enabled = true;
				EndBackPanel.enabled = true;
				aso.PlayOneShot(suddenDeathSound);
				musicAso.Stop();
				
				yield return new WaitForSeconds(2.5f);
				
				EndBackPanel.enabled = false;
				BeginSuddenDeath();
			}
			else
			{
				EndText.text = "GAME\nOVER";
				EndText.enabled = true;
				ResetText.enabled = true;
				EndBackPanel.enabled = true;

				ScoreText[0].gameObject.transform.position = EndText.transform.position + new Vector3(-90f, 0f, 0f);
				ScoreText[1].gameObject.transform.position = EndText.transform.position + new Vector3(90f, 0f, 0f);

				keepScoreVisible = true;
				aso.PlayOneShot(dingSound);
			}
		}
		//Otherwise start a new round
		else
		{
			//this whole sudden death mode thing is kind of spaghetti code, added it all in kind of late but its fun!
			if (suddenDeathMode)
			{
				musicAso.pitch = 1.2f + (suddenDeathAmount * 0.05f);
				foreach (var player in playerScripts)
				{
					player.MoveSpeed = 0.1f - (suddenDeathAmount * 0.02f);
					player.MoveSpeed = Mathf.Clamp(player.MoveSpeed, 0.04f, 0.2f);
				}
				suddenDeathAmount += 1;
			}
			
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

	void BeginSuddenDeath()
	{
		musicAso.pitch = 1.2f + (suddenDeathAmount * 0.05f);
		musicAso.Play();
		EndText.enabled = false;

		foreach (var wall in terrain)
		{
				wall.GetComponent<SpriteRenderer>().color = new Color(1f, 0.22f, 0.28f);
		}

		foreach (var player in playerScripts)
		{
			player.enabled = true;
			player.hitSomething = false;
			player.BulletCount = player.MaxBullets;
			player.MoveSpeed = 0.1f - (suddenDeathAmount * 0.02f);
			player.MoveSpeed = Mathf.Clamp(player.MoveSpeed, 0.04f, 0.2f);
		}

		suddenDeathAmount += 1;
		betweenRounds = false;

	}
	
	void ResetGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
