using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Click_Target : MonoBehaviour {


	public Vector3 parent_target_position = new Vector3(50, 0, 0);

	public GameObject prefab_target;
	GameObject parent_target;


	public Material material_target;

	public Texture2D tex_in;

	[Range(0, 1)]
	public float fade_speed;

	private bool move_me = false;
	private bool moving = false;

	bool dissolving;
	bool is_application_quitting;

	// counters
	private int number_of_players; // Is this needed?
	public int ammo;

	// Use this for initialization
	void Start () {
		is_application_quitting = false;
		dissolving = false;
		parent_target = GameObject.Find ("Target");
		gameObject.GetComponent<MeshRenderer> ().material = material_target;
		gameObject.GetComponent<Renderer> ().material.mainTexture = tex_in;
		//parent_target = Instantiate (prefab_target);
	}

	// Update is called once per frame
	void Update () {
		if (gameObject.GetComponent<AudioSource> ().isPlaying) {
			Debug.Log ("Audio is playing");
		}

	}

	void OnApplicationQuit(){
		is_application_quitting = true;
	}

	void OnDestroy(){
		if (!is_application_quitting) {
			
			Debug.Log ("Destroying object");
			GameObject.Find ("Target").GetComponent<AudioSource> ().Play ();
			Dissolve ();
		}
	}
		
	public void Dissolve () {
		Debug.Log("\nController_Prefab_Target.Dissolve()\n");
		Renderer rend = gameObject.GetComponent<Renderer> ();
		//Debug.Log("Value of rend " + rend.ToString());
		while (rend.material.color.a > .004) {
			rend = gameObject.GetComponent<Renderer> ();
			Color color = rend.material.color;
			color.a = 0;
			rend.material.color = Color.Lerp (rend.material.color, color, 300 * Time.deltaTime);
		}
	}

	public void Play_Audio(){
		gameObject.GetComponent<AudioSource> ().Play ();
	}

	public GameObject Spawn_New_Target(){
		GameObject new_target = Instantiate(prefab_target);
		//new_player.GetComponent<Controller_Prefab_Target> ().prefab_target = this.prefab_target;
		new_target.GetComponent<Controller_Click_Target> ().tex_in = parent_target.GetComponent<Controller_Click_Target>().tex_in;
		new_target.GetComponent<Renderer> ().material.mainTexture = tex_in;
		return new_target;
	}

	public int Get_Ammo(){
		return ammo;
	}

	public void Set_Ammo(int qty){
		ammo = qty;
	}

	bool Move_Towards_Target(GameObject target){
		//GameObject target_object = Instantiate(GameObject.Find("Player").GetComponent<Animation_Controller_Player>().prefab_player);
		target.transform.position = target.transform.position;
		target.transform.name = "Target";
		Debug.Log ("Moving towards target");
		float speed = 10f;
		Vector3 target_position = target.transform.position;
		Vector3 current_position = this.transform.position;
		target = target;
		//Debug.Log ("Value of current_position is " + current_position + " Value of target_position is " + target_position);
		float distance_from_target = Vector3.Distance (current_position, target_position);
		Debug.Log ("Value of distance_from_target is " + distance_from_target);
		if (distance_from_target > .1f || distance_from_target < -0.1f) {
			Vector3 direction_of_travel = target_position - current_position;
			direction_of_travel.Normalize ();
			Debug.Log ("Value of direction_of_travel is " + direction_of_travel);
			this.transform.Translate (
				(direction_of_travel.x * speed * Time.deltaTime),
				(direction_of_travel.y * speed * Time.deltaTime),
				(direction_of_travel.z * speed * Time.deltaTime), 
				Space.World);
			return true;
		} else {
			Debug.Log ("Done moving");
			return false;
			//target.transform = gameObject.transform;
			//Destroy (target);
		}

	}
		
}
