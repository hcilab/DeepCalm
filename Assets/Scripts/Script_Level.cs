using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Script_Level : MonoBehaviour
{
	GameObject GUI_controller;
	public string dialogue_text;
	public string dialogue_header;
	[Header("Even Breathing Settings")]
	public bool has_even_breathing;
	public Vector3 even_breathing_start_position;
	public GameObject prefab_breathing_target;
	public float even_breathing_target_x_spacing;
	public int even_breathing_num_targets_per_cycles;
	public int even_breathing_num_cycles;
	public Even_Breathing_Data even_breathing_data { get; private set; }

	[Header("Click Target Settings")]
	public bool has_click_target;
	public GameObject prefab_click_target;
	public float click_target_health_added;
	public int num_click_targets_to_generate;
	public Vector2 click_target_generation_bounds;

	[Header("Moving Target Settings")]
	public bool has_moving_target;
	public GameObject prefab_moving_target;
	public float moving_target_health_added;
	public float moving_target_speed;
	public int num_moving_targets_to_generate;
	public Vector2 moving_target_generation_bounds;

	[Header("Dissapearing Target Settings")]
	public bool has_dissapearing_target;
	public GameObject prefab_dissapearing_target;
	public float dissapearing_target_health_added;
	public float dissapearing_target_speed;
	public float life_time;
	public int num_dissapearing_targets_to_generate;
	public Vector2 dissapearing_target_generation_bounds;

	[Header("Attacking Target Settings")]
	public bool has_attacking_target;
	public GameObject prefab_attacking_target;
	public float attacking_target_health_added;
	public float attacking_target_speed;
	public int seconds_of_damage;
	public float damage_per_second;
	public int num_attacking_targets_to_generate;
	public Vector2 attacking_target_generation_bounds;

	bool is_click_target = false;
	bool is_moving_target = false; // type 0
	bool is_dissapearing_target = false; // type 1
	bool is_attacking_target = false;

	private bool[] target_types;
	public Vector3 target_start_position;
	private Vector3 moving_target_final_position;

	private Click_Target_Data click_target_data;
	public Click_Target_Generator_Data click_target_generator_data { get; private set; }
	public int level_number;

	// Use this for initialization
	void Start ()
	{	
		GUI_controller = GameObject.Find ("GUI_Controller");
		target_types = new bool[] { is_click_target, is_moving_target, is_dissapearing_target, is_attacking_target };
		even_breathing_data = null;
		click_target_data = null;
	}
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Show_Level_Dialogue(){
		// if you want to update the GUI objects, do it here with Broadcasts to GUI controller
		// has to update the position
		GUI_controller.BroadcastMessage ("Update_Dialogue_Text", dialogue_text);
		GUI_controller.BroadcastMessage ("Update_Dialogue_Header", dialogue_header);
		GUI_controller.SendMessage ("Show", GameObject.Find ("GUI_Dialogue"));
		GUI_controller.SendMessage ("Show", GameObject.FindGameObjectWithTag ("GUI_score"));
		GUI_controller.SendMessage ("Update_Button_Option_0_Text", "Start");
	}

	public void Start_Level(){
		Debug.Log ("Script_Level: Start_Level(): level_number: " + level_number);
		if (level_number == 0) {
			Debug.Log ("Sending message to GUI_controller to hide GUI_health_bar");
			GUI_controller.SendMessage ("Hide", GameObject.FindGameObjectWithTag ("GUI_health_bar"));
		} else {
			GUI_controller.SendMessage ("Show", GameObject.FindGameObjectWithTag ("GUI_health_bar"));
		}

		if (has_even_breathing) {
			GameObject controller_even_breathing = GameObject.Find ("Even_Breathing_Controller");
			even_breathing_data = new Even_Breathing_Data (prefab_breathing_target, transform.position + even_breathing_start_position,
				even_breathing_target_x_spacing, even_breathing_num_targets_per_cycles,
				even_breathing_num_cycles);
			controller_even_breathing.SendMessage ("Configure", even_breathing_data);
			controller_even_breathing.SendMessage ("Populate_Targets");
		}

		if (has_click_target) {
			GameObject click_target_generator = GameObject.Find ("Click_Target_Generator");
			is_click_target = true;
			Click_Target_Data click_target_data = new Click_Target_Data(prefab_click_target, num_click_targets_to_generate, click_target_generation_bounds, target_types);
			click_target_generator_data = new Click_Target_Generator_Data (click_target_data, num_click_targets_to_generate);
			click_target_generator.SendMessage ("Configure", click_target_generator_data);
			click_target_generator.SendMessage ("Wait_And_Spawn");
			is_click_target = false;
		}

		if (has_moving_target) {
			GameObject moving_target_generator = GameObject.Find ("Moving_Target_Generator");
			is_moving_target = true;
			Click_Target_Data moving_target_data = new Click_Target_Data(prefab_moving_target, num_moving_targets_to_generate, moving_target_generation_bounds, target_types);
			Click_Target_Generator_Data moving_target_generator_data = new Click_Target_Generator_Data (moving_target_data, num_moving_targets_to_generate);
			moving_target_generator.SendMessage ("Configure", moving_target_generator_data);
			moving_target_generator.SendMessage ("Wait_And_Spawn");
			is_moving_target = false;
		}

		if (has_dissapearing_target) {
			GameObject dissapearing_target_generator = GameObject.Find ("Dissapearing_Target_Generator");
			is_dissapearing_target = true;
			Click_Target_Data dissapearing_target_data = new Click_Target_Data(prefab_dissapearing_target, num_dissapearing_targets_to_generate, dissapearing_target_generation_bounds, target_types);
			Click_Target_Generator_Data dissapearing_target_generator_data = new Click_Target_Generator_Data (dissapearing_target_data, num_dissapearing_targets_to_generate);
			dissapearing_target_generator.SendMessage ("Configure", dissapearing_target_generator_data);
			dissapearing_target_generator.SendMessage ("Wait_And_Spawn");
			is_dissapearing_target = false;
		}

		if (has_attacking_target) {
			GameObject attacking_target_generator = GameObject.Find ("Attacking_Target_Generator");
			is_attacking_target = true;
			Click_Target_Data attacking_target_data = new Click_Target_Data(prefab_attacking_target, num_attacking_targets_to_generate, attacking_target_generation_bounds, target_types);
			Click_Target_Generator_Data attacking_target_generator_data = new Click_Target_Generator_Data (attacking_target_data, num_attacking_targets_to_generate);
			attacking_target_generator.SendMessage ("Configure", attacking_target_generator_data);
			attacking_target_generator.SendMessage ("Wait_And_Spawn");
			is_attacking_target = true;
		}
			
		GameObject.Find ("Statistics_Controller").SendMessage ("On_Level_Started", level_number);
		GameObject.FindGameObjectWithTag ("player").SendMessage ("Start_Moving");
		GameObject.FindGameObjectWithTag ("player").SendMessage ("On_Level_Started", level_number);
	}

	public void Stop_Level(){
		GameObject.Find ("Statistics_Controller").SendMessage ("On_Level_Ended");
		GameObject.FindGameObjectWithTag ("player").SendMessage ("On_Level_Ended");
	}

}

