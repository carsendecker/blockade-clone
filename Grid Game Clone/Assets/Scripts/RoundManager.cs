using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{

	public int PointMax;
	public Transform P1Spawn, P2Spawn;
	public GameObject Player1, Player2;
	public Text EndText, P1PointText, P2PointText;

	private PlayerMovement p1Script, p2Script;
	private bool betweenRounds = false;
	private int p1Points, p2Points;
	
	// Use this for initialization
	void Start ()
	{
		p1Script = Player1.GetComponent<PlayerMovement>();
		p2Script = Player2.GetComponent<PlayerMovement>();

		Player1.transform.position = P1Spawn.position;
		Player2.transform.position = P2Spawn.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!betweenRounds)
		{
			if (p1Script.hitSomething)
			{
//				P1PointText.gameObject.transform.position = Player1.transform.position;
//				P1PointText.enabled = true;
//				P2PointText.enabled = true;
				StartCoroutine(NewRound());
			}
			else if (p2Script.hitSomething)
			{
//				EndText.enabled = true;
				StartCoroutine(NewRound());
			}
		}
	}

	IEnumerator NewRound()
	{
		betweenRounds = true;
		p1Script.enabled = false;
		p2Script.enabled = false;
		
		yield return new WaitForSeconds(2f);

		GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
		foreach (var wall in walls)
		{
			Destroy(wall);
		}

		Player1.transform.position = P1Spawn.position;
		Player2.transform.position = P2Spawn.position;
		
		yield return new WaitForSeconds(1f);
		
		p1Script.enabled = true;
		p2Script.enabled = true;

		p1Script.hitSomething = false;
		p2Script.hitSomething = false;
		betweenRounds = false;
	}
}
