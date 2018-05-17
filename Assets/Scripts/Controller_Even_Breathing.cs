using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Controller_Even_Breathing : MonoBehaviour
{	
	public GameObject Prefab_Target;

	float i;

	Even_Breathing_Data even_breathing_data;

	void Start ()
	{
		even_breathing_data = null;
	}
	
	void Update ()
	{
	
	}

	// needs start position, target spacing, num cycles, num targets per cycle
	public void Configure(Even_Breathing_Data even_breathing_data){
		this.even_breathing_data = even_breathing_data;
	}

	int Get_Num_Targets_To_Plot(){
		return even_breathing_data.num_cycles * even_breathing_data.num_targets_per_cycle;
	}

	// return: sin(y in radians)
	float Get_Sin(float y, int id, float[] vars){

		string msg = "\nGet_Sin_Position(Vector3 p)";
		msg += "\tid: " + id;
		msg += "\ty = vars[0] = " + vars [0] + "\n";

		// get num targets needed for 360
		float angle_step = ((float)360 / even_breathing_data.num_targets_per_cycle);
		msg += "angle_step: " + angle_step + "\n";
		float angle = (float)(angle_step * vars[0]) % 360;
		msg += "\tangle: " + angle + "\n";

		// convert from degrees to radians
		y = (float)Mathf.Sin(angle * (Mathf.PI / 180));
		msg += "\ty(radians): " + y;
		//Debug.Log (msg);
		return y;
	}

	public void Populate_Targets(){
		string msg = "\nPopulate_Targets(GameObject parent)\n";
		float mult = 1.0f;
		float i = 0.0f;

		for (int j = 0; j < even_breathing_data.num_cycles; j++) {
			for(int k = 0; k < even_breathing_data.num_targets_per_cycle; k++){
				//Debug.Log ("Creathing new target");
				int id = 0;
				float x = 0.0f;
				float y = 0.0f;
				if (j > 0) {
					id = j;
				}
				if (k > 0) {
					if (j > 0) {
						id = (j * even_breathing_data.num_targets_per_cycle) + k;
					} else {
						id = k;
					}
				}
				// new_target.name = "target_" + i;
				float[] vars = new float[] { j, k };

				// Spawn_New_Target: returns -> setParent, startPosition, name
				GameObject new_target = Spawn_New_Target(vars, id);
				// Get horizontal coordinates
				x = new_target.transform.position.x + (even_breathing_data.target_x_spacing * id);
				msg += ("\tSpacing.x * id = \n" + even_breathing_data.target_x_spacing * id + "\n");

				vars = new float[] { k, j };
				msg += "\tx: " + x + "\n";

				y = new_target.transform.position.y * Get_Sin(new_target.transform.position.y, id, vars);
				msg += "\tValue of y: " + y + "\n";

				// Set posiiton
				new_target.transform.position = new Vector3 (x, y, new_target.transform.position.z);
			}
		}
		Debug.Log (msg);
	}
		
	// returns: updated new_target at start position
	GameObject Spawn_New_Target(float[] vars, int id){
		string msg = "\nSpawn_New_Target\n";
		GameObject new_target = Instantiate (even_breathing_data.breathing_target);
		new_target.transform.SetParent(transform);
		new_target.transform.position = even_breathing_data.start_position;
		new_target.GetComponent<Script_Target_Breathing> ().Set_ID (id);
		msg += "\tcycle_ " +vars[0] + "\n\ttarget_" + vars[1];
		new_target.name = id.ToString();
		//Debug.Log (msg);
		return new_target;
	}
}

