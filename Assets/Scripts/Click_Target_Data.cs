using UnityEngine;
using System.Collections;

[System.Serializable]
public class Click_Target_Data
{
	public GameObject click_target { get; private set; }
	public int num_targets_to_generate { get; set; }
	public Vector2 generation_bounds { get; private set; }
	public bool is_click_target { get; private set; }
	public bool is_moving_target { get; private set; }
	public bool is_dissapearing_target { get; private set; }
	public bool is_attacking_target { get; private set; }

	public Click_Target_Data(GameObject click_target, int num_targets_to_generate, 
		Vector2 generation_bounds, bool[] target_types){
		this.click_target = click_target;
		this.num_targets_to_generate = num_targets_to_generate;
		this.generation_bounds = generation_bounds;

		is_click_target = target_types [0];
		is_moving_target = target_types[1];
		is_dissapearing_target = target_types[2];
		is_attacking_target = target_types[3];
		//Debug
	}
}

