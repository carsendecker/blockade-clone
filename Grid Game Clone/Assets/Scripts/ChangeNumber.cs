using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNumber : MonoBehaviour {

	/*WARNING THIS SCRIPT IS POINTLESS I JUST AM NOT SURE HOW TO GET THIS GAME OBJECT IF ITS IN ANOTHER SCENE AS THE SCRIPT
	 THAT WOULD NORMALLY ACCESS IT AND CHANGE THIS TEXT SO IM SORRY THIS IS HERE BUT I FIND THIS WAY TO BE SIMPLE YET LIKELY THE
	 A LAZY WAY TO DO THIS PLEASE DON'T ROAST ME HAVE A NICE DAY*/

	private Text text;
	
	void Start()
	{
		text = GetComponent<Text>();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			text.text = "Map " + "1";
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			text.text = "Map " + "2";
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			text.text = "Map " + "3";
		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			text.text = "Map " + "4";
		}
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			text.text = "Map " + "5";
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			text.text = "Map " + "6";
		}
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			text.text = "Map " + "Randomized";
		}
	}
}
