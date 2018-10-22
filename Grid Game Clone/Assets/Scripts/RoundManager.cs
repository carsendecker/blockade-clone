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

	private PlayerMovement[] playerScripts = new PlayerMovement[2];
	private bool betweenRounds = false;
	private int[] Scores = new int[2];
	
	// Use this for initialization
	void Start ()
	{
		playerScripts[0] = PlayerObjects[0].GetComponent<PlayerMovement>();
		playerScripts[1] = PlayerObjects[1].GetComponent<PlayerMovement>();

		PlayerObjects[0].transform.position = PlayerSpawnPoints[0].position;
		PlayerObjects[1].transform.position = PlayerSpawnPoints[1].position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			ResetGame();
		}

		ScoreText[0].text = Scores[0].ToString();
		ScoreText[1].text = Scores[1].ToString();
		
		if (!betweenRounds)
		{
			if (playerScripts[0].hitSomething)
			{
				Scores[0]++;
				ScoreText[0].enabled = true;
				ScoreText[1].enabled = true;
			
				StartCoroutine(NewRound());
			}
			else if (playerScripts[1].hitSomething)
			{
				Scores[1]++;
				ScoreText[0].enabled = true;
				ScoreText[1].enabled = true;
				
				StartCoroutine(NewRound());
			}
		}
	}

	IEnumerator NewRound()
	{
		betweenRounds = true;
		playerScripts[0].enabled = false;
		playerScripts[1].enabled = false;
		
		yield return new WaitForSeconds(2f);

		GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
		foreach (var wall in walls)
		{
			Destroy(wall);
		}
		

		PlayerObjects[0].transform.position = PlayerSpawnPoints[0].position;
		PlayerObjects[1].transform.position = PlayerSpawnPoints[1].position;

		if (Scores[0] >= MaxScore || Scores[1] >= MaxScore)
		{
			EndText.enabled = true;
			
			ScoreText[0].gameObject.transform.position = EndText.transform.position + new Vector3(-60f, 0f, 0f);
			ScoreText[1].gameObject.transform.position = EndText.transform.position + new Vector3(60f, 0f, 0f);
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
	}
}
