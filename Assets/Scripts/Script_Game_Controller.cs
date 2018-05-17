using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Script_Game_Controller: MonoBehaviour
{
	public List<Transform> levels { get; private set; }
	Transform player;
	public int current_level;
	GameObject GUI_controller;
	GameObject even_breathing;
	GameObject click_target_generator;

	void Start ()
	{
		levels = new List<Transform>();
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("level")) {
			levels.Add(go.transform);
		}
		// sort based on x position

		player = GameObject.FindGameObjectWithTag ("player").transform;

		GUI_controller = GameObject.Find ("GUI_Controller");

		even_breathing = GameObject.Find ("Even_Breathing_Controller");

		click_target_generator = GameObject.Find ("Click_Target_Generator");

		Quicksort (levels, 0, levels.Count -1);
	}

	void Update ()
	{
		// switch this to switch(KeyCode)
		if (Input.GetKeyDown (KeyCode.Alpha0)) {
			Change_Level (0);
		} else if (Input.GetKeyDown (KeyCode.Alpha1)) {
			if (levels.Count - 1 >= 1) {
				Change_Level (1);
			}
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			if (levels.Count - 1 >= 2) {
				Change_Level (2);
			}
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			if (levels.Count - 1 >= 3) {
				Change_Level (3);
			}
		} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
			if (levels.Count - 1 >= 4) {
				Change_Level (4);
			}
		} else if (Input.GetKeyDown (KeyCode.Alpha9)) {
			Transport_To_Four_Seven_Eight ();
		}

	}

	public void Transport_To_Four_Seven_Eight(){
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("click_target")) {
			Destroy (go);
		}
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("click_target_generator")) {
			go.SendMessage ("Stop_Generating");
		}
		GameObject.Find ("4_7_8_Controller").SendMessage ("Start_Relaxing", current_level);
	}

	public void Change_Level(int new_level){
		// what has to be done?
		// change player position to the new level position
		// update GUIs
		// record level data
			// stats reads remaining data from buffer
			// producers flushes level data
			// stats saves level data and preps for next level
		// configure Even Breathing
		current_level = new_level;
		Debug.Log ("Transporting to Level " + current_level);
		GUI_controller.SendMessage ("Update_Score", 0.0f);
		player.gameObject.SendMessage ("Stop_Moving");
		if (current_level != 0) {
			player.gameObject.SendMessage ("Reset_Health_To_Maximum");
			GUI_controller.SendMessage ("Update_Health_Bar", 
				player.gameObject.GetComponent<Script_Player_Flex_Sensor> ().max_health);
		}
		Script_Level script_level = levels [current_level].GetComponent<Script_Level> ();
		player.position = levels [current_level].position;
		GUI_controller.SendMessage ("Update_Dialogue_Position", player.position);
		script_level.Show_Level_Dialogue ();
	}

	public void Start_Current_Level(){
		Debug.Log ("Script_Game_Controller: Start_Current_Level()\n\t" +
		"current_level: " + current_level);
		levels [current_level].SendMessage ("Start_Level");
	}

	public void On_Training_Completed(){
		string s = "Well done! You're all trained up and ready for the open air. But be careful. It's dangerous up here.";
		GUI_controller.SendMessage("Update_Dialogue_Position", player.position);
		GUI_controller.SendMessage ("Update_Dialogue_Text", s);
		GUI_controller.SendMessage ("Show", GUI_controller);
		GUI_controller.SendMessage("Update_Button_Option_0_Text", "Ready for next level");
	}

	void Quicksort(List<Transform> elements, int left, int right){
		int i = left;
		int j = right;
		float pivot = elements[(left + right) / 2].position.x;
		while (i <= j) {
			while (elements[i].position.x.CompareTo(pivot) < 0) {
				i++;
			}

			while (elements[j].position.x.CompareTo(pivot) > 0) {
				j--;
			}

			if (i <= j) {
				Transform temp = elements[i];
				elements[i] = elements[j];
				elements[j] = temp;
				i++;
				j--;
			}
		}

		if (left < j) {
			Quicksort(elements, left, j);
		}

		if (i < right) {
			Quicksort(elements, i, right);
		}
	}
}

