using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Script_Player_Flex_Sensor : MonoBehaviour
{
	CharacterController character_controller;

	// character_settings
	public float Speed_Horizontal;
	public float speed_vertical;
	private float score;
	public float max_health;
	float health = 0.0f;

	bool jumping = false;
	bool grounded;

	float player_height = 10;
	[Range(1,20)]
	float jump_height = 30;
	float jump_start = 0;
	float y = 0.0f;
	float new_y;
	bool run = false;
	public bool started { get; private set; }
	public float start_x_position { get; private set; }
	private GameObject game_controller;
	void Start (){
		y = Input.mousePosition.y;
		new_y = y;
		score = 0;
		health = max_health;
		started = false;
		game_controller = GameObject.Find ("Game_Controller");
		//character_controller = gameObject.GetComponent<CharacterController> ();
	}

	void Update (){
		string msg_update = "\nScript_Player_Flex_Sensor\n";

		if (started) {
			gameObject.GetComponent<CharacterController> ().Move (new Vector3 (Speed_Horizontal * Time.deltaTime, 0, 0));
		}

		if (Input.anyKey) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				Debug.Log ("Space bar down");
				Speed_Horizontal = Speed_Horizontal * -1;
			}
		} 
		Debug.Log (msg_update);

		if (game_controller.GetComponent<Script_Game_Controller> ().current_level == 0) {
			if (score >= 100) {
				started = false;
				game_controller.SendMessage ("On_Training_Completed");
			}
		}
	}

	public void OnTriggerEnter(Collider other){
		string ote_msg = "OnTriggerEnter()\n";
		ote_msg = string.Format ("Collision with Object: {0}\n", transform.name);

		if (started && other.transform.name != this.transform.name && (!other.transform.tag.Equals("click_target"))) {
			ote_msg += string.Format ("Name of object that hit me: {0}\n", other.transform.name);
			ote_msg += string.Format ("Destroying the object\n");
			Destroy (other.gameObject);
		} else {
			ote_msg += "Collision with object but not destroying";
		}
		Debug.Log (ote_msg);
	}

	public void Start_Moving(){
		Debug.Log ("Script_PLayer_Flex_Sensor: Start_Moving()\n\tSetting started = true");
		started = true;
	}

	public void Stop_Moving(){
		Debug.Log ("Stopping player");
		started = false;
	}

	public void Add_Points_To_Score(float points){
		score += points;
		GameObject.Find ("GUI_Controller").BroadcastMessage ("Update_Score", score);
	}

	public void On_Player_Damaged(float damage){
		health -= damage;
		if (health < 0) {
			health = 0;
			On_Player_Died ();
		} 
		float health_bar_value = (health / max_health);
		GameObject.Find ("GUI_Controller").SendMessage ("Update_Health_Bar", health_bar_value);
	}

	public void Add_Health(float health_to_add){
		string msg = "Adding health to player\n";
		msg += "\tplayer current health: " + health;
		health = (health + health_to_add) % max_health;
		msg += "\n\tupdate_player_health: " + health;
		float health_bar_value = (health / max_health);
		GameObject.Find ("GUI_Controller").SendMessage ("Update_Health_Bar", health_bar_value);
		Debug.Log (msg);
	}

	public void Reset_Health_To_Maximum(){
		health = max_health;
	}

	void On_Player_Died(){
		// TODO: this
		Stop_Moving ();
		GameObject game_controller = GameObject.Find("Game_Controller");
		// game_controller.SendMessage ("Stop_Current_Level");
		game_controller.SendMessage ("Transport_To_Four_Seven_Eight");
	}

}

