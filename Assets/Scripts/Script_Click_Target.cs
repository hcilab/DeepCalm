using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Script_Click_Target : MonoBehaviour
{

	[Header("Target Type")]
	public bool is_click_target;
	public bool is_moving_target;
	public bool is_dissapearing_target;
	public bool is_attacking_target;

	bool is_damaging_player = false;
	public int seconds_of_damage;
	public float damage_per_second;
	private Vector3 moving_target_final_position;
	public float speed;
	private float step;
	public float life_time;

	bool target_was_clicked = false;

	public Vector3 target_start_position;
	Transform player;
	public float points;
	public Material material_target;
	List<float> vars = new List<float>();
	float time_created;
	Text target_time_remaining_text;
	bool exists = false;

	void Start ()
	{
		if (!is_dissapearing_target) {
			Destroy (GameObject.Find (transform.name + "/Target_Time_Remaining"));
		} else {
			target_time_remaining_text = GameObject.Find (transform.name + "/Target_Time_Remaining/Canvas/Text_Value").
				GetComponent<Text> ();
			target_time_remaining_text.text = life_time.ToString ();
			time_created = Time.time;
			InvokeRepeating ("Count_Down", 1.0f, 1.0f);
		}

		// time_created = Time.time;
		player = GameObject.FindGameObjectWithTag ("player").transform;
		moving_target_final_position = player.position + target_start_position;
	}
		
	public void Configure(Click_Target_Data click_Target_Data){

	}
	
	// Update is called once per frame
	void Update ()
	{
		step = speed * Time.deltaTime;
		if (is_moving_target) {
			transform.position += new Vector3 (0, step, 0);
		} else if (is_dissapearing_target) {
			transform.position += new Vector3 (step, -step, step);
		}

		if (is_attacking_target) {
			if (transform.position != player.position) {
				Move_Towards_Player ();
			} 
			if (!is_damaging_player && Mathf.Abs(Vector3.Distance(
				transform.position, player.position)) < player.localScale.x) {
				Debug.Log ("Target is going to start damaging player");
				is_damaging_player = true;
				InvokeRepeating("Damage_Player", 0, 1);
			}
		}
		Destroy_If_Missed ();
	}

	void Destroy_If_Missed(){
		if(Vector3.Distance(transform.position, player.position) > 200.0f){
			Destroy(gameObject);
		}
	}

	void Damage_Player(){
		if (seconds_of_damage >= 0) {
			Debug.Log ("Damaging player");
			player.gameObject.SendMessage ("On_Player_Damaged", damage_per_second);
			seconds_of_damage--;
		} else {
			CancelInvoke ();
			Destroy (gameObject);
		}
	}

	void Count_Down(){
		life_time -= 1.0f;
		target_time_remaining_text.text = life_time.ToString ();
		if (life_time < 0) {
			CancelInvoke ();
			Destroy (gameObject);
		}
	}

	void Move_Towards_Player(){
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, player.position, step);
	}
		
	int Parse_Target_ID_From_Name(){
		int i = transform.name.LastIndexOf ("_");
		string s = transform.name.Substring (i);
		int id = -1;
		if (int.TryParse (s, out id)) {
			return id;
		} else {
			return id;
		}
	}

	public void On_Target_Clicked(){
		target_was_clicked = true;
		if (is_click_target) {
			Debug.Log ("Going to try adding " + points + " points to player");
			player.gameObject.SendMessage ("Add_Health", points);
		}
		if (is_attacking_target) {
			if(IsInvoking("Damage_Player")){
				CancelInvoke ();
			}
		}
		else if (is_dissapearing_target) {
			if(IsInvoking("Count_Down")){
				CancelInvoke ();
			}
		}
		Destroy (gameObject);
	}

	void OnDestroy(){
		Debug.Log ("Destroying " + transform.name);
		vars.Add(Parse_Target_ID_From_Name ());
		vars.Add (target_was_clicked ? 1 : -1);
		vars.Add (time_created - Time.time * 1000);
		vars.Add (is_click_target ? 1 : -1);
		vars.Add (is_moving_target ? 1 : -1);
		vars.Add (is_dissapearing_target ? 1 : -1);
		vars.Add (is_attacking_target ? 1 : -1);

		if (is_click_target) {
			Debug.Log ("Broadcasting to Click_Target_Generator");
			GameObject.Find ("Click_Target_Generator").BroadcastMessage ("On_Click_Target_Destroyed", vars);
		} else if (is_moving_target) {
			Debug.Log ("Broadcasting to Moving_Target_Generator");
			GameObject.Find ("Moving_Target_Generator").BroadcastMessage ("On_Click_Target_Destroyed", vars);
		} else if(is_dissapearing_target){
			Debug.Log ("Broadcasting to Dissapearing_Target_Generator");
			GameObject.Find ("Dissapearing_Target_Generator").BroadcastMessage ("On_Click_Target_Destroyed", vars);
		} else if(is_attacking_target){
			Debug.Log ("Broadcasting to Attacking_Target_Generator");
			GameObject.Find ("Attacking_Target_Generator").BroadcastMessage ("On_Click_Target_Destroyed", vars);
		}
		// TODO: send data to wanted script via Broadcast
	}
}

