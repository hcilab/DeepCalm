using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;


public class Controller_Generate_Click_Targets: MonoBehaviour
{
	/*
	 * 	assumptions
	 * 	Is attached to GameObject Contrller_Scene
	 */
	
	public GameObject Prefab_Target;

	// vals for curve
	public int Num_Cycles;
	public int Num_Targets_Per_Cycle;


	// vals for sin_wave
	[Range(0, 20)]
	public float Amp_x;
	[Range(0, 20)]
	public float Amp_y;
	[Range(0, 5)]
	public float Omega_x;
	[Range(0, 5)]
	public float Omega_y;
	float i;
	float[] Vals_For_Sin;

	public Vector3 Start_Position_Target;
	public Vector3 Spacing_Target;
	public Vector3 target_player_offset_upper_bound;
	public Vector3 target_player_offset_lower_bound;
	int target_id;

	// Use this for initialization
	void Start ()
	{
		// Error checking start
		Vals_For_Sin = new float[] { Amp_x, Amp_y, Omega_x, Omega_y };
		if (Num_Targets_Per_Cycle % 4 != 0) {
			Debug.Log ("\n*** Error:\tNum_Target_Per_Cycle % 4 != 0 -> Fix in editor");
		}
		if (Start_Position_Target == new Vector3 ()) {
			Debug.Log ("\n*** Error:\tStart_Position_Target cannot be zero");
		}

		// End error checking
		target_id = 0;
		Spawn_New_Target(GameObject.FindGameObjectWithTag ("player").transform, target_id);
	}
	
	// Update is called once per frame
	void Update ()
	{	
		Start_Position_Target = GameObject.FindGameObjectWithTag ("player").transform.position;
		// Check for target
		GameObject[] targets_to_click = GameObject.FindGameObjectsWithTag("click_target");
		if (targets_to_click.Length > 1) {
			Debug.Log ("There are targets to click");
		} else {
			target_id++;
			GameObject new_target = Spawn_New_Target (GameObject.FindGameObjectWithTag ("player").transform, target_id);
		}
	}

	int Get_Num_Targets_To_Plot(){
		return Num_Cycles * Num_Targets_Per_Cycle;
	}

	// return: sin(y in radians)
	float Get_Sin_Test(float y, int id, float[] vars){

		string msg = "\nGet_Sin_Position(Vector3 p)";
		msg += "\tid: " + id;
		msg += "\ty = vars[0] = " + vars [0] + "\n";

		// get num targets needed for 360
		float angle_step = ((float)360 / Num_Targets_Per_Cycle);
		msg += "angle_step: " + angle_step + "\n";
		float angle = (float)(angle_step * vars[0]) % 360;
		msg += "\tangle: " + angle + "\n";

		// convert from degrees to radians
		y = (float)Mathf.Sin(angle * (Mathf.PI / 180));
		msg += "\ty(radians): " + y;
		//Debug.Log (msg);
		return y;
	}
		
	// return: sin(y in radians)
	float Get_Sin(float y, int id, float[] vars){

		string msg = "\nGet_Sin_Position(Vector3 p)";
		msg += "\tid: " + id;
		msg += "\ty = vars[0] = " + vars [0] + "\n";

		// get num targets needed for 360
		float angle_step = ((float)360 / Num_Targets_Per_Cycle);
		msg += "angle_step: " + angle_step + "\n";
		float angle = (float)(angle_step * vars[0]) % 360;
		msg += "\tangle: " + angle + "\n";

		// convert from degrees to radians
		y = (float)Mathf.Sin(angle * (Mathf.PI / 180));
		msg += "\ty(radians): " + y;
		Debug.Log (msg);
		return y;
	}

	/* public Class new_target_return{
		GameObject go { get; set; }
		protected internal GameObject go{
			get { return go; }
		}
		string msg;


			
		public new_target_return(GameObject g, string m){
			go = g;
			msg = m;
		}


	}*/

	// returns: updated new_target at start position
	GameObject Spawn_New_Target(Transform parent, int num){
		string msg = "\nSpawn_New_Target\n";
		GameObject new_target = Instantiate (Prefab_Target);
		//Training_Controller_Target tct = Prefab_Target.GetComponent<Training_Controller_Target> ();
		new_target.transform.SetParent(Prefab_Target.transform);
		System.Random r = new System.Random ();
		float x = (float)r.Next ((int)target_player_offset_lower_bound.x, (int)target_player_offset_upper_bound.x);
		float y = (float)r.Next ((int)target_player_offset_lower_bound.y, (int)target_player_offset_upper_bound.y);
		new_target.transform.position = Start_Position_Target;
		new_target.transform.position += new Vector3 (x, y, 0);
		//new_target.GetComponent<Renderer> ().material.mainTexture = Prefab_Target.GetComponent<Controller_Prefab_Target>().Tex_In;
		new_target.name = "Click_Target " + num;
		//Debug.Log (msg);
		return new_target;
	}
}

