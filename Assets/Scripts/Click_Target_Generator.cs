using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Click_Target_Generator : MonoBehaviour
{
	public int num_targets_generated = 0;
	public int num_targets_to_generate = 0;

	int seconds_of_damage;
	float damage_per_second;
	float attacking_target_speed;
	float moving_target_speed;
	float dissapearing_target_life_time;
	public Vector2 wait_range;

	public bool is_click_target_generator;
	public bool is_moving_target_generator;
	public bool is_dissapearing_target_generator;
	public bool is_attacking_target_generator;

	private GameObject click_target;
	private Click_Target_Data click_target_data;
	private bool stopped_generating = false;
	Script_Game_Controller script_game_controller;
	// Use this for initialization
	void Start ()
	{
		script_game_controller = GameObject.Find ("Game_Controller").GetComponent<Script_Game_Controller> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Stop_Generating(){
		stopped_generating = true;
	}

	public void Configure(Click_Target_Generator_Data click_target_generator_data){
		this.click_target_data = click_target_generator_data.click_target_data;
		click_target = click_target_data.click_target;
		num_targets_to_generate = script_game_controller.levels [script_game_controller.current_level]
			.GetComponent<Script_Level> ().num_attacking_targets_to_generate;
	}

	void Spawn_New_Click_Target(){
		if (!stopped_generating) {
			Debug.Log ("Spawning new Click_Target from: " + transform.name);
			GameObject new_click_target = Instantiate (this.click_target_data.click_target);
			new_click_target.transform.position = Generate_Target_Start_Position ();
			Script_Level script_level = script_game_controller.levels [script_game_controller.current_level]
			.GetComponent<Script_Level> ();
			if (is_click_target_generator) {
				new_click_target.GetComponent<Script_Click_Target> ().points = script_level.click_target_health_added;
				new_click_target.transform.name = "click_target_" + num_targets_generated;
				new_click_target.transform.SetParent (this.transform);
			} else if (is_moving_target_generator) {
				new_click_target.GetComponent<Script_Click_Target> ().is_moving_target = true;
				new_click_target.GetComponent<Script_Click_Target> ().speed = script_level.moving_target_speed;
				new_click_target.GetComponent<Script_Click_Target> ().points = script_level.moving_target_health_added;
				new_click_target.transform.name = "moving_target_" + num_targets_generated;
				new_click_target.transform.SetParent (this.transform);
			} else if (is_dissapearing_target_generator) {
				new_click_target.GetComponent<Script_Click_Target> ().is_dissapearing_target = true;
				new_click_target.GetComponent<Script_Click_Target> ().speed = script_level.dissapearing_target_speed;
				new_click_target.GetComponent<Script_Click_Target> ().points = script_level.dissapearing_target_health_added;
				new_click_target.GetComponent<Script_Click_Target> ().life_time = script_level.life_time;
				new_click_target.transform.name = "dissapearing_target_" + num_targets_generated;
				new_click_target.transform.SetParent (this.transform);
			} else if (is_attacking_target_generator) {
				new_click_target.GetComponent<Script_Click_Target> ().is_attacking_target = true;
				new_click_target.GetComponent<Script_Click_Target> ().speed = script_level.attacking_target_speed;
				new_click_target.GetComponent<Script_Click_Target> ().points = script_level.attacking_target_health_added;
				new_click_target.GetComponent<Script_Click_Target> ().seconds_of_damage = script_level.seconds_of_damage;
				new_click_target.GetComponent<Script_Click_Target> ().damage_per_second = script_level.damage_per_second;
				new_click_target.transform.name = "attacking_target_" + num_targets_generated;
				new_click_target.transform.SetParent (this.transform);
			}
		}
		num_targets_generated++;
	}

	public void On_Click_Target_Destroyed(){
		Debug.Log ("On_Click_Target_Destroyed()\n\tAbout to Wait_And_Spawn");
		click_target_data.num_targets_to_generate--; // doing this because one target at a time
		StartCoroutine(Wait_And_Spawn());
		// TODO: send data to Clicked Target Data producer
	}

	Vector3 Generate_Target_Start_Position(){
		Vector3 player_position = GameObject.FindGameObjectWithTag ("player").transform.position;
		float x = 0.0f;
		float y = 0.0f;
		float z = 0.0f;
		if (is_click_target_generator) {
			x = player_position.x + UnityEngine.Random.Range (click_target_data.generation_bounds.x, click_target_data.generation_bounds.y);
			y = UnityEngine.Random.Range (-20, 20);
		} else if (is_moving_target_generator) {
			// start somewhere in the bottom
			x = player_position.x + (float)UnityEngine.Random.Range (click_target_data.generation_bounds.x, click_target_data.generation_bounds.y);
			y = -25.0f;
		} else if (is_dissapearing_target_generator) {
			// start somewhere in top-left
			x = player_position.x - 100;
			y = UnityEngine.Random.Range (click_target_data.generation_bounds.x, click_target_data.generation_bounds.y);
		} else if (is_attacking_target_generator) {
			x = player_position.x + ((UnityEngine.Random.value > 0.5f ? -1 : 1) * 100);
			y = UnityEngine.Random.Range (click_target_data.generation_bounds.x, click_target_data.generation_bounds.y);
		}

		/*
		int up_down = UnityEngine.Random.Range (0, 1);
		x = (float) UnityEngine.Random.Range((int)player_position.x + click_target_data.generation_bounds.x, (int)player_position.x + click_target_data.generation_bounds.x) ;
		y = (float) UnityEngine.Random.Range(click_target_data.generation_bounds.x, click_target_data.generation_bounds.y) * (up_down == 0 ? -1 : 1);
		z = player_position.z + 10;
		*/
		return new Vector3 (x, y, z);
	}

	public IEnumerator Wait_And_Spawn(){
		Debug.Log ("Waiting for random seconds");
		yield return new WaitForSeconds (UnityEngine.Random.Range(wait_range.x, wait_range.y));
		if (num_targets_generated < click_target_data.num_targets_to_generate) {
			Spawn_New_Click_Target ();
		}
	}
		
}

