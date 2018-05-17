using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System;

public class Script_Breathing_Target_Data_Producer : MonoBehaviour
{
	// attached to breathing controller e.g. Even_Breathing
	[Range(0, 10)]
	public int script_delay;

	Transform player;

	public int buffer_size;

	List<Transform> all_targets;
	List<Transform> transform_buffer;
	List<Vector3[]> data_buffer;

	int current_data_index;
	int current_target_index;

	Transform current_target;

	string msg;

	public bool is_recording { get; set; }
	public bool ready;

	// Use this for initialization

	void Start ()
	{
		msg = "";
		ready = false;
		player = GameObject.FindGameObjectWithTag ("player").transform;

		all_targets = new List<Transform> ();

		transform_buffer = new List<Transform>();
		data_buffer = new List<Vector3[]> ();

		current_data_index = 0;
		current_target_index = 0;

	}

	void Update ()
	{	
		string u_msg = "Update()\n";

		// New plan
		// Once the player has passed the object (2 cases: left or right) record distance data
		if (is_recording) {
			string r_msg = "is_recording";
			r_msg += "\nUpdate() -> ready = true\n";
			r_msg += string.Format("\tcurrent_target_index: {0}\n", current_target_index);
			r_msg += string.Format ("{0} - {1} = {2} (position1.x - position2.x)\n", player.position.x, 
				all_targets [current_target_index].position.x, (player.position.x - all_targets [current_target_index].position.x)); 
			if (player.position.x > all_targets [current_target_index].position.x) {
				Insert_Into_Data_Buffer (all_targets [current_target_index].position - player.position,
					all_targets [current_target_index].name, false);

			}
			r_msg += "\n";
			//Debug.Log (r_msg);
			u_msg += r_msg;
			is_recording = true;
		}
		//Debug.Log (u_msg);
	}

	void Insert_Into_Data_Buffer(Vector3 dist_from_player, string target_name, bool hit){
		// Debug.Log ("current_target_index: " + current_target_index);
		// Debug.Log(string.Format("Insert_Into_Data_Buffer: {0}\t{1}\t{2}\n", dist_from_player, target_name, hit));
		float n = -1f;
		float.TryParse (target_name, out n);
		float h = -1; // false
		if (hit) {
			h = 1;
		}
		Vector3 v = new Vector3 (n, h, -1);
		data_buffer.Add (new Vector3[] {dist_from_player, v});
		if (data_buffer.Count >= buffer_size) {
			data_buffer.RemoveAt(0);
		}
		current_target_index++;
		if ((current_target_index)  % buffer_size == 0) {
			gameObject.BroadcastMessage ("Read_From_Data_Buffer", data_buffer);
			Debug.Log ("Sending buffered data");
			Debug.Log(Vector3_Array_List_To_String(data_buffer));
		}
	}

	public void On_Level_Ended(){
		is_recording = false;
		ready = false;
		gameObject.BroadcastMessage ("Read_From_Data_Buffer", data_buffer);
		data_buffer.Clear ();
		transform_buffer.Clear ();
		all_targets.Clear ();
		transform_buffer.Clear ();
		current_target_index = 0;
		current_data_index = 0;
	}

	public void On_Level_Started(int level_number){
		StartCoroutine(Wait_And_Get(script_delay)); // sets ready when done
		is_recording = true;
	}

	void Record_Hit_Target(List<Vector3> vars){		
		//Debug.Log ("vars[1]: " + vars [1]);
		// Debug.Log("Recording hit target");
		if (vars.Count == 2) {
			bool h = false;
			if (vars [1].y == 1) {
				h = true;
			}
			Insert_Into_Data_Buffer (vars [0], vars[1].x.ToString(), h);
		} else {
			Debug.Log (string.Format("*** Error: vars.Count != 3: vars.Count: {0}\n", vars.Count));
		}
	}

	List<int> Get_Parsed_Target_IDs(List<Transform> targets, ref string r_msg){
		r_msg += "\nParse_Target_ID(Lis<Transform> targets)\n";
		List<int> ids = new List<int> ();
		foreach (Transform t in all_targets) {
			string pattern = @"[0-9]+$";
			r_msg += "t.name: " + t.name + "\n";
				
			string string_id = "";
			string_id = Regex.Match(t.name, pattern).Value; // returns "" if fail;
			r_msg += "\tResult of Regex: [" + string_id + "]";
			if (string_id.Equals("")) {
				r_msg += "\t*** Error:\tcould not find: " + "_-> Returning -1\n";
				ids.Add(-1);
			} else {
				int id = int.Parse(string_id);
				r_msg += "\tSuccessful parse\n\treturning: " + id + "\n";
				if (!ids.Contains (id) && id != -1) {
					ids.Add (id);
				} else {
					r_msg += "\tDUPLICATE -> Skipping\n";
				}
			}
		}
		return ids;
	}

	void Fill_Buffer(int offset, int buffer_size, ref string r_msg){
		r_msg += "\n\tFill_Buffer\n";
		// replaces all elements
		transform_buffer.Clear();
		r_msg += "\t\toffset + window_size: " + buffer_size + "\n";
		int end = offset + buffer_size;
		r_msg += "end: " + end + "\n";
		int n = 0;
		for (int i = offset; i < end; i++) {
			int index = i % all_targets.Count;
			//Debug.Log ("end: " + end);
			//Debug.Log ("i: " + i);
			if(!transform_buffer.Contains((Transform)all_targets [index])){
				transform_buffer.Add ((Transform)all_targets [index]);
				r_msg += "\t\t\tadded: " + transform_buffer[n].name + "\n";
				n++;
			} else {
				r_msg += "\t\t\tDUPLICATE: Skipping\n";
			}
		}
	}

	void Remove_Duplicates(List<Transform> list, ref string r_msg){
		r_msg += "\nRemove_Duplicates\n";
		r_msg += "list.Count: " + list.Count + "\n";
		List<string> covered = new List<string> ();
		List<Transform> t_to_remove = new List<Transform>();
		for (int i = 0; i < list.Count - 1; i++) {
			if (covered.Contains (list [i].name)) {
				t_to_remove.Add (list [i]);
				Destroy (list [i].gameObject);
			} else {
				covered.Add (list [i].name);
			}
		}
			
		for(int i = 0; i < t_to_remove.Count; i++){
			list.Remove (t_to_remove[i]);
		}
		r_msg += "list.Count after prunning: " + list.Count + "\n";
	}

	string Transform_List_To_String(List<Transform> list){
		string msg = "\nlist.length: " + list.Count + "\nList content:";
		for(int i = 0; i < list.Count; i++) {
			msg += string.Format ("\n\tlist[{0}]: \n\tname: {1}\n", i, list[i].name);
		}
		return msg;
	}

	string Vector3_Array_List_To_String(List<Vector3[]> list){
		string s = "Vector3_Array_List_To_String";
		for (int i = 0; i < list.Count; i++) {
			s += string.Format ("list[{0}][0]: {1}", i, list[i][0]);
			s += string.Format ("\tlist[{0}][1]: {1}\n", i, list[i][1]);
		}
		return s;
	}

	void Quicksort_ID(int[] ids, int left, int right, ref string r_msg){
		r_msg += "\nQuicksort_ID\n";
		r_msg += "\tids.Length: " + ids.Length + "\tleft: " + left + "\tright: " + right + "\n";;
		int i = left;
		int j = right;
		float pivot = ids[(left + right) / 2];
		while (i <= j) {
			while (ids[i].CompareTo(pivot) < 0) {
				i++;
			}

			while (ids[j].CompareTo(pivot) > 0) {
				j--;
			}

			if (i <= j) {
				int temp = ids[i];
				ids[i] = ids[j];
				ids[j] = temp;
				i++;
				j--;
			}
		}

		if (left < j) {
			Quicksort_ID(ids, left, j,ref r_msg);
		}

		if (i < right) {
			Quicksort_ID(ids, i, right, ref r_msg);
		}
	}

	string Quicksort(List<Transform> elements, int left, int right){
		string msg = "\telements.Count: " + elements.Count + "\tleft: " + left + "\tright: " + right + "\n";;
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
		return msg;
	}

	// Update is called once per frame


	public void Set_Buffer_Size(int buffer_size){
		this.buffer_size = buffer_size;
	}

	public int Get_Buffer_Size(){
		return buffer_size;
	}

	public List<Vector3> Get_Buffer_Error_Data(List<Transform> buffer, ref string msg){
		// work with window
		return new List<Vector3>();

	}

	IEnumerator Wait_And_Get(float delay){
		string w_msg = "Wait_For(int sec, GameObject[])\n";
		Debug.Log ("Waiting");
		yield return new WaitForSeconds (delay);
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("breathing_target")){
			this.all_targets.Add (g.transform);
			w_msg += "\tadded: " +g.transform.name;
		}
		w_msg += "Before sorting:" + Transform_List_To_String (this.all_targets) + "\n";
		Quicksort (this.all_targets, 0, this.all_targets.Count - 1);
		Remove_Duplicates (this.all_targets, ref msg);
		w_msg += "After sorting: " + Transform_List_To_String (this.all_targets) + "\n";

		//Debug.Log ("Done waiting\n" + w_msg);
	}
}

