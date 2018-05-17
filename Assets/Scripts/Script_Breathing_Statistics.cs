using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Script_Breathing_Statistics : MonoBehaviour
{
	public int error_cutt_off;
	int num_targets;
	int num_misses;
	int num_hits;
	int buffer_size;
	Transform player;
	List<List<Vector3[]>> all_data = new List<List<Vector3[]>>();
	int reads_from_producer;

	string msg;
	// Use this for initialization

	void Start () {
		msg = "Script_Breathing_Statistics\n";
		player = GameObject.FindGameObjectWithTag ("player").transform;
		int num_targets = -1;
		int reads_from_producer = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Set_Num_Targets(Transform root){
		num_targets = root.childCount;
	}

	public int Get_Num_Targets(){
		return num_targets;
	}

	public void Set_Buffer_Size(int size){
		this.buffer_size = size;
	}

	public int Get_Buffer_Size(){
		return buffer_size;
	}

	public string Get_Pattern(List<Vector3[]> list){
		Debug.Log ("Get_Pattern");
		string pattern = "";
		float y = 0.0f;
		float x = 0.0f;

		for (int i = 0; i < list.Count; i++) {
			if (list [i] [1].y == 1) {
				pattern += "h";
			} else {
				y = list [i][0].y;
				if (y < 0) {
					pattern += "u";
				} else {
					pattern += "o";
				}
			}
		}
		Debug.Log ("pattern: " + pattern);
		return pattern;
	}

	/*
	public string Get_Max_Error_Direction_From_Buffer(List<Vector3[]> list){
		Vector4 directions = new Vector4 (0, 0, 0, 0);
		for (int i = 0; i < list.Count; i++) {
			// check for a miss
			if (list [i] [2].y == -1) {
				
				for (int j = 0; j < 3; j++) {

				}
			}
		}
	}

	public int Get_Num_Misses_From_Buffer(List<Vector3[]> list){
		int nm = 0;
		for(int i = 0; i < list.Count){
			if(list[i][1].y == -1){
				num_misses++;
			}
		}
		return nm;
	}

	public void On_Read_From_Buffer(List<Vector3> buffer){
		int buff_num_misses = Get_Num_Misses (buffer);
		string max_error_direction = Get_Max_Error_Direction (buffer);
	}
	*/

	public void Read_From_Data_Buffer(List<Vector3[]> data_in){
		string s_msg = "\nRead_From_Data_Buffer\n";
		s_msg += "reads_from_producer: " + reads_from_producer + "\n";
		all_data.Add (data_in);
		s_msg += string.Format ("d_data.Count: {0}\n", data_in.Count);
		s_msg += string.Format ("d_data.Count: {0}\n", data_in.Count);
		reads_from_producer++;
		//On_Read_From_Buffer (data_in);
		Debug.Log("Script_Breathing_Statistics:\n\tRead_From_Data_Producer:\n\t\tCalling Get_Pattern");
		Get_Pattern(data_in);
		Debug.Log(s_msg);
	}

}

