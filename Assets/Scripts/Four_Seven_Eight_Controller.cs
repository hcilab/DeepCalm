using UnityEngine;
using System.Collections;

public class Four_Seven_Eight_Controller : MonoBehaviour
{
	Transform player;
	// Use this for initialization
	// length of it is 183
	float lenght = 670f;
	float end_position_x;
	float start_position_x = 1130;
	public GameObject prefab_4_7_8;
	int return_level = 0;
	bool started_relaxing;
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("player").transform;
	}

	// Update is called once per frame
	void Update ()
	{
		if ((player.position.x > end_position_x) && started_relaxing) {
			Return_To_Level ();
		}

	}

	public void Start_Relaxing(int current_level){
		GameObject new_four_seven_eight = GameObject.Find ("Prefab_4_7_8"); //Instantiate (prefab_4_7_8);
		player.SendMessage ("On_Level_Ended");
		new_four_seven_eight.transform.SetParent (transform);
		new_four_seven_eight.transform.position = new Vector3 (start_position_x, -10, 0);
		GameObject.Find ("Main_Camera").transform.position += new Vector3 (0, 10, 0);
		player.position = transform.position;
		player.position -= new Vector3 (-5, 0, 0);
		started_relaxing = false;
		return_level = current_level;
		start_position_x = this.transform.position.x - 5.0f;
		end_position_x = start_position_x + lenght;
		GameObject GUI_controller = GameObject.Find ("GUI_Controller");
		GUI_controller.SendMessage("Hide", GameObject.Find("GUI_Health_Bar"));
		GUI_controller.SendMessage ("Update_Dialogue_Position", player.position);
		GUI_controller.SendMessage ("Update_Dialogue_Header", "4-7-8 Breathing");
		GUI_controller.SendMessage ("Update_Button_Option_0_Text", "Start");
		string s = "Let's take a breather. 4-7-8 breathing helps you calm down when you get stressed out. " +
		           "To do it, take a strong breath in, hold it, and then breath out slowly." +
		           "Take a few deep breaths first and, when you're ready, click \"Start\" to begin.";
		GUI_controller.SendMessage ("Update_Dialogue_Text", s);
		GUI_controller.SendMessage ("Show", GameObject.Find ("GUI_Dialogue"));
		started_relaxing = true;
	}

	void Return_To_Level(){
		GameObject.Find ("Main_Camera").transform.position += new Vector3 (0, -10, 0);
		started_relaxing = false;
		GameObject.Find ("Game_Controller").SendMessage ("Change_Level", return_level);
	}
}

