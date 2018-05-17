using UnityEngine;
using System.Collections;

[System.Serializable]
public class Even_Breathing_Data
{
	public GameObject breathing_target { get; private set; }
	public Vector3 start_position { get; private set; }
	public float target_x_spacing { get; private set; }
	public int num_targets_per_cycle { get; private set; }
	public int num_cycles { get; private set; }

	public Even_Breathing_Data(GameObject breathing_target, Vector3 start_position,
		float target_x_spacing, int num_targets_per_cycle, int num_cycles){
		this.breathing_target = breathing_target;
		this.start_position = start_position;
		this.target_x_spacing = target_x_spacing;
		this.num_targets_per_cycle = num_targets_per_cycle;
		this.num_cycles = num_cycles;
	}
}

